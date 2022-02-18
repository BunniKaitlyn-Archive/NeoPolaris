using NeoPolaris.Unreal.Classes;
using NeoPolaris.Unreal.Structs;

namespace NeoPolaris.Fortnite.Classes
{
    internal class UFortAbilitySet : UPrimaryDataAsset
    {
        public TArray<UClass> GameplayAbilities => GetPropertyStruct<TArray<UClass>>(BaseAddress, "ArrayProperty /Script/FortniteGame.FortAbilitySet.GameplayAbilities", false);
    }
}
