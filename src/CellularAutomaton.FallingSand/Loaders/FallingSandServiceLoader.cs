using Autofac;
using CellularAutomaton.Core;
using CellularAutomaton.FallingSand.Services;
using CellularAutomaton.FallingSand.Services.CellTypeServices;
using Guppy.Attributes;
using Guppy.Loaders;

namespace CellularAutomaton.FallingSand.Loaders
{
    [AutoLoad]
    internal class FallingSandServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<World<CellData>>().AsSelf().InstancePerLifetimeScope();
            services.RegisterType<CellService>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();

            services.RegisterType<NotImplementedCellTypeService>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
