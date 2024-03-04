using Autofac;
using Guppy.Attributes;
using Guppy.Game.MonoGame.Utilities.Cameras;
using Guppy.Loaders;

namespace CellularAutomaton.Core.Loaders
{
    [AutoLoad]
    internal sealed class CoreServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<Camera2D>().As<Camera>().AsSelf().SingleInstance();
        }
    }
}
