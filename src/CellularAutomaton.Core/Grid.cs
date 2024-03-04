using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CellularAutomaton.Core
{
    public unsafe struct Grid<TData> : IDisposable
        where TData : unmanaged
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int Length;
        public readonly bool Wrap;

        public readonly Cell<TData>* Cells;

        public Grid(int width, int height, bool wrap, TData defaultData)
        {
            this.Width = width;
            this.Height = height;
            this.Length = this.Width * this.Height;
            this.Wrap = wrap;

            this.Cells = (Cell<TData>*)Marshal.AllocHGlobal(this.Length * sizeof(Cell<TData>));

            for (int i = 0; i < this.Length; i++)
            {
                Point position = this.CalculatePosition(i);
                this.Cells[i] = new Cell<TData>(
                    position: position,
                    index: i,
                    neighbors: this.CalculateNeighbors(position));

                this.Cells[i].Initialize();
                this.Cells[i].Old = defaultData;
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < this.Length; i++)
            {
                this.Cells[i].Dispose();
            }

            Marshal.FreeHGlobal((nint)this.Cells);
        }

        public ref Cell<TData> GetCell(int x, int y, out bool exists)
        {
            return ref this.GetCell(this.CalculateIndex(x, y), out exists);
        }

        public ref Cell<TData> GetCell(int index, out bool exists)
        {
            if (index != -1)
            {
                exists = true;
                return ref this.Cells[index];
            }

            exists = false;
            return ref Unsafe.NullRef<Cell<TData>>();
        }

        public int CalculateIndex(int x, int y)
        {
            if (x >= 0 && x < this.Width && y >= 0 && y < this.Height)
            {
                return x + (y * this.Width);
            }

            if (this.Wrap)
            {
                x = x < 0 ? this.Width + x : x;
                y = y < 0 ? this.Height + y : y;

                return x + (y * this.Width);
            }

            return -1;
        }

        public Point CalculatePosition(int index)
        {
            if (index > this.Length)
            {
                return Point.Zero;
            }

            if (index < 0)
            {
                return Point.Zero;
            }

            return new Point(index % this.Width, index / this.Width);
        }

        public IEnumerable<int> GetCellIndices(Rectangle bounds)
        {
            for(int x = bounds.Left; x < bounds.Right; x++)
            {
                for (int y = bounds.Top; y < bounds.Bottom; y++)
                {
                    int index = this.CalculateIndex(x, y);

                    if (index != -1)
                    {
                        yield return index;
                    }
                }
            }
        }

        private Neighbors<TData> CalculateNeighbors(Point position)
        {
            Span<IntPtr> ptrs = stackalloc nint[8];
            int ptrCount = 0;

            void AddIfExists(ref int index, ref Span<IntPtr> ptrs, int cell, Cell<TData>* cells)
            {
                if (cell == -1)
                {
                    return;
                }

                ptrs[ptrCount++] = (IntPtr)Unsafe.AsPointer(ref cells[cell]);
            }

            AddIfExists(ref ptrCount, ref ptrs, this.CalculateIndex(position.X - 1, position.Y - 1), this.Cells);
            AddIfExists(ref ptrCount, ref ptrs, this.CalculateIndex(position.X + 0, position.Y - 1), this.Cells);
            AddIfExists(ref ptrCount, ref ptrs, this.CalculateIndex(position.X + 1, position.Y - 1), this.Cells);

            AddIfExists(ref ptrCount, ref ptrs, this.CalculateIndex(position.X - 1, position.Y + 0), this.Cells);
            AddIfExists(ref ptrCount, ref ptrs, this.CalculateIndex(position.X + 1, position.Y + 0), this.Cells);

            AddIfExists(ref ptrCount, ref ptrs, this.CalculateIndex(position.X - 1, position.Y + 1), this.Cells);
            AddIfExists(ref ptrCount, ref ptrs, this.CalculateIndex(position.X + 0, position.Y + 1), this.Cells);
            AddIfExists(ref ptrCount, ref ptrs, this.CalculateIndex(position.X + 1, position.Y + 1), this.Cells);

            return new Neighbors<TData>(ptrCount, ptrs);
        }
    }
}
