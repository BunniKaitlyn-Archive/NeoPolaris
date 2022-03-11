using Reality.ModLoader.Unreal.CoreUObject;

namespace NeoPolaris.Unreal.Classes
{
    internal class UWorld : UObject
    {
        public AGameModeBase AuthorityGameMode => FindProperty<AGameModeBase>("ObjectProperty /Script/Engine.World.AuthorityGameMode");
    }
}
