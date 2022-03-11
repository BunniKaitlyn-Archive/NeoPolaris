using Reality.ModLoader.Unreal.CoreUObject;

namespace NeoPolaris.Unreal.Classes
{
    internal class UEngine : UObject
    {
        public UGameViewportClient GameViewport => FindProperty<UGameViewportClient>("ObjectProperty /Script/Engine.Engine.GameViewport");
    }
}
