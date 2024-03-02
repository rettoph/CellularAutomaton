using System.Runtime.InteropServices;

namespace CellularAutomaton.Core
{
    public unsafe struct Neighbors<TData> : IDisposable
        where TData : unmanaged
    {
        private Cell<TData>** _neighbors;

        public ref Cell<TData> this[int index] => ref _neighbors[index][0];
        public readonly int Length;

        public Neighbors(int length, Span<IntPtr> ptrs)
        {
            _neighbors = (Cell<TData>**)Marshal.AllocHGlobal(length * sizeof(Cell<TData>*));
            this.Length = length;

            for (int i = 0; i < this.Length; i++)
            {
                _neighbors[i] = (Cell<TData>*)ptrs[i];
            }
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal((nint)_neighbors);
        }
    }
}
