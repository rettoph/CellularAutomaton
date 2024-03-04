using Guppy.Attributes;
using Guppy.Game.MonoGame.Graphics.Effects;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework.Graphics;

namespace CellularAutomaton.Core.Graphics.Effects
{
    [AutoLoad]
    public sealed class GridEffect : EffectMatricesEffect
    {
        private EffectParameter _width;
        private EffectParameter _renderAsleep;

        public int Width
        {
            set => _width.SetValue(value);
        }

        public bool RenderAsleep
        {
            set => _renderAsleep.SetValue(value);
        }

        public GridEffect(GraphicsDevice graphicsDevice, IResourceProvider resources) : base(graphicsDevice, resources.Get(Constants.Resources.EffectCodes.Grid).Value)
        {
            _width = this.Parameters[nameof(Width)];
            _renderAsleep = this.Parameters[nameof(RenderAsleep)];
        }
    }
}
