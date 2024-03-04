using CellularAutomaton.Core.Graphics.Vertices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CellularAutomaton.Core.Utilities
{
    public unsafe class VertexCellBuffer<TData> : IDisposable
        where TData : unmanaged
    {
        private readonly GraphicsDevice _graphics;
        private readonly VertexBuffer _buffer;
        public readonly VertexCell[] _vertices;

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

        public unsafe void Update(ref Cell<TData> cell)
        {
            _vertices[cell.Index].Color = cell.Color.PackedValue;
            _vertices[cell.Index].Asleep = cell.Asleep;

            // _buffer.SetData(cell.Index * sizeof(VertexCell), _vertices, cell.Index, 1, sizeof(VertexCell));
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }
    }
}
