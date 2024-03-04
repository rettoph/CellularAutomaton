using CellularAutomaton.Core;
using CellularAutomaton.FallingSand.Enums;
using Guppy;
using Guppy.Attributes;
using Guppy.Game.Common;
using Guppy.Game.Input;
using Guppy.Game.Input.Messages;
using Guppy.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        public WorldComponent(World<CellData> world, GraphicsDevice graphics, GameWindow window)
        {
            _world = world;
            _graphics = graphics;
            _window = window;

            _world.Initialize(_window.ClientBounds.Width / 10, _window.ClientBounds.Height / 10, false, CellData.Air, Color.White);
        }

        public void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
        }

        public void Process(in Guid messageId, CursorPress message)
        {
            if(message.Value == false && message.Button == Guppy.Game.Input.Enums.CursorButtons.Left)
            {
                _world.Update(null!);
            }

        }

        public void Update(GameTime gameTime)
        {
            return;
            _stepTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            while (_stepTime > 1000)
            {
                _stepTime -= 1000;
                _world.Update(gameTime);
            }
        }
    }
}
