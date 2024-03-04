using CellularAutomaton.Core;
using Guppy;
using Guppy.Attributes;
using Guppy.Game.Common;
using Guppy.Game.Input;
using Guppy.Game.Input.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CellularAutomaton.FallingSand.Components
{
    [AutoLoad]
    [GuppyFilter<FallingSandGuppy>]
    internal unsafe sealed class WorldComponent : GuppyComponent, IGuppyUpdateable, IGuppyDrawable, IInputSubscriber<CursorPress>
    {
        private readonly GraphicsDevice _graphics;
        private readonly GameWindow _window;
        private readonly World<CellData> _world;
        private double _stepTime;
        private bool _manual;

        public WorldComponent(World<CellData> world, GraphicsDevice graphics, GameWindow window)
        {
            _world = world;
            _graphics = graphics;
            _window = window;

            _world.Initialize(_window.ClientBounds.Width / 2, _window.ClientBounds.Height / 2, false, CellData.Air, Color.White);
            _window.ClientSizeChanged += this.HandleClientSizeChanged;
        }

        public void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
        }

        public void Process(in Guid messageId, CursorPress message)
        {
            if (_manual && message.Value == false && message.Button == Guppy.Game.Input.Enums.CursorButtons.Left)
            {
                _world.Update(null!);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_manual)
            {
                return;
            }

            _world.Update(gameTime);

            //stepTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            //hile (_stepTime > 20)
            //
            //   _stepTime -= 20;
            //   _world.Update(gameTime);
            //
        }

        private void HandleClientSizeChanged(object? sender, EventArgs e)
        {
            _world.Dispose();
            _world.Initialize(_window.ClientBounds.Width / 2, _window.ClientBounds.Height / 2, false, CellData.Air, Color.White);
        }
    }
}
