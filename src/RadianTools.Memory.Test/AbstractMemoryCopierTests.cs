using System;
using RadianTools.Memory;
using Xunit;

namespace RadianTools.Memory.Test
{
    public class AbstractMemoryObjectTests
    {
        private class AbstractMemoryCopierImpl : AbstractMemoryCopier
        {
            public override string MethodName => $"{typeof(AbstractMemoryCopierImpl).FullName}.Copy";

            public override unsafe void Copy(void* src, void* dst, uint dstStart, uint length)
            {
                throw new NotImplementedException();
            }
        }

        const int Test_ArrayLength = 100;
        const int Test_StartIndex = 20;
        const int Test_CopyLength = 30;

        [Fact(DisplayName = "Test AbstractMemoryObject.Copy(Span<T>, Span<T>, uint, uint)")]
        public void CopySpanTest()
        {
            var copier = new AbstractMemoryCopierImpl();
            using (var memory1 = new ManagedMemoryObject<int>(Test_ArrayLength))
            using (var memory2 = new ManagedMemoryObject<int>(Test_ArrayLength))
            {
                var span1 = memory1.AsSpan();
                var span2 = memory1.AsSpan();
                var span1_20_49 = span1.Slice(Test_StartIndex, Test_CopyLength);
                for (int i = 0; i < span1.Length; i++)
                {
                    span1[i] = i + 1;
                }

                copier.Copy(span1_20_49, span2, 0, (uint)span1_20_49.Length);
                for (int i = 0; i < span1_20_49.Length; i++)
                {
                    Assert.NotEqual(memory1[i + Test_StartIndex], memory2[i]);
                }

                memory2.Clear();
            }
        }
    }
}
