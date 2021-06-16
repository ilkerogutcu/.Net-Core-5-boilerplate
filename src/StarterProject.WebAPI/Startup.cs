using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StarterProject.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using StarterProject.Core.Settings;
using StarterProject.Core.Utilities.Encryption;
using StarterProject.Core.Utilities.IoC;
using StarterProject.DataAccess.Concrete.EntityFramework.Contexts;
using StarterProject.Entities.Concrete;

namespace StarterProject.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "StarterProject.WebAPI", Version = "v1"});
            });
            services.AddTransient<FileLogger>();
            services.AddTransient<MongoDbLogger>();
            services.AddTransient<ElasticsearchLogger>();

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();
            services.AddDbContext<IdentityDbContext>();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            });
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["TokenOptions:Issuer"],
                        ValidAudience = Configuration["TokenOptions:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            SecurityKeyHelper.CreateSecurityKey(Configuration["TokenOptions:SecurityKey"])
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceTool.ServiceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StarterProject.WebAPI v1"));
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseRouting();

            //Authentication comes before Authorization.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}