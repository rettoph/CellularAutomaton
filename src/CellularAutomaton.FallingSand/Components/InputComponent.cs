using CellularAutomaton.Core;
using CellularAutomaton.FallingSand.Services;
using Guppy;
using Guppy.Attributes;
using Guppy.Common.Attributes;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Input;
using Guppy.Game.Input.Enums;
using Guppy.Game.Input.Messages;
using Guppy.Game.MonoGame.Extensions.Primitives;
using Guppy.Game.MonoGame.Primitives;
using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.FallingSand.Components
{
    [AutoLoad]
    [Sequence<DrawSequence>(DrawSequence.PostDraw)]
    internal class InputComponent : GuppyComponent, IGuppyDrawable,
        IInputSubscriber<CursorScroll>, 
        IInputSubscriber<CursorMove>,
        IInputSubscriber<CursorPress>
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _pixel;
        private readonly CellService _cells;
        private readonly World<CellData> _world;

        private Rectangle _brush;
        private int _size = 10;

        public InputComponent(SpriteBatch spriteBatch, GraphicsDevice graphics, CellService cells, World<CellData> world)
        {
            _cells = cells;
            _spriteBatch = spriteBatch;
            _world = world;

            _pixel = new Texture2D(graphics, 1, 1);
            _pixel.SetData(new Color[] { Color.White });

            _size = 10;
            _brush.Size = new Point(_size, _size);
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(_pixel, _brush, Color.Red);

            _spriteBatch.End();
        }

        public void Process(in Guid messageId, CursorScroll message)
        {
            _size += message.Delta > 0 ? 1 : -1;
            _brush.Size = new Point(_size, _size);
        }

        public void Process(in Guid messageId, CursorMove message)
        {
            _brush.Location = message.Cursor.Position.ToPoint();
        }

        public unsafe void Process(in Guid messageId, CursorPress message)
        {
            if(message.Button != CursorButtons.Right || message.Value == false)
            {
                return;
            }

            IEnumerable<int>? indices = _world.Grid.GetCellIndices(new Rectangle(_brush.X / 10, _brush.Y / 10, _size, _size));

            if(indices is null)
            {
                return;
            }

            foreach(int index in indices)
            {
                _world.Grid.Cells[index].New.Type = Enums.CellTypeEnum.Sand;
            }
        }
    }
}
