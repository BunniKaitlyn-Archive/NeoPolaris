namespace NeoPolaris.Unreal.Classes
{
    internal class UWorld : UObject
    {
        public AGameModeBase AuthorityGameMode => GetProperty<AGameModeBase>("ObjectProperty /Script/Engine.World.AuthorityGameMode");
    }
}
