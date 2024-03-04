using Autofac;
using CellularAutomaton.Core;
using CellularAutomaton.FallingSand.Messages;
using CellularAutomaton.FallingSand.Services;
using CellularAutomaton.FallingSand.Services.CellTypeServices;
using Guppy.Attributes;
using Guppy.Loaders;
using Microsoft.Xna.Framework.Input;

namespace CellularAutomaton.FallingSand.Loaders
{
    [AutoLoad]
    internal class FallingSandServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<World<CellData>>().AsSelf().InstancePerLifetimeScope();
            services.RegisterType<CellService>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();

            services.RegisterType<NullCellTypeService>().AsSelf().InstancePerLifetimeScope();

            services.RegisterInput("RenderAsleep", Keys.F3, new[]
            {
                (true, new RenderAsleepInput(true)),
                (false, new RenderAsleepInput(false))
            });
        }
    }
}
