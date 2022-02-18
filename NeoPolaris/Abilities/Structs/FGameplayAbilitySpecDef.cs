using NeoPolaris.Memory;
using NeoPolaris.Unreal.Classes;

namespace NeoPolaris.Abilities.Structs
{
    internal class FGameplayAbilitySpecDef : MemoryObject
    {
        public UClass Ability
        {
            get => ReadStruct<UClass>(UObject.GetPropertyOffset("ClassProperty /Script/GameplayAbilities.GameplayAbilitySpecDef.Ability"));
            set => WriteIntPtr(UObject.GetPropertyOffset("ClassProperty /Script/GameplayAbilities.GameplayAbilitySpecDef.Ability"), value.BaseAddress);
        }

        public override int ObjectSize => 0x50;
    }
}
