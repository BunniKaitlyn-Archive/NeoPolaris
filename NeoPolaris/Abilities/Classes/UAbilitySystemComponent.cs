using NeoPolaris.Abilities.Structs;
using NeoPolaris.Tasks.Classes;
using NeoPolaris.Unreal.Classes;
using NeoPolaris.Utilities;
using System.Runtime.InteropServices;

namespace NeoPolaris.Abilities.Classes
{
    internal class UAbilitySystemComponent : UGameplayTasksComponent
    {
        public FActiveGameplayEffectHandle BP_ApplyGameplayEffectToSelf(UClass gameplayEffectClass, float level)
        {
            var func = GetFunction("Function /Script/GameplayAbilities.AbilitySystemComponent.BP_ApplyGameplayEffectToSelf");
            var ptr = Marshal.AllocHGlobal(0x50);
            Win32.RtlFillMemory(ptr, 0x50, 0);
            Memory.WriteIntPtr(ptr, 0, gameplayEffectClass.BaseAddress);
            Memory.WriteSingle(ptr, 8, level);
            //Memory.WriteBytes(ptr, 0xC, new byte[0x18]);
            App.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
            return Memory.ReadStruct<FActiveGameplayEffectHandle>(ptr, 0x24, false);
        }
    }
}
