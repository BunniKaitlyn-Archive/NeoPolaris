using NeoPolaris.Memory;
using NeoPolaris.Utilities;
using NeoPolaris.Unreal.Classes;
using NeoPolaris.Unreal.Stores;
using System;
using System.IO;
using System.Runtime.InteropServices;
using NeoPolaris.Fortnite.Classes;
using NeoPolaris.Unreal.Misc;
using NeoPolaris.Unreal.Structs;
using NeoPolaris.Natives;
using System.Reflection;

namespace NeoPolaris
{
    internal class App
    {
        #region "Singletons"

        private static App _instance;
        public static App Instance => _instance ?? (_instance = new());

        private static SDK.Generator _sdkGenerator;
        public static SDK.Generator SdkGenerator => _sdkGenerator ?? (_sdkGenerator = new());

        #endregion

        #region "Memory"

        public IMemory Memory { get; private set; }

        #endregion

        #region "Paths"

        public static string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Polaris");
        public static string SdkPath => Path.Combine(DataPath, "SDK");
        public static string NativesPath => Path.Combine(DataPath, "Natives");
        public static string PluginsPath => Path.Combine(DataPath, "Plugins");

        #endregion

        #region "Stores"

        public ObjectStore Objects { get; private set; }

        #endregion

        #region "Misc"

        public enum AppState
        {
            Idle,
            InGame
        }

        public AppState State { get; private set; }
        public bool Initialized { get; private set; }

        public UEngine GEngine { get; private set; }

        public APlayerController CurrentPlayerController { get; private set; }
        public UWorld CurrentWorld { get; private set; }

        public UClass CurrentPawnClass { get; private set; }

        public USkeletalMesh CurrentSkeletalMesh { get; private set; }
        public UFortAbilitySet CurrentAbilitySet { get; private set; }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr StaticLoadObjectDelegate(IntPtr objectClass, IntPtr inOuter, IntPtr inName, IntPtr fileName, uint loadFlags, IntPtr sandbox, bool bAllowObjectReconciliation);
        public static StaticLoadObjectDelegate StaticLoadObject;

        public static T LoadObject<T>(UClass objectClass, string name) where T : UObject, new()
            => new T { BaseAddress = StaticLoadObject(objectClass.BaseAddress, IntPtr.Zero, new FString(name).Data, new FString(string.Empty).Data, 0, IntPtr.Zero, true) };

