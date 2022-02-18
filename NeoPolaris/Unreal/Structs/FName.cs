using NeoPolaris.Memory;
using System;
using System.Runtime.InteropServices;

namespace NeoPolaris.Unreal.Structs
{
    /// <summary>
    /// Offsets should *usually* never change here.
    /// </summary>
    public class FName : MemoryObject
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ToStringDelegate(IntPtr thisPtr, IntPtr value);
        public static ToStringDelegate ToString_;

        public int ComparisonIndex => ReadInt32(0);
        public int Number => ReadInt32(4);

        static FName()
        {
            ToString_ = MemoryUtil.GetNativeFuncFromPatternWithOffset<ToStringDelegate>("\xE8\x00\x00\x00\x00\x39\x75\xD0", "x????xxx");
        }

        public override string ToString()
        {
            var value = new FString();
            ToString_(BaseAddress, value.BaseAddress);
            return value.Value;
        }

        public override int ObjectSize => 8;
    }
}
