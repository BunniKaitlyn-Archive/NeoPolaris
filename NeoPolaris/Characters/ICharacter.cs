using NeoPolaris.Unreal.Classes;

namespace NeoPolaris.Pawns
{
    public interface ICharacter
    {
        /// <summary>
        /// Contains a reference to the internal class of a character.
        /// </summary>
        public UClass Clazz { get; set; }
        /// <summary>
        /// Contains the instance of a character.
        /// </summary>
        public ACharacter Instance { get; set; }
    }
}
