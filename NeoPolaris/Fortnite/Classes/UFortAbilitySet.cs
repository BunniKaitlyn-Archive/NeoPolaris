using NeoPolaris.Unreal.Classes;
using Reality.ModLoader.Unreal.Core;
using Reality.ModLoader.Unreal.CoreUObject;

namespace NeoPolaris.Fortnite.Classes
{
    internal class UFortAbilitySet : UPrimaryDataAsset
    {
        public TArray<UClass> GameplayAbilities => FindProperty<TArray<UClass>>("ArrayProperty /Script/FortniteGame.FortAbilitySet.GameplayAbilities", isPtr: false);
    }
}
