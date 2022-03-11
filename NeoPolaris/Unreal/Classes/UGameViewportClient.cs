namespace NeoPolaris.Unreal.Classes
{
    internal class UGameViewportClient : UScriptViewportClient
    {
        public UWorld World => FindProperty<UWorld>("ObjectProperty /Script/Engine.GameViewportClient.World");
    }
}
