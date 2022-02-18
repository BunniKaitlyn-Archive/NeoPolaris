namespace NeoPolaris.Unreal.Classes
{
    internal class UGameEngine : UEngine
    {
        public UGameInstance GameInstance => GetPropertyStruct<UGameInstance>(BaseAddress, "ObjectProperty /Script/Engine.GameEngine.GameInstance");
    }
}
