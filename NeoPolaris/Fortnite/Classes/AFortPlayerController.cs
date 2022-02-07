using NeoPolaris.Unreal.Classes;
using System;

namespace NeoPolaris.Fortnite.Classes
{
    internal class AFortPlayerController : APlayerController
    {
        public void ServerReadyToStartMatch()
        {
            var func = GetFunction("Function /Script/FortniteGame.FortPlayerController.ServerReadyToStartMatch");
            App.ProcessEvent(BaseAddress, func.BaseAddress, IntPtr.Zero);
        }
    }
}
