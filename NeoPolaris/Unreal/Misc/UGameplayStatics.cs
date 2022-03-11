using NeoPolaris.Unreal.Classes;
using NeoPolaris.Unreal.Structs;
using Reality.ModLoader;
using Reality.ModLoader.Memory;
using Reality.ModLoader.Unreal.Core;
using Reality.ModLoader.Unreal.CoreUObject;
using System;
using System.Runtime.InteropServices;

namespace NeoPolaris.Unreal.Misc
{
    internal class UGameplayStatics : UBlueprintFunctionLibrary
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr SpawnActorDelegate(IntPtr world, IntPtr clazz, IntPtr location, IntPtr rotation, IntPtr spawnParameters);
        public static SpawnActorDelegate SpawnActor_;

        static UGameplayStatics()
        {
            SpawnActor_ = MemoryUtil.GetNativeFuncFromPatternWithOffset<SpawnActorDelegate>("\xE8\x00\x00\x00\x00\x0F\x10\x04\x3E", "x????xxxx");
        }

        public static T SpawnActor<T>(UWorld world, UClass clazz, FVector location, FRotator rotation) where T : AActor, new()
        {
            var spawnParms = FMemory.Malloc(0x40, 0);
            var ptr = SpawnActor_(world.BaseAddress, clazz.BaseAddress, location.BaseAddress, rotation.BaseAddress, spawnParms);
            return new() { BaseAddress = ptr };
        }
    }
}
