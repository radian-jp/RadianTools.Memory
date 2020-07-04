using System;
using System.Runtime.InteropServices;

namespace RadianTools.Memory
{
    public class UnmanagedMemoryObject<T> : AbstractMemoryObject<T> where T : struct, IEquatable<T>
    {
        private Action _DisposeAction;
        private bool _Disposed = false;

        public UnmanagedMemoryObject(int length) : base(length)
        {
            Initialize(Marshal.AllocCoTaskMem(Capacity), () => { Marshal.FreeCoTaskMem(Pointer); });
        }

        public UnmanagedMemoryObject(IntPtr pointer, int length, Action disposeAction) : base(length)
        {
            Initialize(pointer, disposeAction);
        }

        private void Initialize(IntPtr pointer, Action disposeAction)
        {
            Pointer = pointer;
            _DisposeAction = disposeAction;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                try
                {
                    _DisposeAction?.Invoke();
                }
                catch
                {
                }
                _Disposed = true;
            }
        }
    }
}
