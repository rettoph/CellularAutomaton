using CellularAutomaton.Core.Graphics.Vertices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CellularAutomaton.Core.Utilities
{
    public class VertexCellBuffer : IDisposable
    {
        private readonly GraphicsDevice _graphics;
        private readonly VertexBuffer _buffer;
        private readonly VertexCell[] _vertices;

        public readonly int Length;

        public VertexCellBuffer(int length, GraphicsDevice graphics, Color defaultColor)
        {
            _graphics = graphics;
            _vertices = new VertexCell[length];
            _buffer = new DynamicVertexBuffer(_graphics, typeof(VertexCell), length, BufferUsage.WriteOnly);

            this.Length = length;

            for (int i = 0; i < length; i++)
            {
                _vertices[i] = new VertexCell()
                {
                    Index = i,
                    Color = defaultColor.PackedValue
                };
            }
        }

        public void Flush()
        {
            _graphics.SetVertexBuffer(_buffer);

            _buffer.SetData(_vertices);
        }

        public void Set(int index, Color color)
        {
            _vertices[index].Color = color.PackedValue;
        }

        public void Swap(int indexA, int indexB)
        {
            uint placeholder = _vertices[indexA].Color;

            _vertices[indexA].Color = _vertices[indexB].Color;
            _vertices[indexB].Color = placeholder;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
