namespace NeoPolaris.Unreal.Classes
{
    internal class ACharacter : APawn
    {
        public USkeletalMeshComponent Mesh => GetPropertyStruct<USkeletalMeshComponent>(BaseAddress, "ObjectProperty /Script/Engine.Character.Mesh");
    }
}
