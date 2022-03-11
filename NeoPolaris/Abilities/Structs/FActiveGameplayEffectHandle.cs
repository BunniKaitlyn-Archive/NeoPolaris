using Reality.ModLoader.Memory;

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

        public override int ObjectSize => 8;
    }
}
