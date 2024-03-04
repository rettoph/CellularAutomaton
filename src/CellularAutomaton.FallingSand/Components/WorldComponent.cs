using CellularAutomaton.Core;
using CellularAutomaton.FallingSand.Enums;
using Guppy;
using Guppy.Attributes;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Components
{
    [AutoLoad]
    [GuppyFilter<FallingSandGuppy>]
    internal unsafe sealed class WorldComponent : GuppyComponent, IGuppyUpdateable, IGuppyDrawable
    {
        private World<CellData> _world;
        private double _stepTime;

        public WorldComponent(World<CellData> world)
        {
            _world = world;

            _world.Initialize(100, 100, false, CellData.Air, Color.White);

            _world.Grid.Cells[0].Latest.Type = CellTypeEnum.Sand;
        }

        public void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            _stepTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            while (_stepTime > 100)
            {
                _stepTime -= 100;
                _world.Update(gameTime);
            }
        }
    }
}
