using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;

namespace RadianTools.Memory
{
    public abstract class AbstractMemoryObject<T> : IMemoryObject<T> where T : struct, IEquatable<T>
    {
        public virtual unsafe T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= Length) throw new IndexOutOfRangeException();

                return Unsafe.Add(ref Unsafe.AsRef<T>((void*)Pointer), index);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if ((uint)index >= Length) throw new IndexOutOfRangeException();

                Unsafe.Add(ref Unsafe.AsRef<T>((void*)Pointer), index) = value;
            }
        }

        public IntPtr Pointer { get; protected set; }

        public bool IsReadOnly { get; protected set; }

        public int Capacity { get; protected set; }

        public int Count { get => Length; protected set => Length = value; }

        public int Length { get; protected set; }

        public int ItemSize => Unsafe.SizeOf<T>();

        protected AbstractMemoryObject(int length)
        {
            if (length <= 0) throw new ArgumentException("Invalid length");

            Capacity = length * ItemSize;
            Length = length;
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        ~AbstractMemoryObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public unsafe Span<T> AsSpan()
        {
            return new Span<T>((void*)Pointer, Length);
        }

        public unsafe Span<TDst> AsSpan<TDst>()
        {
            var itemSize = Unsafe.SizeOf<TDst>();
            return new Span<TDst>((void*)Pointer, Capacity / itemSize);
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return AsSpan();
        }

        public ReadOnlySpan<TDst> AsReadOnlySpan<TDst>()
        {
            return AsSpan<TDst>();
        }

        public int IndexOf(T item)
        {
            return MemoryExtensions.IndexOf(AsSpan(), item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            AsSpan().Clear();
        }

        public bool Contains(T obj)
        {
            var span = AsSpan();
            foreach (var objThis in span)
            {
                if (objThis.Equals(obj))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var spanDst = array.AsSpan().Slice(arrayIndex);
            AsSpan().CopyTo(spanDst);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
        }

        public static implicit operator Span<T>(AbstractMemoryObject<T> memory)
        {
            return memory.AsSpan();
        }

        public virtual Stream CreateStream()
        {
            return new MemoryObjectStream<T>(this);
        }
    }
}
