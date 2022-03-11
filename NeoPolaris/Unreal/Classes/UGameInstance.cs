using Reality.ModLoader.Unreal.Core;
using Reality.ModLoader.Unreal.CoreUObject;

namespace NeoPolaris.Unreal.Classes
{
    internal class UGameInstance : UObject
    {
        public TArray<ULocalPlayer> LocalPlayers => FindProperty<TArray<ULocalPlayer>>("ArrayProperty /Script/Engine.GameInstance.LocalPlayers", isPtr: false);
    }
}
