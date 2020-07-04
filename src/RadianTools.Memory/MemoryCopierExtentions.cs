using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RadianTools.Memory
{
    public enum MemoryCopierType
    {
        Buffer,
        Unsafe,
        WinApi
    }

    public static class MemoryCopierExtentions
    {
        private static Dictionary<MemoryCopierType, IMemoryCopier> _DictCopier = new Dictionary<MemoryCopierType, IMemoryCopier>();

        public static MemoryCopierType CopierType
        {
            get => _CopierType;
            set
            {
                Copier = CreateCopier(value);
                _CopierType = value;
            }
        }
        private static MemoryCopierType _CopierType = MemoryCopierType.Buffer;

        public static IMemoryCopier Copier { get; private set; } = CreateCopier(_CopierType);

        public static IMemoryCopier CreateCopier(MemoryCopierType cpType)
        {
            if (_DictCopier.ContainsKey(cpType)) return _DictCopier[cpType];

            var type = Type.GetType(typeof(AbstractMemoryCopier).FullName.Replace("Abstract", cpType.ToString()));
            var copier = (IMemoryCopier)Activator.CreateInstance(type);
            _DictCopier[cpType] = copier;
            return copier;
        }

        public static void Copy<T>(this T[] source, T[] destination, uint destinationStart, uint length)
        {
            Copier.Copy(source, destination, destinationStart, length);
        }

        public static void Copy<T>(this Span<T> source, Span<T> destination, uint destinationStart, uint length)
        {
            Copier.Copy(source, destination, destinationStart, length);
        }

        public static void Copy<T>(this IMemoryObject<T> source, IMemoryObject<T> destination, uint destinationStart, uint length) where T : struct, IEquatable<T>
        {
            Copier.Copy(source, destination, destinationStart, length);
        }

        public static void Copy(this IntPtr source, IntPtr destination, uint destinationStart, uint length)
        {
            Copier.Copy(source, destination, destinationStart, length);
        }
    }
}
