namespace NeoPolaris.Unreal.Classes
{
    internal class ACharacter : APawn
    {
        public USkeletalMeshComponent Mesh => GetProperty<USkeletalMeshComponent>("ObjectProperty /Script/Engine.Character.Mesh");
    }
}
