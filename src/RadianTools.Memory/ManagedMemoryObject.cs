using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RadianTools.Memory
{
    public class ManagedMemoryObject<T> : AbstractMemoryObject<T> where T : struct, IEquatable<T>
    {
        private bool _Disposed = false;
        private T[] _Array;
        private GCHandle _GCH;

        public override T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _Array[index];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _Array[index] = value;
        }

        public ManagedMemoryObject(int length) : base(length)
        {
            Initialize(new T[length]);
        }

        public ManagedMemoryObject(T[] array) : base(array.Length)
        {
            Initialize(array);
        }

        private void Initialize(T[] array)
        {
            _Array = array;
            _GCH = GCHandle.Alloc(_Array, GCHandleType.Pinned);
            Pointer = _GCH.AddrOfPinnedObject();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                try
                {
                    _GCH.Free();
                }
                catch
                {
                }
                _Disposed = true;
            }
        }
    }
}
