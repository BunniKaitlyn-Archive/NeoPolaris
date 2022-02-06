using System;

namespace NeoPolaris.Unreal.Classes
{
    internal class UProperty : UField
    {
        public int Offset => ReadInt32(0x44);
    }
}
