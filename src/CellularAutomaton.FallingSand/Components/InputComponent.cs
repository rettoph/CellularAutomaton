using CellularAutomaton.Core;
using CellularAutomaton.FallingSand.Enums;
using CellularAutomaton.FallingSand.Messages;
using CellularAutomaton.FallingSand.Services;
using Guppy;
using Guppy.Attributes;
using Guppy.Common.Attributes;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Input;
using Guppy.Game.Input.Enums;
using Guppy.Game.Input.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CellularAutomaton.FallingSand.Components
{
    [AutoLoad]
    [Sequence<DrawSequence>(DrawSequence.PostDraw)]
    [Sequence<UpdateSequence>(UpdateSequence.PreUpdate)]
    internal class InputComponent : GuppyComponent, IGuppyDrawable, IGuppyUpdateable,
        IInputSubscriber<CursorScroll>,
        IInputSubscriber<CursorMove>,
        IInputSubscriber<CursorPress>,
        IInputSubscriber<RenderAsleepInput>
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _pixel;
        private readonly CellService _cells;
        private readonly World<CellData> _world;
        private readonly CellTypeEnum[] _validInputs;

        private Rectangle _brush;
        private int _size = 10;
        private int _density;
        private int _inputIndex;
        private bool _brushActive;
        private bool _renderAsleep;

        public InputComponent(SpriteBatch spriteBatch, GraphicsDevice graphics, CellService cells, World<CellData> world)
        {
            _cells = cells;
            _spriteBatch = spriteBatch;
            _world = world;
            _density = 2;
            _validInputs = cells.GetAll().Where(x => x.ValidInput).Select(x => x.Type).ToArray();

            _pixel = new Texture2D(graphics, 1, 1);
            _pixel.SetData(new Color[] { Color.White });

            _size = 10;
            _brush.Size = new Point(_size * 2, _size * 2);
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(_pixel, _brush, _cells[_validInputs[_inputIndex]].Color);

            _spriteBatch.End();
        }

        public unsafe void Update(GameTime gameTime)
        {
            if (_brushActive == false)
            {
                return;
            }

            IEnumerable<int>? indices = _world.Grid.GetCellIndices(new Rectangle((_brush.X / 2) - 1, (_brush.Y / 2) - 1, _size + 2, _size + 2));

            if (indices is null)
            {
                return;
            }

            int minX = (_brush.X / 2);
            int minY = (_brush.Y / 2);

            int maxX = (minX + _size);
            int maxY = (minY + _size);

            foreach (int index in indices)
            {
                ref Cell<CellData> cell = ref _world.Grid.Cells[index];

                if (cell.Position.X > minX && cell.Position.X < maxX && cell.Position.Y > minY && cell.Position.Y < maxY)
                {
                    cell.Latest.Type = _validInputs[_inputIndex];
                }

                cell.Asleep = false;
            }
        }

        public void Process(in Guid messageId, CursorScroll message)
        {
            _size += message.Delta > 0 ? 1 : -1;
            _brush.Size = new Point(_size * 2, _size * 2);

            _inputIndex = (_inputIndex + 1) % _validInputs.Length;
        }

        public void Process(in Guid messageId, CursorMove message)
        {
            _brush.Location = message.Cursor.Position.ToPoint() - new Point(_size, _size);
        }

        public unsafe void Process(in Guid messageId, CursorPress message)
        {
            if (message.Button != CursorButtons.Right)
            {
                return;
            }

            _brushActive = message.Value;
        }

        public void Process(in Guid messageId, RenderAsleepInput message)
        {
            _world.RenderAsleep = message.Value;
        }
    }
}
