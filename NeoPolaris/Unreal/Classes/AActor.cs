using NeoPolaris.Unreal.Structs;
using System.Runtime.InteropServices;

namespace NeoPolaris.Unreal.Classes
{
    public class AActor : UObject
    {
        public FRotator GetActorRotation()
        {
            var func = GetFunction("Function /Script/Engine.Actor.K2_GetActorRotation");
            var ptr = Marshal.AllocHGlobal(0xC);
            App.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
            return new FRotator { BaseAddress = ptr };
        }

        public FVector GetActorLocation()
        {
            var func = GetFunction("Function /Script/Engine.Actor.K2_GetActorLocation");
            var ptr = Marshal.AllocHGlobal(0xC);
            App.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
            return new FVector { BaseAddress = ptr };
        }
    }
}
