namespace NeoPolaris.Unreal.Classes
{
    internal class ACharacter : APawn
    {
        public USkeletalMeshComponent Mesh => FindProperty<USkeletalMeshComponent>("ObjectProperty /Script/Engine.Character.Mesh");
    }
}
