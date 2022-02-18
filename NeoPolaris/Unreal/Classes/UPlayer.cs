namespace NeoPolaris.Unreal.Classes
{
    internal class UPlayer : UObject
    {
        public APlayerController PlayerController => GetPropertyStruct<APlayerController>(BaseAddress, "ObjectProperty /Script/Engine.Player.PlayerController");
    }
}
