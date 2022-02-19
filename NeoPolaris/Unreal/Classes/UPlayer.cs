namespace NeoPolaris.Unreal.Classes
{
    internal class UPlayer : UObject
    {
        public APlayerController PlayerController => GetProperty<APlayerController>("ObjectProperty /Script/Engine.Player.PlayerController");
    }
}
