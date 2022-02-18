namespace NeoPolaris.Unreal.Classes
{
    internal class UEngine : UObject
    {
        public UGameViewportClient GameViewport => GetPropertyStruct<UGameViewportClient>(BaseAddress, "ObjectProperty /Script/Engine.Engine.GameViewport");
    }
}
