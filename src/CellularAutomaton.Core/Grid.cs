﻿using CellularAutomaton.Core.Services;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CellularAutomaton.Core
{
    public unsafe struct Grid<TData> : IDisposable
        where TData : unmanaged
    {
        private readonly int* _updateIndices;

        public readonly int Width;
        public readonly int Height;
        public readonly int Length;
        public readonly bool Wrap;

        public readonly Cell<TData>* Cells;

        public Grid(int width, int height, bool wrap)
        {
            this.Width = width;
            this.Height = height;
            this.Length = this.Width * this.Height;
            this.Wrap = wrap;

            this.Cells = (Cell<TData>*)Marshal.AllocHGlobal(this.Length * sizeof(Cell<TData>));
            _updateIndices = CalculateUpdateIndices(this.Length);

            for (int i = 0; i < this.Length; i++)
            {
                Point position = this.CalculatePosition(i);
                this.Cells[0] = new Cell<TData>(
                    position: position,
                    index: i,
                    neighbors: this.CalculateNeighbors(position));
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < this.Length; i++)
            {
                this.Cells[i].Dispose();
            }

            Marshal.FreeHGlobal((nint)this.Cells);
            Marshal.FreeHGlobal((nint)_updateIndices);
        }

        public void Update(ICellUpdateService<TData> cellUpdateService)
        {
            for (int i = 0; i < this.Length; i++)
            {
                this.Update(ref this.Cells[_updateIndices[i]], cellUpdateService);
            }
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
                return Point.Empty;
            }

            if (index < 0)
            {
                return Point.Empty;
            }

            return new Point(index % this.Width, index / this.Width);
        }

        private void Update(ref Cell<TData> cell, ICellUpdateService<TData> cellUpdateService)
        {
            cellUpdateService.UpdateCell(ref cell, ref this);
            cell.Reset();
        }

        private static int* CalculateUpdateIndices(int length)
        {
            int* indices = (int*)Marshal.AllocHGlobal(length * sizeof(int));

            int evenLength = (length + (length % 2)) / 2;
            for (int i = 0; i < evenLength; i++)
            {
                indices[i * 2] = i * 2;
            }

            int oddLength = length / 2;
            for (int i = 0; i < oddLength; i++)
            {
                indices[(i * 2) + 1] = (length - (length % 2)) - ((i * 2) + 1);
            }

            return indices;
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

                ptrs[ptrCount++] = (IntPtr)(Cell<TData>*)Unsafe.AsPointer(ref cells[cell]);
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