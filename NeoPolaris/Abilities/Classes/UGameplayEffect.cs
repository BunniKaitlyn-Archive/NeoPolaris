using NeoPolaris.Abilities.Enums;
using NeoPolaris.Abilities.Structs;
using NeoPolaris.Unreal.Classes;
using NeoPolaris.Unreal.Structs;

namespace NeoPolaris.Abilities.Classes
{
    internal class UGameplayEffect : UObject
    {
        public EGameplayEffectDurationType DurationPolicy
        {
            get => (EGameplayEffectDurationType) Memory.ReadUInt8(BaseAddress, 0x30);
            set => Memory.WriteUInt8(BaseAddress, 0x30, (byte) value);
        }

        public TArray<FGameplayAbilitySpecDef> GrantedAbilities => GetPropertyStruct<TArray<FGameplayAbilitySpecDef>>(BaseAddress, "ArrayProperty /Script/GameplayAbilities.GameplayEffect.GrantedAbilities", false).SetIsPtr(false);
    }
}
