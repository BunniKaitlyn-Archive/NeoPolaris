using NeoPolaris.Unreal.Classes;
using System;

namespace NeoPolaris.Fortnite.Classes
{
    internal class AFortPlayerController : APlayerController
    {
        public AFortQuickBars QuickBars
        {
            get => GetPropertyStruct<AFortQuickBars>(BaseAddress, "ObjectProperty /Script/FortniteGame.FortPlayerController.QuickBars");
            set
            {
                var offset = GetPropertyOffset("ObjectProperty /Script/FortniteGame.FortPlayerController.QuickBars");
                WriteIntPtr(offset, value.BaseAddress);
            }
        }

        public void ServerReadyToStartMatch()
        {
            var func = GetFunction("Function /Script/FortniteGame.FortPlayerController.ServerReadyToStartMatch");
            App.ProcessEvent(BaseAddress, func.BaseAddress, IntPtr.Zero);
        }
    }
}
