using NeoPolaris.Memory;
using NeoPolaris.Unreal.Structs;
using System;

namespace NeoPolaris.Unreal.Classes
{
    /// <summary>
    /// Represents an object in UE4.
    /// Read-only, because currently there is no point in crafting our own UObject.
    /// </summary>
    internal class UObject : MemoryObject
    {
        public IntPtr VTable => ReadIntPtr(0);
        public int ObjectFlags => ReadInt32(8);
        public int InternalIndex => ReadInt32(0xC);
        public UClass Class => ReadStruct<UClass>(0x10);
        public FName Name => ReadStruct<FName>(0x18, false);
        public UObject Outer => ReadStruct<UObject>(0x20);

        public static implicit operator UObject(IntPtr baseAddress)
            => new UObject { BaseAddress = baseAddress };

        public string GetName()
            => Name.ToString();

        public string GetFullName()
        {
            var name = string.Empty;

            if (Class != null)
            {
                var temp = string.Empty;
                for (var outer = Outer; outer != null; outer = outer.Outer)
                    temp = $"{outer.GetName()}.{temp}";

                name = Class.GetName();
                name += " ";
                name += temp;
                name += GetName();
            }

            return name;
        }

        public T Cast<T>() where T : UObject, new()
            => new T { BaseAddress = this.BaseAddress };

        public override int ObjectSize => 0x28;
    }
}
