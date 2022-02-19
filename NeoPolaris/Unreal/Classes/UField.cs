namespace NeoPolaris.Unreal.Classes
{
    internal class UField : UObject
    {
        public UField Next => ReadStruct<UField>(0x28);
    }
}
