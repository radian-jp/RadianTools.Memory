using System;
using System.Runtime.InteropServices;

namespace RadianTools.Memory
{
    internal class WinApiMemoryCopier : AbstractMemoryCopier
    {
        public override string MethodName => $"{typeof(WinApiMemoryCopier).FullName}.RtlMoveMemory";

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.Winapi)]
        private static extern void RtlMoveMemory(IntPtr destination, IntPtr source, IntPtr size);

        public override unsafe void Copy(void* source, void* destination, uint destinationStart, uint length)
        {
            RtlMoveMemory((IntPtr)((byte*)destination + destinationStart), (IntPtr)source, new IntPtr(length));
        }
    }
}
