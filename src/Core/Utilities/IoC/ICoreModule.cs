#region

using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Core.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}