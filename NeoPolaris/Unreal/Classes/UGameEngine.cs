namespace NeoPolaris.Unreal.Classes
{
    internal class UGameEngine : UEngine
    {
        public UGameInstance GameInstance => GetProperty<UGameInstance>("ObjectProperty /Script/Engine.GameEngine.GameInstance");
    }
}
