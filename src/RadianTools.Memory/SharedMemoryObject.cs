using System;
using System.IO.MemoryMappedFiles;

namespace RadianTools.Memory
{
    public enum SharedMemoryOpenMethod : int
    {
        CreateOrOpen,
        CreateNew,
        OpenExisting
    }

    public class SharedMemoryObject<T> : AbstractMemoryObject<T>, IDisposable where T : struct, IEquatable<T>
    {
        private bool _Disposed = false;
        private MemoryMappedFile _MMF;
        private MemoryMappedViewAccessor _Accessor;

        public SharedMemoryObject(string mapName, int length) : base(length)
        {
            Initialize(mapName, length, SharedMemoryOpenMethod.CreateOrOpen);
        }

        public SharedMemoryObject(string mapName, int length, SharedMemoryOpenMethod openType) : base(length)
        {
            Initialize(mapName, length, openType);
        }

        private void Initialize(string mapName, int length, SharedMemoryOpenMethod openType)
        {
            switch (openType)
            {
                case SharedMemoryOpenMethod.CreateOrOpen:
                    _MMF = MemoryMappedFile.CreateOrOpen(mapName, Capacity);
                    break;
                case SharedMemoryOpenMethod.CreateNew:
                    _MMF = MemoryMappedFile.CreateNew(mapName, Capacity);
                    break;
                default:
                    _MMF = MemoryMappedFile.OpenExisting(mapName);
                    break;
            }
            _Accessor = _MMF.CreateViewAccessor();
            unsafe
            {
                byte* pByte = null;
                _Accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pByte);
                Pointer = (IntPtr)pByte;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                try
                {
                    _Accessor.SafeMemoryMappedViewHandle.ReleasePointer();
                    _Accessor.Dispose();
                    _MMF.Dispose();
                }
                catch
                {
                }
                _Disposed = true;
            }
        }
    }
}
