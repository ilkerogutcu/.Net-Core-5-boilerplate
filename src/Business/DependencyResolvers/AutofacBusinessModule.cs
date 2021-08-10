using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using FluentValidation;
using MediatR;
using Module = Autofac.Module;

namespace Business.DependencyResolvers
{
    /// <summary>
    ///     Register dependencies for business layer
    /// </summary>
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterType<EfProductRepository>().As<IProductRepository>().SingleInstance();
            builder.RegisterType<AuthenticationMailManager>().As<IAuthenticationMailService>().SingleInstance();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IValidator<>));
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}