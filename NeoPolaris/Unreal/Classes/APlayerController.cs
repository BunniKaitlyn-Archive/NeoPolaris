using Reality.ModLoader;
using Reality.ModLoader.Unreal.Core;

namespace NeoPolaris.Unreal.Classes
{
    internal class APlayerController : AController
    {
        public void SwitchLevel(string url)
        {
            var func = FindObject("Function /Script/Engine.PlayerController.SwitchLevel");
            Loader.ProcessEvent(BaseAddress, func.BaseAddress, new FString(url).BaseAddress);
        }
    }
}
