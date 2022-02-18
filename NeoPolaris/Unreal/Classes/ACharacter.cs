namespace NeoPolaris.Unreal.Classes
{
    public class ACharacter : APawn
    {
        public USkeletalMeshComponent Mesh => GetPropertyStruct<USkeletalMeshComponent>(BaseAddress, "ObjectProperty /Script/Engine.Character.Mesh");
    }
}
