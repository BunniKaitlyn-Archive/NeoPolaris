namespace NeoPolaris.Unreal.Classes
{
    internal class UWorld : UObject
    {
        public AGameModeBase AuthorityGameMode => GetPropertyStruct<AGameModeBase>(BaseAddress, "ObjectProperty /Script/Engine.World.AuthorityGameMode");
    }
}
