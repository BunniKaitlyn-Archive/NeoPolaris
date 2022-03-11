using Reality.ModLoader;
using Reality.ModLoader.Memory;
using Reality.ModLoader.Unreal.CoreUObject;

namespace NeoPolaris.Abilities.Structs
{
    internal class FGameplayAbilitySpecDef : MemoryObject
    {
        public UClass Ability
        {
            get => ReadStruct<UClass>(Loader.Instance.Objects.FindProperty("ClassProperty /Script/GameplayAbilities.GameplayAbilitySpecDef.Ability"));
            set => WriteIntPtr(Loader.Instance.Objects.FindProperty("ClassProperty /Script/GameplayAbilities.GameplayAbilitySpecDef.Ability"), value.BaseAddress);
        }

        public override int ObjectSize => 0x50;
    }
}
