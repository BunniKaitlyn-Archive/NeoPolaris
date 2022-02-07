namespace NeoPolaris.Unreal.Classes
{
    internal class UWorld : UObject
    {
        public AGameModeBase AuthorityGameMode => GetProperty<AGameModeBase>(BaseAddress, "ObjectProperty /Script/Engine.World.AuthorityGameMode");
    }
}
