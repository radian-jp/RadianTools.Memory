using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RadianTools.Memory
{
    public abstract class AbstractMemoryCopier : IMemoryCopier
    {
        public abstract unsafe void Copy(void* source, void* destination, uint destinationStart, uint length);

        public abstract string MethodName { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Copy<T>(T[] source, T[] destination, uint destinationStart, uint length)
        {
            Copy(source.AsSpan(), destination.AsSpan(), destinationStart, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Copy<T>(IMemoryObject<T> source, IMemoryObject<T> destination, uint destinationStart, uint length) where T : struct, IEquatable<T>
        {
            if (destination == null)
            {
                throw new ArgumentNullException("source is null");
            }
            if (destination == null)
            {
                throw new ArgumentNullException("destination is null");
            }
            if (destination.Length - destinationStart < length)
            {
                throw new IndexOutOfRangeException("The length exceeds the copy destination range");
            }
            if (source.Length - destinationStart < length)
            {
                throw new IndexOutOfRangeException("The length exceeds the copy source range");
            }

            var itemSize = (uint)source.ItemSize;
            Copy(source.Pointer, destination.Pointer, destinationStart * itemSize, length * itemSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Copy<T>(Span<T> source, Span<T> destination, uint destinationStart, uint length)
        {
            var srcSli = source.Slice(0, (int)length);
            var dstSli = destination.Slice((int)destinationStart, (int)length);
            srcSli.CopyTo(dstSli);
        }

        public unsafe void Copy(IntPtr source, IntPtr destination, uint destinationStart, uint length)
        {
            Copy((void*)source, (void*)destination, destinationStart, length);
        }
    }
}
