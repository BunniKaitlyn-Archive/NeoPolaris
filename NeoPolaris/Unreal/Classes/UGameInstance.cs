using NeoPolaris.Unreal.Structs;

namespace NeoPolaris.Unreal.Classes
{
    internal class UGameInstance : UObject
    {
        public TArray<ULocalPlayer> LocalPlayers => GetPropertyStruct<TArray<ULocalPlayer>>(BaseAddress, "ArrayProperty /Script/Engine.GameInstance.LocalPlayers", false);
    }
}
