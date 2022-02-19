﻿using System;

namespace NeoPolaris.Unreal.Classes
{
    public class AGameMode : AGameModeBase
    {
        public void StartMatch()
        {
            var func = GetFunction("Function /Script/Engine.GameMode.StartMatch");
            App.ProcessEvent(BaseAddress, func.BaseAddress, IntPtr.Zero);
        }
    }
}
