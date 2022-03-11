using Reality.ModLoader;
using System;

namespace NeoPolaris.Unreal.Classes
{
    internal class AGameMode : AGameModeBase
    {
        public void StartMatch()
        {
            var func = FindObject("Function /Script/Engine.GameMode.StartMatch");
            Loader.ProcessEvent(BaseAddress, func.BaseAddress, IntPtr.Zero);
        }
    }
}
