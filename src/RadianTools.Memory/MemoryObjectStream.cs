using System;
using System.Runtime.CompilerServices;
using System.IO;

namespace RadianTools.Memory
{
    internal class MemoryObjectStream<T> : Stream where T : struct, IEquatable<T>
    {
        private IMemoryObject<T> _Memory;

        public MemoryObjectStream(IMemoryObject<T> memory)
        {
            _Memory = memory;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => _Memory.Length;

        public override long Position { get; set; }

        public override void Flush()
        {
        }

        public unsafe override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer is null");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid offset");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid count");
            }
            if (buffer.Length - offset < count)
            {
                throw new ArgumentException("buffer too small for count");
            }
            if (_Memory.Capacity - Position < count)
            {
                throw new IOException("count greater than the end of memory");
            }

            var bufPtr = Unsafe.AsPointer(ref buffer[offset]);
            Buffer.MemoryCopy((byte*)_Memory.Pointer + Position, bufPtr, buffer.Length - offset, count);
            Position += count;
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;

                case SeekOrigin.Current:
                    Position += offset;
                    break;

                case SeekOrigin.End:
                    Position = Length + offset;
                    break;
            }

            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public unsafe override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer is null");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid offset");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid count");
            }
            if (buffer.Length - offset < count)
            {
                throw new ArgumentException("buffer too small for count");
            }
            if (_Memory.Capacity - Position < count)
            {
                throw new IOException("count greater than the end of memory");
            }

            var bufPtr = Unsafe.AsPointer(ref buffer[offset]);
            Buffer.MemoryCopy(bufPtr, (byte*)_Memory.Pointer + Position, _Memory.Capacity - Position, count);
            Position += count;
        }
    }
}
