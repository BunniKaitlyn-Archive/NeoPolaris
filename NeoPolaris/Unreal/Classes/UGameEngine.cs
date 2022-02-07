namespace NeoPolaris.Unreal.Classes
{
    internal class UGameEngine : UEngine
    {
        public UGameInstance GameInstance => GetProperty<UGameInstance>(BaseAddress, "ObjectProperty /Script/Engine.GameEngine.GameInstance");
    }
}
