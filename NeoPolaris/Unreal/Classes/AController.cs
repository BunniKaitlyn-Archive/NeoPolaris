using Reality.ModLoader;
using Reality.ModLoader.Unreal.Core;
using System;

namespace NeoPolaris.Unreal.Classes
{
    internal class AController : AActor
    {
        public void Possess(APawn inPawn)
        {
            var func = FindObject("Function /Script/Engine.Controller.Possess");
            var ptr = FMemory.Malloc(IntPtr.Size, 0);
            Memory.WriteIntPtr(ptr, 0, inPawn.BaseAddress);
            Loader.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
        }
    }
}
