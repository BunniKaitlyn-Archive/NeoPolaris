using NeoPolaris.Memory;
using NeoPolaris.Unreal.Classes;

namespace NeoPolaris.Abilities.Structs
{
    internal class FGameplayAbilitySpecDef : MemoryObject
    {
        public UClass Ability
        {
            get => ReadStruct<UClass>(App.Instance.Objects.GetProperty("ClassProperty /Script/GameplayAbilities.GameplayAbilitySpecDef.Ability"));
            set => WriteIntPtr(App.Instance.Objects.GetProperty("ClassProperty /Script/GameplayAbilities.GameplayAbilitySpecDef.Ability"), value.BaseAddress);
        }

        public override int ObjectSize => 0x50;
    }
}
