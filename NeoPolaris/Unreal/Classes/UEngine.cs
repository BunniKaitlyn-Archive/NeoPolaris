namespace NeoPolaris.Unreal.Classes
{
    internal class UEngine : UObject
    {
        public UGameViewportClient GameViewport => GetProperty<UGameViewportClient>("ObjectProperty /Script/Engine.Engine.GameViewport");
    }
}
