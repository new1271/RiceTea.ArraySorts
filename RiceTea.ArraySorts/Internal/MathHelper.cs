using InlineMethod;

using System.Runtime.CompilerServices;

#if NET5_0_OR_GREATER
using System.Numerics;
#endif

namespace RiceTea.ArraySorts.Internal
{
    internal static class MathHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2(uint value)
        {
#if NET5_0_OR_GREATER
            return BitOperations.Log2(value);
#else
            if (value < 2U)
                return 0;
            return 32 - LeadingZeroCount(value);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2(ulong value)
        {
#if NET5_0_OR_GREATER
            return BitOperations.Log2(value);
#else
            if (value < 2U)
                return 0;
            int head = Log2(unchecked((uint)(value >> 32)));
            return head > 0 ? head : Log2(unchecked((uint)value));
#endif
        }

#if !NET5_0_OR_GREATER
        private const int numIntBits = sizeof(int) * 8;

        [Inline(InlineBehavior.Remove)]
        private static int LeadingZeroCount(uint value)
        {
            unchecked
            {
                value |= value >> 1;
                value |= value >> 2;
                value |= value >> 4;
                value |= value >> 8;
                value |= value >> 16;

                value -= value >> 1 & 0x55555555;
                value = (value >> 2 & 0x33333333) + (value & 0x33333333);
                value = (value >> 4) + value & 0x0f0f0f0f;
                value += value >> 8;
                value += value >> 16;
                return (int)(numIntBits - (value & 0x0000003f));
            }
        }
#endif
    }
}
