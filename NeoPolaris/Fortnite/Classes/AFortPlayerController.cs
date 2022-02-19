using NeoPolaris.Unreal.Classes;
using System;

namespace NeoPolaris.Fortnite.Classes
{
    internal class AFortPlayerController : APlayerController
    {
        public AFortQuickBars QuickBars
        {
            get => GetProperty<AFortQuickBars>("ObjectProperty /Script/FortniteGame.FortPlayerController.QuickBars");
            set
            {
                var offset = GetProperty("ObjectProperty /Script/FortniteGame.FortPlayerController.QuickBars");
                WriteIntPtr(offset, value.BaseAddress);
            }
        }

        public void ServerReadyToStartMatch()
        {
            var func = FindObject("Function /Script/FortniteGame.FortPlayerController.ServerReadyToStartMatch");
            App.ProcessEvent(BaseAddress, func.BaseAddress, IntPtr.Zero);
        }
    }
}
