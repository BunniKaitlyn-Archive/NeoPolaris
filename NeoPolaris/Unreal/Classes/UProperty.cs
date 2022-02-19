namespace NeoPolaris.Unreal.Classes
{
    internal class UProperty : UField
    {
        public int ElementSize => ReadInt32(0x34);
        public int Offset => ReadInt32(0x44);
    }
}
