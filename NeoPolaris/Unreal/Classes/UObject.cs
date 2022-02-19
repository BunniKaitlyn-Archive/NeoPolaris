using NeoPolaris.Memory;
using NeoPolaris.Unreal.Stores;
using NeoPolaris.Unreal.Structs;
using System;
using System.Collections.Generic;

namespace NeoPolaris.Unreal.Classes
{
    /// <summary>
    /// Represents an object in UE4.
    /// Read-only, because currently there is no point in crafting our own UObject.
    /// </summary>
    internal class UObject : MemoryObject
    {
        public ObjectStore Objects => App.Instance.Objects;

        public IntPtr VTable => ReadIntPtr(0);
        public int ObjectFlags => ReadInt32(8);
        public int InternalIndex => ReadInt32(0xC);
        public UClass Class => ReadStruct<UClass>(0x10);
        public FName Name => ReadStruct<FName>(0x18, false);
        public UObject Outer => ReadStruct<UObject>(0x20);

        public static implicit operator UObject(IntPtr baseAddress)
            => new UObject { BaseAddress = baseAddress };

        public T Cast<T>() where T : UObject, new()
            => new T { BaseAddress = this.BaseAddress };

        public bool IsA(string name, bool isSelfClass = false)
        {
            if (Class == null)
                return false;

            var clazz = isSelfClass ? Cast<UClass>() : Class;
            do
            {
                if (clazz == null)
                    return false;
                if (clazz.GetName() == name)
                    return true;
                if (clazz.SuperField == null)
                    break;

                clazz = clazz.SuperField.Cast<UClass>();
            } while (clazz != null);

            return false;
        }

        public bool IsA<T>(bool isSelfClass = false)
            => IsA(typeof(T).Name.TrimStart(new[] { 'F', 'A', 'U' }), isSelfClass);

        public string GetPrefix()
        {
            if (IsA("ScriptStruct"))
                return "F";
            if (IsA("Actor", true))
                return "A";

            return "U";
        }

        public string GetName(bool withPrefix = false)
            => (withPrefix ? GetPrefix() : string.Empty) + Name.ToString();

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

        public T FindObject<T>(string fullName) where T : UObject, new()
            => App.Instance.Objects.FindObject<T>(fullName);

        public UObject FindObject(string fullName)
            => App.Instance.Objects.FindObject(fullName);

        public int GetProperty(string fullName)
            => App.Instance.Objects.GetProperty(fullName);

        public T GetProperty<T>(string fullName, bool isPtr = true) where T : MemoryObject, new()
            => App.Instance.Objects.GetProperty<T>(BaseAddress, fullName, isPtr);

        public override int ObjectSize => 0x28;
    }
}
