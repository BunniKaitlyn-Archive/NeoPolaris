using NeoPolaris.Memory;
using System.Runtime.InteropServices;

namespace NeoPolaris.Unreal.Structs
{
    internal class FVector : MemoryObject
    {
        public float X
        {
            get => Memory.ReadSingle(BaseAddress, 0);
            set => Memory.WriteSingle(BaseAddress, 0, value);
        }

        public float Y
        {
            get => Memory.ReadSingle(BaseAddress, 4);
            set => Memory.WriteSingle(BaseAddress, 4, value);
        }

        public float Z
        {
            get => Memory.ReadSingle(BaseAddress, 8);
            set => Memory.WriteSingle(BaseAddress, 8, value);
        }

        public FVector()
        {
            BaseAddress = Marshal.AllocHGlobal(ObjectSize);
        }

        public override int ObjectSize => 0xC;
    }
}
