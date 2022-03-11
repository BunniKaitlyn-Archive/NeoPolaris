using NeoPolaris.Abilities.Enums;
using NeoPolaris.Abilities.Structs;
using NeoPolaris.Tasks.Classes;
using Reality.ModLoader;
using Reality.ModLoader.Unreal.Core;
using Reality.ModLoader.Unreal.CoreUObject;

namespace NeoPolaris.Abilities.Classes
{
    internal class UAbilitySystemComponent : UGameplayTasksComponent
    {
        private static UGameplayEffect _gameplayEffectDefault;
        private static UClass _gameplayEffectClass;

        public FActiveGameplayEffectHandle BP_ApplyGameplayEffectToSelf(UClass gameplayEffectClass, float level)
        {
            var func = FindObject("Function /Script/GameplayAbilities.AbilitySystemComponent.BP_ApplyGameplayEffectToSelf");
            var ptr = FMemory.Malloc(0x50, 0);
            Memory.WriteIntPtr(ptr, 0, gameplayEffectClass.BaseAddress);
            Memory.WriteSingle(ptr, 8, level);
            Loader.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
            return Memory.ReadStruct<FActiveGameplayEffectHandle>(ptr, 0x24, false);
        }

        public void ApplyGameplayAbilityToSelf(UClass ability)
        {
            if (_gameplayEffectDefault == null)
            {
                _gameplayEffectDefault = Objects.FindObject<UGameplayEffect>("GE_Athena_PurpleStuff_C /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.Default__GE_Athena_PurpleStuff_C");
                if (_gameplayEffectDefault == null)
                    _gameplayEffectDefault = Objects.FindObject<UGameplayEffect>("GE_Athena_PurpleStuff_Health_C /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff_Health.Default__GE_Athena_PurpleStuff_Health_C");
            }

            _gameplayEffectDefault.GrantedAbilities[0].Ability = ability;
            _gameplayEffectDefault.DurationPolicy = EGameplayEffectDurationType.Infinite;

            if (_gameplayEffectClass == null)
            {
                _gameplayEffectClass = Objects.FindObject<UClass>("BlueprintGeneratedClass /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.GE_Athena_PurpleStuff_C");
                if (_gameplayEffectClass == null)
                    _gameplayEffectClass = Objects.FindObject<UClass>("BlueprintGeneratedClass /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff_Health.GE_Athena_PurpleStuff_Health_C");
            }
            
            BP_ApplyGameplayEffectToSelf(_gameplayEffectClass, 1);
        }
    }
}
