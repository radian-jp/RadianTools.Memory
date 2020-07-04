using System;

namespace RadianTools.Memory
{
    internal class BufferMemoryCopier : AbstractMemoryCopier
    {
        public override string MethodName => $"{typeof(Buffer).FullName}.MemoryCopy";

        public override unsafe void Copy(void* source, void* destination, uint destinationStart, uint length)
        {
            Buffer.MemoryCopy(source, (byte*)destination + destinationStart, length, length);
        }
    }
}
