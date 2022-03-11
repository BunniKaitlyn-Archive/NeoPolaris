using NeoPolaris.Abilities.Enums;
using NeoPolaris.Abilities.Structs;
using Reality.ModLoader.Unreal.Core;
using Reality.ModLoader.Unreal.CoreUObject;

namespace NeoPolaris.Abilities.Classes
{
    internal class UGameplayEffect : UObject
    {
        public EGameplayEffectDurationType DurationPolicy
        {
            get => (EGameplayEffectDurationType) Memory.ReadUInt8(BaseAddress, 0x30);
            set => Memory.WriteUInt8(BaseAddress, 0x30, (byte) value);
        }

        public TArray<FGameplayAbilitySpecDef> GrantedAbilities => FindProperty<TArray<FGameplayAbilitySpecDef>>("ArrayProperty /Script/GameplayAbilities.GameplayEffect.GrantedAbilities", isPtr: false).IsPtr(false);
    }
}
