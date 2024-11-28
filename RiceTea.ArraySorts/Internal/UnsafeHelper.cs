using InlineIL;

using InlineMethod;

using System.Runtime.CompilerServices;
using System.Security;

namespace RiceTea.ArraySorts.Internal
{
    internal sealed unsafe class UnsafeHelper
    {
        [Inline(InlineBehavior.Remove)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(void* source)
        {
#if NET5_0_OR_GREATER
            return Unsafe.Read<T>(source);
#else
            IL.Push(source);
            IL.Emit.Ldobj(typeof(T));
            return IL.Return<T>();
#endif
        }

        [Inline(InlineBehavior.Remove)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(void* destination, void* source, uint byteCount)
        {
#if NET5_0_OR_GREATER
            Unsafe.CopyBlock(destination, source, byteCount);
#else
            IL.Push(destination);
            IL.Push(source);
            IL.Push(byteCount);
            IL.Emit.Cpblk();
#endif
        }

        [Inline(InlineBehavior.Remove)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGreaterThan<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Cgt();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Remove)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLessThan<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt();
            return IL.Return<bool>();
        }
    }
}
