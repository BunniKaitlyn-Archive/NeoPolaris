using NeoPolaris.Memory;
using NeoPolaris.Utilities;
using NeoPolaris.Unreal.Classes;
using NeoPolaris.Unreal.Stores;
using System;
using System.IO;
using System.Runtime.InteropServices;
using NeoPolaris.Fortnite.Classes;
using NeoPolaris.Unreal.Misc;

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
        public static string NativesPath => Path.Combine(DataPath, "Natives");
        public static string PluginsPath => Path.Combine(DataPath, "Plugins");

        #endregion

        #region "Stores"

        public ObjectStore Objects { get; private set; }

        #endregion

        #region "Misc"

        public UEngine GEngine { get; private set; }

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

        #endregion

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
                        if (funcName.Contains("ReadyToStartMatch") && Instance.Status == GameState.None)
                        {
                            if (Instance.GEngine is UFortEngine fortEngine)
                            {
                                Instance.CurrentPlayerController = fortEngine.GameInstance.LocalPlayers[0].PlayerController;
                                Instance.CurrentWorld = fortEngine.GameViewport.World;

                                Console.WriteLine($"PlayerController = {Instance.CurrentPlayerController.GetFullName()}");
                            }

                            Instance.Status = GameState.Initialized;

                            var pawnClass = Instance.Objects.FindObject<UClass>("BlueprintGeneratedClass /Game/Athena/PlayerPawn_Athena.PlayerPawn_Athena_C");
                            var pawn = UGameplayStatics.SpawnActor<APawn>(pawnClass, Instance.CurrentPlayerController.GetActorLocation(), Instance.CurrentPlayerController.GetActorRotation());

                            Instance.CurrentPlayerController.Possess(pawn);
                            Instance.CurrentPlayerController.Cast<AFortPlayerController>().ServerReadyToStartMatch();
                            Instance.CurrentWorld.AuthorityGameMode.Cast<AGameMode>().StartMatch();
                        }
                        break;
                }
            }

            ProcessEvent(thisPtr, func, parms);
        }

        public void Initialize()
        {
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
            MinHook.CreateHook(processEventAddress, ProcessEventHook, out ProcessEvent);

            if (GEngine is UFortEngine fortEngine)
            {
                CurrentPlayerController = fortEngine.GameInstance.LocalPlayers[0].PlayerController;
                CurrentWorld = fortEngine.GameViewport.World;

                Console.WriteLine($"PlayerController = {CurrentPlayerController.GetFullName()}");
            }

            MinHook.EnableAllHooks();
        }
    }
}
