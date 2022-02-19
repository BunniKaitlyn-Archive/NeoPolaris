using NeoPolaris.Unreal.Structs;

namespace NeoPolaris.Unreal.Classes
{
    internal class APlayerController : AController
    {
        public void SwitchLevel(string url)
        {
            var func = FindObject("Function /Script/Engine.PlayerController.SwitchLevel");
            App.ProcessEvent(BaseAddress, func.BaseAddress, new FString(url).BaseAddress);
        }
    }
}
