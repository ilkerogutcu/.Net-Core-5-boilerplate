using Microsoft.Extensions.DependencyInjection;

namespace StarterProject.Core.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}