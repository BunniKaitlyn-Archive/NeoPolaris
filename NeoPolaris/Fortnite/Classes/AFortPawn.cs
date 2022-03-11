using NeoPolaris.Unreal.Classes;

namespace NeoPolaris.Fortnite.Classes
{
    internal class AFortPawn : ACharacter
    {
        public UFortAbilitySystemComponent AbilitySystemComponent => FindProperty<UFortAbilitySystemComponent>("ObjectProperty /Script/FortniteGame.FortPawn.AbilitySystemComponent");
    }
}
