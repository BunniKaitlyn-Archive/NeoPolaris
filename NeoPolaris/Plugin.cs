using NeoPolaris.Fortnite.Classes;
using NeoPolaris.Unreal.Classes;
using NeoPolaris.Unreal.Misc;
using Reality.ModLoader;
using Reality.ModLoader.Common;
using Reality.ModLoader.Memory;
using Reality.ModLoader.Unreal.Core;
using Reality.ModLoader.Unreal.CoreUObject;
using Reality.ModLoader.Utilities;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using static Reality.ModLoader.Utilities.Win32;

namespace NeoPolaris
{
    public class Plugin : IGamePlugin
    {
        public string Name => "Polaris";
        public string Author => "Kaitlyn";
        public string Version => "1.0.0";

        private enum PluginState
        {
            Idle,
            InGame
        }

        private static PluginState _state;
        private static bool _isInitialized;

        private static UEngine _gEngine;

        private static UWorld _currentWorld;
        private static UClass _currentPawnClass;
        private static APlayerController _currentPlayerController;

        private static USkeletalMesh _currentSkeletalMesh;
        private static UFortAbilitySet _currentAbilitySet;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr StaticLoadObjectDelegate(IntPtr objectClass, IntPtr inOuter, IntPtr inName, IntPtr fileName, uint loadFlags, IntPtr sandbox, bool bAllowObjectReconciliation);
        private static StaticLoadObjectDelegate StaticLoadObject;

        private static T LoadObject<T>(UClass objectClass, string name) where T : UObject, new()
            => new T { BaseAddress = StaticLoadObject(objectClass.BaseAddress, IntPtr.Zero, new FString(name).Data, new FString().Data, 0, IntPtr.Zero, true) };

        private static UObject FindOrLoadObject(string pathName)
            => LoadObject<UObject>(Loader.Instance.Objects.FindObject<UClass>("Class /Script/CoreUObject.Object"), pathName);

        [Obfuscation(Feature = "virtualization", Exclude = false)]
        public static void SpawnPlayer()
        {
            var rotation = _currentPlayerController.GetActorRotation();
            rotation.Pitch = 1;

            var pawn = UGameplayStatics.SpawnActor<AFortPawn>(_currentWorld, _currentPawnClass, _currentPlayerController.GetActorLocation(), rotation);
            pawn.Mesh.SetSkeletalMesh(_currentSkeletalMesh, true);

            _currentPlayerController.Possess(pawn);

            for (var i = 0; i < _currentAbilitySet.GameplayAbilities.Count; i++)
                pawn.AbilitySystemComponent.ApplyGameplayAbilityToSelf(_currentAbilitySet.GameplayAbilities[i]);
        }

        [Obfuscation(Feature = "virtualization", Exclude = false)]
        public static void StartMatch()
        {
            _currentPawnClass = Loader.Instance.Objects.FindObject<UClass>("BlueprintGeneratedClass /Game/Athena/PlayerPawn_Athena.PlayerPawn_Athena_C");

            FindOrLoadObject("/Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.GE_Athena_PurpleStuff_C");

            _currentSkeletalMesh = Loader.Instance.Objects.FindObject<USkeletalMesh>("SkeletalMesh /Game/Characters/Survivors/Female/Small/F_SML_Starter_01/Meshes/F_SML_Starter_Epic.F_SML_Starter_Epic");
            _currentAbilitySet = Loader.Instance.Objects.FindObject<UFortAbilitySet>("FortAbilitySet /Game/Abilities/Player/Generic/Traits/DefaultPlayer/GAS_DefaultPlayer.GAS_DefaultPlayer");

            _isInitialized = true;

            _currentPlayerController.Cast<AFortPlayerController>().QuickBars = UGameplayStatics.SpawnActor<AFortQuickBars>(_currentWorld, Loader.Instance.Objects.FindObject<UClass>("Class /Script/FortniteGame.FortQuickBars"), new(), new());

            SpawnPlayer();

            _currentPlayerController.Cast<AFortPlayerController>().ServerReadyToStartMatch();
            _currentWorld.AuthorityGameMode.Cast<AGameMode>().StartMatch();
        }

        public void OnLoad()
        {
            StaticLoadObject = MemoryUtil.GetNativeFunc<StaticLoadObjectDelegate>(GetModuleHandle(null) + 0x142E560);
        }

        public bool OnProcessEvent(UObject obj, UObject func, IntPtr parms)
        {
            if (_gEngine == null)
            {
                _gEngine = Loader.Instance.Objects.FindObject<UFortEngine>("FortEngine /Engine/Transient.FortEngine_0");
                if (_gEngine is UFortEngine fortEngine)
                {
                    _currentWorld = fortEngine.GameViewport.World;
                    _currentPlayerController = fortEngine.GameInstance.LocalPlayers[0].PlayerController;

                    Logger.Info($"PlayerController = {_currentPlayerController.GetFullName()}");
                }
            }

            switch (_state)
            {
                case PluginState.Idle:
                    if (func.GetName().Contains("BP_PlayButton"))
                    {
                        _currentPlayerController.SwitchLevel("Athena_Terrain");
                        _state = PluginState.InGame;
                    }
                    break;

                case PluginState.InGame:
                    if (!_isInitialized && func.GetName().Contains("ReadyToStartMatch"))
                    {
                        if (_gEngine is UFortEngine fortEngine)
                        {
                            _currentWorld = fortEngine.GameViewport.World;
                            _currentPlayerController = fortEngine.GameInstance.LocalPlayers[0].PlayerController;

                            Logger.Info($"PlayerController = {_currentPlayerController.GetFullName()}");
                        }

                        StartMatch();
                    }
                    if (_isInitialized && func.GetName().Contains("ServerAttemptAircraftJump"))
                        SpawnPlayer();
                    break;
            }
            return true;
        }
    }
}
