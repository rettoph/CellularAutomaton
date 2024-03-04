using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace CellularAutomaton.Core
{
    public unsafe struct Cell<TData> : IDisposable
        where TData : unmanaged
    {
        private bool _updated;
        private int _idleCount;
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

        public int IdleCount => _idleCount;
        public bool Idle => _idleCount > 0;
        public bool Updated => _updated;

        public Color Color;

        /// <summary>
        /// It is the responsibility of the <see cref="Services.ICellService{TData}"/> to
        /// update a cell state as needed
        /// </summary>
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
            this.Position = position;
            this.Index = index;
            this.Neighbors = neighbors;
        }

        public void Dispose()
        {
            this.Neighbors.Dispose();
        }

        internal void Initialize()
        {
            _newPtr = (TData*)Unsafe.AsPointer(ref _dataA);
            _oldPtr = (TData*)Unsafe.AsPointer(ref _dataB);
        }

        public void Reset()
        {
            if (_updated == false)
            {
                _idleCount++;
            }
            else
            {
                _updated = false;
                _idleCount = 0;
                _asleep = false;

                TData* old = _newPtr;
                _newPtr = _oldPtr;
                _oldPtr = old;

                _newPtr[0] = default;
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
