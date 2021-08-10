using Core.Utilities.IoC;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.DependencyResolvers
{
    public class DataAccessModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>();
        }
    }
}