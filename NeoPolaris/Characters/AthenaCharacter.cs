using NeoPolaris.Unreal.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoPolaris.Pawns
{
    public class AthenaCharacter : ICharacter
    {
        public UClass Clazz { get; set; }
        public ACharacter Instance { get; set; }
    }
}
