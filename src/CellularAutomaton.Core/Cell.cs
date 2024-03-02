using System.Runtime.CompilerServices;

namespace CellularAutomaton.Core
{
    public unsafe struct Cell<TData>
        where TData : unmanaged
    {
        private TData _dataA;
        private TData _dataB;

        private TData* _newPtr;
        private TData* _oldPtr;

        private bool _updated;
        private int _inactivityCount;
        private bool _asleep;

        public ref TData Data
        {
            get
            {
                _updated = true;
                return ref _newPtr[0];
            }
        }
        public ref TData DataOld => ref _oldPtr[0];
        public ref TData DataLatest => ref this.GetLatest();

        public int InactivityCount => _inactivityCount;
        public bool Updated => _updated;
        public bool Asleep
        {
            get => _asleep;
            set => _asleep = value;
        }

        public Cell()
        {
            _newPtr = (TData*)Unsafe.AsPointer(ref _dataA);
            _oldPtr = (TData*)Unsafe.AsPointer(ref _dataB);
        }

        public void Reset()
        {
            TData* old = (TData*)Unsafe.AsPointer(ref _newPtr[0]);
            _newPtr = _oldPtr;
            _newPtr[0] = default;

            _oldPtr = old;

            if (_updated == (_updated = false))
            {
                _inactivityCount++;
            }
            else
            {
                _inactivityCount = 0;
                this.Asleep = false;
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
