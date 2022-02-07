namespace NeoPolaris.Unreal.Classes
{
    internal class UGameViewportClient : UScriptViewportClient
    {
        public UWorld World => GetProperty<UWorld>(BaseAddress, "ObjectProperty /Script/Engine.GameViewportClient.World");
    }
}
