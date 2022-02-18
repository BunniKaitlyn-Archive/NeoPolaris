using NeoPolaris.Memory;
using NeoPolaris.Utilities;
using NeoPolaris.Unreal.Classes;
using NeoPolaris.Unreal.Stores;
using System;
using System.IO;
using System.Runtime.InteropServices;
using NeoPolaris.Fortnite.Classes;
using NeoPolaris.Unreal.Misc;
using NeoPolaris.Abilities.Classes;
using NeoPolaris.Unreal.Structs;
using NeoPolaris.SDK;

namespace NeoPolaris
{
    internal class App
    {
        #region "Singleton"

        private static App _instance;
        public static App Instance => _instance ?? (_instance = new App());

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

        public UEngine GEngine { get; private set; }

        public UGameplayEffect DefaultGameplayEffect { get; private set; }
        public UClass GameplayEffectClass { get; private set; }

        public UClass PawnClass { get; private set; }

        public APlayerController CurrentPlayerController { get; private set; }
        public UWorld CurrentWorld { get; private set; }

        public enum AppState
        {
            Idle,
            InGame
        }

        public AppState State { get; private set; } = AppState.Idle;

        public enum GameState
        {
            None,
            Initialized
        }

        public GameState Status { get; private set; } = GameState.None;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ProcessEventDelegate(IntPtr thisPtr, IntPtr func, IntPtr parms);
        public static ProcessEventDelegate ProcessEvent;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr StaticLoadObjectDelegate(IntPtr objectClass, IntPtr inOuter, IntPtr inName, IntPtr filename, uint loadFlags, IntPtr sandbox, bool bAllowObjectReconciliation);
        public static StaticLoadObjectDelegate StaticLoadObject;

        #endregion

        public static T LoadObject<T>(UClass objectClass, string name, string filename = "", uint loadFlags = 0) where T : UObject, new()
            => new T { BaseAddress = StaticLoadObject(objectClass.BaseAddress, IntPtr.Zero, new FString(name).Data, new FString(filename).Data, loadFlags, IntPtr.Zero, true) };

        public static UObject FindOrLoadObject(string pathName)
            => LoadObject<UObject>(Instance.Objects.FindObject<UClass>("Class /Script/CoreUObject.Object"), pathName);

        private static void ApplyGameplayAbilityToSelf(AFortPawn pawn, UClass ability)
        {
            if (Instance.DefaultGameplayEffect == null)
            {
                Instance.DefaultGameplayEffect = Instance.Objects.FindObject<UGameplayEffect>("GE_Athena_PurpleStuff_C /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.Default__GE_Athena_PurpleStuff_C");
                if (Instance.DefaultGameplayEffect == null)
                    Instance.DefaultGameplayEffect = Instance.Objects.FindObject<UGameplayEffect>("GE_Athena_PurpleStuff_Health_C /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff_Health.Default__GE_Athena_PurpleStuff_Health_C");
            }

            Instance.DefaultGameplayEffect.GrantedAbilities[0].Ability = ability;
            Instance.DefaultGameplayEffect.DurationPolicy = Abilities.Enums.EGameplayEffectDurationType.Infinite;

            if (Instance.GameplayEffectClass == null)
            {
                Instance.GameplayEffectClass = Instance.Objects.FindObject<UClass>("BlueprintGeneratedClass /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.GE_Athena_PurpleStuff_C");
                if (Instance.GameplayEffectClass == null)
                    Instance.GameplayEffectClass = Instance.Objects.FindObject<UClass>("BlueprintGeneratedClass /Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff_Health.GE_Athena_PurpleStuff_Health_C");
            }

            Console.WriteLine($"Applying gameplay ability {ability.GetFullName()} to self...");

            pawn.AbilitySystemComponent.BP_ApplyGameplayEffectToSelf(Instance.GameplayEffectClass, 1);

            Console.WriteLine($"Applied gameplay ability {ability.GetFullName()} to self!");
        }

