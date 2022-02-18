using NeoPolaris.Memory;
using System;
using System.Runtime.InteropServices;

namespace NeoPolaris.Abilities.Structs
{
    internal class FGameplayEffectContextHandle : MemoryObject
    {
        public FGameplayEffectContextHandle()
        {
            BaseAddress = Marshal.AllocHGlobal(ObjectSize);
        }

        public FGameplayEffectContextHandle(IntPtr baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public override int ObjectSize => 0x18;
    }
}
