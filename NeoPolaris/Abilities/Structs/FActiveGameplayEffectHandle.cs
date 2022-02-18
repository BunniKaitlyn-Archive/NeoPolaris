using NeoPolaris.Memory;
using NeoPolaris.Utilities;
using System;
using System.Runtime.InteropServices;

namespace NeoPolaris.Abilities.Structs
{
    internal class FActiveGameplayEffectHandle : MemoryObject
    {
        public int Handle
        {
            get => ReadInt32(0);
            set => WriteInt32(0, value);
        }

        public bool bPassedFiltersAndWasExecuted
        {
            get => Memory.ReadUInt8(BaseAddress, 4) != 0;
            set => Memory.WriteUInt8(BaseAddress, 4, value ? (byte) 1 : (byte) 0);
        }

        public FActiveGameplayEffectHandle()
        {
            BaseAddress = Marshal.AllocHGlobal(ObjectSize);
            Win32.RtlFillMemory(BaseAddress, (uint)ObjectSize, 0);
        }

        public FActiveGameplayEffectHandle(IntPtr baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public override int ObjectSize => 8;
    }
}
