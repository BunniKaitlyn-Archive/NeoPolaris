namespace NeoPolaris.Unreal.Classes
{
    internal class UStruct : UField
    {
        public UStruct SuperField => ReadStruct<UStruct>(0x30);
        public UField Children => ReadStruct<UField>(0x38);
        public int PropertySize => ReadInt32(0x40);
    }
}
