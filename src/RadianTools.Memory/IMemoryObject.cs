using System;
using System.Collections.Generic;

namespace RadianTools.Memory
{
    public interface IMemoryObject<T> : IList<T>, IDisposable where T : struct, IEquatable<T>
    {
        int Capacity { get; }
        int Length { get; }
        int ItemSize { get; }
        IntPtr Pointer { get; }
        Span<T> AsSpan();
        Span<TDst> AsSpan<TDst>();
        ReadOnlySpan<T> AsReadOnlySpan();
        ReadOnlySpan<TDst> AsReadOnlySpan<TDst>();
        System.IO.Stream CreateStream();
    }
}
