namespace NeoPolaris.Unreal.Classes
{
    internal class UPlayer : UObject
    {
        public APlayerController PlayerController => GetProperty<APlayerController>(BaseAddress, "ObjectProperty /Script/Engine.Player.PlayerController");
    }
}
