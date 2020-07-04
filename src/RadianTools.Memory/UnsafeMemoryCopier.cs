using System.Runtime.CompilerServices;

namespace RadianTools.Memory
{
    internal class UnsafeMemoryCopier : AbstractMemoryCopier
    {
        public override string MethodName => $"{typeof(Unsafe).FullName}.CopyBlockUnaligned";

        public override unsafe void Copy(void* source, void* destination, uint destinationStart, uint length)
        {
            Unsafe.CopyBlockUnaligned((byte*)destination + destinationStart, source, length);
        }
    }
}
