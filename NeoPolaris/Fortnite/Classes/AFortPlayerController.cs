using NeoPolaris.Unreal.Classes;
using Reality.ModLoader;
using System;

namespace NeoPolaris.Fortnite.Classes
{
    internal class AFortPlayerController : APlayerController
    {
        public AFortQuickBars QuickBars
        {
            get => FindProperty<AFortQuickBars>("ObjectProperty /Script/FortniteGame.FortPlayerController.QuickBars");
            set => WriteIntPtr(FindProperty("ObjectProperty /Script/FortniteGame.FortPlayerController.QuickBars"), value.BaseAddress);
        }

        public void ServerReadyToStartMatch()
        {
            var func = FindObject("Function /Script/FortniteGame.FortPlayerController.ServerReadyToStartMatch");
            Loader.ProcessEvent(BaseAddress, func.BaseAddress, IntPtr.Zero);
        }
    }
}
