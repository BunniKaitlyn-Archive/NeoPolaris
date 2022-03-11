using NeoPolaris.Unreal.Structs;
using Reality.ModLoader;
using Reality.ModLoader.Unreal.Core;
using Reality.ModLoader.Unreal.CoreUObject;

namespace NeoPolaris.Unreal.Classes
{
    internal class AActor : UObject
    {
        public FRotator GetActorRotation()
        {
            var func = FindObject("Function /Script/Engine.Actor.K2_GetActorRotation");
            var ptr = FMemory.Malloc(0xC, 0);
            Loader.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
            return new FRotator { BaseAddress = ptr };
        }

        public FVector GetActorLocation()
        {
            var func = FindObject("Function /Script/Engine.Actor.K2_GetActorLocation");
            var ptr = FMemory.Malloc(0xC, 0);
            Loader.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
            return new FVector { BaseAddress = ptr };
        }
    }
}
