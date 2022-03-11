using Reality.ModLoader.Unreal.CoreUObject;

namespace NeoPolaris.Unreal.Classes
{
    internal class UPlayer : UObject
    {
        public APlayerController PlayerController => FindProperty<APlayerController>("ObjectProperty /Script/Engine.Player.PlayerController");
    }
}
