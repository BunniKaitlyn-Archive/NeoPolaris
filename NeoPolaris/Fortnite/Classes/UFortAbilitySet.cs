using NeoPolaris.Unreal.Classes;
using NeoPolaris.Unreal.Structs;

namespace NeoPolaris.Fortnite.Classes
{
    internal class UFortAbilitySet : UPrimaryDataAsset
    {
        public TArray<UClass> GameplayAbilities => GetProperty<TArray<UClass>>("ArrayProperty /Script/FortniteGame.FortAbilitySet.GameplayAbilities", false);
    }
}
