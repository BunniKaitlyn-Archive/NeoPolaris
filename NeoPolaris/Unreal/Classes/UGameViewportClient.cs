namespace NeoPolaris.Unreal.Classes
{
    internal class UGameViewportClient : UScriptViewportClient
    {
        public UWorld World => GetPropertyStruct<UWorld>(BaseAddress, "ObjectProperty /Script/Engine.GameViewportClient.World");
    }
}
