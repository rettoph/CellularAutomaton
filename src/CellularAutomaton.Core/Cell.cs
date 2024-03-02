using System.Drawing;
using System.Runtime.CompilerServices;

namespace CellularAutomaton.Core
{
    public unsafe struct Cell<TData> : IDisposable
        where TData : unmanaged
    {
        private bool _updated;
        private int _inactivityCount;
        private bool _asleep;

        private TData _dataA;
        private TData _dataB;

        private TData* _newPtr;
        private TData* _oldPtr;

        public ref TData New
        {
            get
            {
                _updated = true;
                return ref _newPtr[0];
            }
        }
        public ref TData Old => ref _oldPtr[0];
        public ref TData Latest => ref this.GetLatest();

        public int InactivityCount => _inactivityCount;
        public bool Updated => _updated;
        public bool Asleep
        {
            get => _asleep;
            set => _asleep = value;
        }

        public readonly Point Position;
        public readonly int Index;
        public readonly Neighbors<TData> Neighbors;

        public Cell(Point position, int index, Neighbors<TData> neighbors)
        {
            _newPtr = (TData*)Unsafe.AsPointer(ref _dataA);
            _oldPtr = (TData*)Unsafe.AsPointer(ref _dataB);

            this.Position = position;
            this.Index = index;
        }

        public void Dispose()
        {
            this.Neighbors.Dispose();
        }

        public void Reset()
        {
            if (_updated == (_updated = false))
            {
                _inactivityCount++;
            }
            else
            {
                _inactivityCount = 0;
                this.Asleep = false;

                TData* old = (TData*)Unsafe.AsPointer(ref _newPtr[0]);
                _newPtr = _oldPtr;
                _newPtr[0] = default;
                _oldPtr = old;
            }
        }

        private ref TData GetLatest()
        {
            if (_updated)
            {
                return ref _newPtr[0];
            }

            return ref _oldPtr[0];
        }
    }
}
