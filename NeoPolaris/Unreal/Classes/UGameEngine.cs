namespace NeoPolaris.Unreal.Classes
{
    internal class UGameEngine : UEngine
    {
        public UGameInstance GameInstance => FindProperty<UGameInstance>("ObjectProperty /Script/Engine.GameEngine.GameInstance");
    }
}