        public static UObject FindOrLoadObject(string pathName)
            => LoadObject<UObject>(Instance.Objects.FindObject<UClass>("Class /Script/CoreUObject.Object"), pathName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ProcessEventDelegate(IntPtr thisPtr, IntPtr func, IntPtr parms);
        public static ProcessEventDelegate ProcessEvent;

        #endregion

        [Obfuscation(Feature = "virtualization", Exclude = false)]
        public void SpawnPlayer()
        {
            var rotation = CurrentPlayerController.GetActorRotation();
            rotation.Pitch = 1;

            var pawn = UGameplayStatics.SpawnActor<AFortPawn>(CurrentPawnClass, CurrentPlayerController.GetActorLocation(), rotation);
            pawn.Mesh.SetSkeletalMesh(CurrentSkeletalMesh, true);

            CurrentPlayerController.Possess(pawn);

            for (var i = 0; i < CurrentAbilitySet.GameplayAbilities.Count; i++)
                pawn.AbilitySystemComponent.ApplyGameplayAbilityToSelf(CurrentAbilitySet.GameplayAbilities[i]);
        }

        [Obfuscation(Feature = "virtualization", Exclude = false)]
        public void StartMatch()
        {
            CurrentPawnClass = Objects.FindObject<UClass>("BlueprintGeneratedClass /Game/Athena/PlayerPawn_Athena.PlayerPawn_Athena_C");

            FindOrLoadObject("/Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.GE_Athena_PurpleStuff_C");

            CurrentSkeletalMesh = Objects.FindObject<USkeletalMesh>("SkeletalMesh /Game/Characters/Survivors/Female/Small/F_SML_Starter_01/Meshes/F_SML_Starter_Epic.F_SML_Starter_Epic");
            CurrentAbilitySet = Objects.FindObject<UFortAbilitySet>("FortAbilitySet /Game/Abilities/Player/Generic/Traits/DefaultPlayer/GAS_DefaultPlayer.GAS_DefaultPlayer");

            Initialized = true;

            CurrentPlayerController.Cast<AFortPlayerController>().QuickBars = UGameplayStatics.SpawnActor<AFortQuickBars>(Objects.FindObject<UClass>("Class /Script/FortniteGame.FortQuickBars"), new(), new());

            SpawnPlayer();

            CurrentPlayerController.Cast<AFortPlayerController>().ServerReadyToStartMatch();
            CurrentWorld.AuthorityGameMode.Cast<AGameMode>().StartMatch();
        }

        private static void ProcessEventHook(IntPtr thisPtr, IntPtr func, IntPtr parms)
        {
            if (thisPtr != IntPtr.Zero && func != IntPtr.Zero)
            {
                //var thisObj = (UObject) thisPtr;
                var funcObj = (UObject) func;
                var funcName = funcObj.GetName();

                switch (Instance.State)
                {
                    case AppState.Idle:
                        if (funcName.Contains("BP_PlayButton"))
                        {
                            Instance.CurrentPlayerController.SwitchLevel("Athena_Terrain");
                            Instance.State = AppState.InGame;
                        }
                        break;

                    case AppState.InGame:
                        if (!Instance.Initialized && funcName.Contains("ReadyToStartMatch"))
                        {
                            if (Instance.GEngine is UFortEngine fortEngine)
                            {
                                Instance.CurrentPlayerController = fortEngine.GameInstance.LocalPlayers[0].PlayerController;
                                Instance.CurrentWorld = fortEngine.GameViewport.World;

                                Console.WriteLine($"PlayerController = {Instance.CurrentPlayerController.GetFullName()}");
                            }

                            Instance.StartMatch();
                        }
                        if (Instance.Initialized && funcName.Contains("ServerAttemptAircraftJump"))
                            Instance.SpawnPlayer();
                        break;
                }
            }

            ProcessEvent(thisPtr, func, parms);
        }

        public void Initialize()
        {
            // Create all directories.
            Directory.CreateDirectory(DataPath);
            Directory.CreateDirectory(SdkPath);
            Directory.CreateDirectory(NativesPath);
            Directory.CreateDirectory(PluginsPath);

            Memory = new InternalMemory();

            var objectsOffset = MemoryUtil.FindPattern(
                "\x48\x8D\x0D\x00\x00\x00\x00\xE8\x00\x00\x00\x00\xE8\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x48\x8B\xD6",
                "xxx????x????x????x????xxx"
            );
            Objects = new ObjectStore(MemoryUtil.GetAddressFromOffset(objectsOffset, 7, 3));

            //SdkGenerator.Generate();

            GEngine = Objects.FindObject<UFortEngine>("FortEngine /Engine/Transient.FortEngine_0");
            if (GEngine is UFortEngine fortEngine)
            {
                CurrentPlayerController = fortEngine.GameInstance.LocalPlayers[0].PlayerController;
                CurrentWorld = fortEngine.GameViewport.World;

                Console.WriteLine($"PlayerController = {CurrentPlayerController.GetFullName()}");
            }

            StaticLoadObject = MemoryUtil.GetNativeFunc<StaticLoadObjectDelegate>(Win32.GetModuleHandle(null) + 0x142E560);

            var processEventAddress = MemoryUtil.FindPattern(
                "\x40\x55\x56\x57\x41\x54\x41\x55\x41\x56\x41\x57\x48\x81\xEC\x00\x00\x00\x00\x48\x8D\x6C\x24\x00\x48\x89\x9D\x00\x00\x00\x00\x48\x8B\x05\x00\x00\x00\x00\x48\x33\xC5\x48\x89\x85\x00\x00\x00\x00\x48\x63\x41\x0C",
                "xxxxxxxxxxxxxxx????xxxx?xxx????xxx????xxxxxx????xxxx"
            );
            MinHook.CreateHook(processEventAddress, ProcessEventHook, out ProcessEvent);

            MinHook.EnableAllHooks();
        }
    }
}
