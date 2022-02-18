namespace NeoPolaris.Unreal.Classes
{
    public class UField : UObject
    {
        public UField Next => ReadStruct<UField>(0x28);
    }
}