        private static USkeletalMesh _skeletalMesh;
        private static UFortAbilitySet _abilitySet;

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
                        if (Instance.Status == GameState.None && funcName.Contains("ReadyToStartMatch"))
                        {
                            if (Instance.GEngine is UFortEngine fortEngine)
                            {
                                Instance.CurrentPlayerController = fortEngine.GameInstance.LocalPlayers[0].PlayerController;
                                Instance.CurrentWorld = fortEngine.GameViewport.World;

                                Console.WriteLine($"PlayerController = {Instance.CurrentPlayerController.GetFullName()}");
                            }

                            FindOrLoadObject("/Game/Athena/Items/Consumables/PurpleStuff/GE_Athena_PurpleStuff.GE_Athena_PurpleStuff_C");

                            _skeletalMesh = Instance.Objects.FindObject<USkeletalMesh>("SkeletalMesh /Game/Characters/Survivors/Female/Small/F_SML_Starter_01/Meshes/F_SML_Starter_Epic.F_SML_Starter_Epic");
                            _abilitySet = Instance.Objects.FindObject<UFortAbilitySet>("FortAbilitySet /Game/Abilities/Player/Generic/Traits/DefaultPlayer/GAS_DefaultPlayer.GAS_DefaultPlayer");

                            Instance.Status = GameState.Initialized;

                            Instance.PawnClass = Instance.Objects.FindObject<UClass>("BlueprintGeneratedClass /Game/Athena/PlayerPawn_Athena.PlayerPawn_Athena_C");

                            var pawn = UGameplayStatics.SpawnActor<AFortPawn>(Instance.PawnClass, Instance.CurrentPlayerController.GetActorLocation(), Instance.CurrentPlayerController.GetActorRotation());
                            pawn.Mesh.SetSkeletalMesh(_skeletalMesh, true);
                            
                            Instance.CurrentPlayerController.Possess(pawn);

                            Instance.CurrentPlayerController.Cast<AFortPlayerController>().ServerReadyToStartMatch();
                            Instance.CurrentWorld.AuthorityGameMode.Cast<AGameMode>().StartMatch();

                            for (var i = 0; i < _abilitySet.GameplayAbilities.Count; i++)
                                ApplyGameplayAbilityToSelf(pawn, _abilitySet.GameplayAbilities[i]);
                        }
                        if (Instance.Status == GameState.Initialized && funcName.Contains("ServerAttemptAircraftJump"))
                        {
                            var pawn = UGameplayStatics.SpawnActor<AFortPawn>(Instance.PawnClass, Instance.CurrentPlayerController.GetActorLocation(), Instance.CurrentPlayerController.GetActorRotation());
                            pawn.Mesh.SetSkeletalMesh(_skeletalMesh, true);

                            Instance.CurrentPlayerController.Possess(pawn);

                            for (var i = 0; i < _abilitySet.GameplayAbilities.Count; i++)
                                ApplyGameplayAbilityToSelf(pawn, _abilitySet.GameplayAbilities[i]);
                        }
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

            GEngine = Objects.FindObject<UFortEngine>("FortEngine /Engine/Transient.FortEngine_0");

            var processEventAddress = MemoryUtil.FindPattern(
                "\x40\x55\x56\x57\x41\x54\x41\x55\x41\x56\x41\x57\x48\x81\xEC\x00\x00\x00\x00\x48\x8D\x6C\x24\x00\x48\x89\x9D\x00\x00\x00\x00\x48\x8B\x05\x00\x00\x00\x00\x48\x33\xC5\x48\x89\x85\x00\x00\x00\x00\x48\x63\x41\x0C",
                "xxxxxxxxxxxxxxx????xxxx?xxx????xxx????xxxxxx????xxxx"
            );
            //MinHook.CreateHook(processEventAddress, ProcessEventHook, out ProcessEvent);

            var shit = Win32.GetModuleHandle(null) + 0x142E560;
            StaticLoadObject = MemoryUtil.GetNativeFunc<StaticLoadObjectDelegate>(shit);

            //var playGliderOpenSoundOffset = MemoryUtil.FindPattern("\xE9\x00\x00\x00\x00\x48\x8B\xCB\xB2\x01", "x????xxxxx");
            //MinHook.CreateHook(MemoryUtil.GetAddressFromOffset(playGliderOpenSoundOffset), PlayGliderOpenSoundHook, out PlayGliderOpenSound);

            //var soundUnk0Offset = MemoryUtil.FindPattern("\xE8\x00\x00\x00\x00\x49\x89\x46\x30\x48\x85\xC0", "x????xxxxxxx");
            //MinHook.CreateHook(MemoryUtil.GetAddressFromOffset(soundUnk0Offset), SoundUnk0Hook, out SoundUnk0);

            if (GEngine is UFortEngine fortEngine)
            {
                CurrentPlayerController = fortEngine.GameInstance.LocalPlayers[0].PlayerController;
                CurrentWorld = fortEngine.GameViewport.World;

                Console.WriteLine($"PlayerController = {CurrentPlayerController.GetFullName()}");
            }

            var sdkGenerator = new SdkGenerator();
            sdkGenerator.Generate();

            //MinHook.EnableAllHooks();
        }
    }
}
