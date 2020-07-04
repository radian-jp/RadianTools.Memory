using System;
using RadianTools.Memory;
using System.Diagnostics;
using System.Threading;

namespace MemoryTest
{
    class Program
    {
        private const int Length = 40000000;
        private const int TryCount = 100;

        static unsafe void Main(string[] args)
        {
            Console.WriteLine($"Process start (EnvironmentVersion:{Environment.Version.ToString()} ProcessName:{Process.GetCurrentProcess().ProcessName} Is64BitProcess:{Environment.Is64BitProcess})");

            using (var memory1 = new UnmanagedMemoryObject<int>(Length))
            using (var memory2 = new UnmanagedMemoryObject<int>(Length))
            using (var stream1 = memory1.CreateStream())
            using (var stream2 = memory2.CreateStream())
            {
                var copierTypes = new MemoryCopierType[]
                {
                    MemoryCopierType.Buffer,
                    MemoryCopierType.Unsafe,
                    MemoryCopierType.WinApi
                };
                var watch = new Stopwatch();
                var span1 = memory1.AsSpan();
                var span2 = memory2.AsSpan();
                var bspan1 = memory1.AsSpan<byte>();
                for (int i = 0; i < bspan1.Length; i++)
                {
                    bspan1[i] = (byte)(i & 0xff);
                }

                Console.WriteLine($"=== Copy int value (Length:{Length}) MemoryType:{memory1.GetType().Name}) ===");

                Thread.Sleep(3000);

                memory2.Clear();
                watch.Reset();
                watch.Start();
                for (int count = 0; count < TryCount; count++)
                {
                    span1.Copy(span2, 0, (uint)span1.Length);
                }
                watch.Stop();
                Console.WriteLine($"Copy Span:{(decimal)watch.ElapsedMilliseconds / (decimal)TryCount} msec");

                memory2.Clear();
                watch.Reset();
                watch.Start();
                for (int count = 0; count < TryCount; count++)
                {
                    for (int i = 0; i < Length; i++)
                    {
                        memory2[i] = memory1[i];
                    }
                }
                watch.Stop();
                Console.WriteLine($"Copy Indexer:{(decimal)watch.ElapsedMilliseconds / (decimal)TryCount} msec");

                memory2.Clear();
                watch.Reset();
                watch.Start();
                byte[] buffer = new byte[memory1.Capacity];
                for (int count = 0; count < TryCount; count++)
                {
                    stream1.Position = 0;
                    stream2.Position = 0;
                    stream1.Read(buffer, 0, memory1.Capacity);
                    stream2.Write(buffer, 0, memory1.Capacity);
                }
                watch.Stop();
                Console.WriteLine($"Copy Stream:{(decimal)watch.ElapsedMilliseconds / (decimal)TryCount} msec");

                foreach (var copierType in copierTypes)
                {
                    MemoryCopierExtentions.CopierType = copierType;
                    memory2.Clear();
                    watch.Reset();
                    watch.Start();
                    for (int count = 0; count < TryCount; count++)
                    {
                        memory1.Copy(memory2, 0, (uint)memory1.Length);
                    }
                    watch.Stop();
                    Console.WriteLine($"Copy MemoryObject:{MemoryCopierExtentions.Copier.MethodName}:{(decimal)watch.ElapsedMilliseconds / (decimal)TryCount} msec");
                }
            }

            Console.WriteLine($"\nPress Enter to exit.");
            Console.ReadLine();
        }
    }
}
