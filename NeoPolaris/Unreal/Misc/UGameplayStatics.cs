using NeoPolaris.Unreal.Classes;
using NeoPolaris.Unreal.Enums;

namespace NeoPolaris.Unreal.Misc
{
    internal class UGameplayStatics : UBlueprintFunctionLibrary
    {
        private static UGameplayStatics _default;

        static UGameplayStatics()
        {
            _default = (UGameplayStatics)App.Instance.Objects.FindObject("GameplayStatics Engine.Default__GameplayStatics");
        }

        public static AActor BeginDeferredActorSpawnFromClass(UObject worldContextObject, UClass actorClass, ESpawnActorCollisionHandlingMethod collisionHandlingOverride, AActor owner)
        {
            return null;
        }
    }
}
