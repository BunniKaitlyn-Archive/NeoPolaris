using NeoPolaris.Unreal.Classes;

namespace NeoPolaris.Fortnite.Classes
{
    internal class AFortPawn : ACharacter
    {
        public UFortAbilitySystemComponent AbilitySystemComponent => GetProperty<UFortAbilitySystemComponent>("ObjectProperty /Script/FortniteGame.FortPawn.AbilitySystemComponent");
    }
}
