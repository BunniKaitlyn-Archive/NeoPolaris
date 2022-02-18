using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoPolaris.Unreal.Classes
{
    internal class UByteProperty : UProperty
    {
    }

    internal class UIntProperty : UProperty
    {
    }

    internal class UFloatProperty : UProperty
    {
    }

    internal class UBoolProperty : UProperty
    {
        public byte FieldSize => Memory.ReadUInt8(BaseAddress, 0x70);
        public byte ByteOffset => Memory.ReadUInt8(BaseAddress, 0x71);
        public byte ByteMask => Memory.ReadUInt8(BaseAddress, 0x72);
        public byte FieldMask => Memory.ReadUInt8(BaseAddress, 0x73);
        public UInt32 BitMask => Memory.ReadUInt32(BaseAddress, 0x70);
    }
    internal class UArrayProperty : UProperty
    {
        public UClass Inner => ReadStruct<UClass>(0x70);
    }
    internal class UStructProperty : UProperty
    {
        public UStruct Struct => ReadStruct<UStruct>(0x70);
    }
    internal class UObjectProperty : UProperty
    {
        public UClass PropertyClass => ReadStruct<UClass>(0x70);
    }

    internal class UClassProperty : UProperty
    {
    }
    internal class UEnumProperty : UProperty
    {
    }
    internal class UNumericProperty : UProperty
    {
    }
    internal class UInterfaceProperty : UProperty
    {
    }
}
