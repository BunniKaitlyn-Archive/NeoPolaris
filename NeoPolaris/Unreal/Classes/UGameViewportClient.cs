namespace NeoPolaris.Unreal.Classes
{
    internal class UGameViewportClient : UScriptViewportClient
    {
        public UWorld World => GetProperty<UWorld>("ObjectProperty /Script/Engine.GameViewportClient.World");
    }
}
