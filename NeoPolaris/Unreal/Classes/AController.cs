using System;
using System.Runtime.InteropServices;

namespace NeoPolaris.Unreal.Classes
{
    internal class AController : AActor
    {
        public void Possess(APawn inPawn)
        {
            var func = FindObject("Function /Script/Engine.Controller.Possess");
            var ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Memory.WriteIntPtr(ptr, 0, inPawn.BaseAddress);
            App.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
        }
    }
}
