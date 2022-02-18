using NeoPolaris.Unreal.Classes;

namespace NeoPolaris.Fortnite.Classes
{
    internal class AFortPawn : ACharacter
    {
        public UFortAbilitySystemComponent AbilitySystemComponent => GetPropertyStruct<UFortAbilitySystemComponent>(BaseAddress, "ObjectProperty /Script/FortniteGame.FortPawn.AbilitySystemComponent");
    }
}
