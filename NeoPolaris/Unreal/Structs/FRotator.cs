using NeoPolaris.Memory;
using System.Runtime.InteropServices;

namespace NeoPolaris.Unreal.Structs
{
    internal class FRotator : MemoryObject
    {
        public float Pitch
        {
            get => Memory.ReadSingle(BaseAddress, 0);
            set => Memory.WriteSingle(BaseAddress, 0, value);
        }

        public float Yaw
        {
            get => Memory.ReadSingle(BaseAddress, 4);
            set => Memory.WriteSingle(BaseAddress, 4, value);
        }

        public float Roll
        {
            get => Memory.ReadSingle(BaseAddress, 8);
            set => Memory.WriteSingle(BaseAddress, 8, value);
        }

        public FRotator()
        {
            BaseAddress = Marshal.AllocHGlobal(ObjectSize);
        }

        public override int ObjectSize => 0xC;
    }
}
