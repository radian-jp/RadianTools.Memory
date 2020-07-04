using System;

namespace RadianTools.Memory
{
    public interface IMemoryCopier
    {
        string MethodName { get; }
        unsafe void Copy(void* source, void* destination, uint destinationStart, uint length);
        void Copy(IntPtr source, IntPtr destination, uint destinationStart, uint length);
        void Copy<T>(T[] source, T[] destination, uint destinationStart, uint length);
        void Copy<T>(Span<T> source, Span<T> destination, uint destinationStart, uint length);
        void Copy<T>(IMemoryObject<T> source, IMemoryObject<T> destination, uint destinationStart, uint length) where T : struct, IEquatable<T>;
    }
}
