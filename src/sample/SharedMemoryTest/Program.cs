using System;
using System.Diagnostics;
using RadianTools.Memory;

namespace SharedMemoryTest
{
    class Program
    {
        private const int Length = 5;

        static void Main(string[] args)
        {
            Console.WriteLine($"Process start (EnvironmentVersion:{Environment.Version.ToString()} ProcessName:{Process.GetCurrentProcess().ProcessName} Is64BitProcess:{Environment.Is64BitProcess})");
            Console.WriteLine("Press Enter:Add shared memory value\nPress Q:Exit\n");
            using (var shareMem = new SharedMemoryObject<double>("Memory1", Length))
            {
                var span = shareMem.AsSpan();

                while (true)
                {
                    var keyInfo = Console.ReadKey(true);
                    var rand = new Random();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Enter:
                            for (int i = 0; i < Length; i++)
                            {
                                var addVal = rand.NextDouble();
                                span[i] += addVal;
                                Console.WriteLine($"span[{i}]={span[i]} (+{addVal})");
                            }
                            break;

                        case ConsoleKey.Q:
                            return;
                    }
                }
            }
        }
    }
}
