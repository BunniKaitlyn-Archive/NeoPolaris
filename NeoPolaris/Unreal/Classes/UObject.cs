using NeoPolaris.Memory;
using NeoPolaris.Unreal.Structs;
using System;
using System.Collections.Generic;

namespace NeoPolaris.Unreal.Classes
{
    /// <summary>
    /// Represents an object in UE4.
    /// Read-only, because currently there is no point in crafting our own UObject.
    /// </summary>
    public class UObject : MemoryObject
    {
        private static Dictionary<string, UObject> _cachedObjects = new();
        private static Dictionary<string, int> _cachedOffsets = new();

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

        public bool IsA(string name, bool isClass = false)
        {
            if (Class == null)
                return false;

            var clazz = isClass ? Cast<UClass>() : Class;
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

        public bool IsA<T>(bool isClass = false)
            => IsA(typeof(T).Name.TrimStart(new[] { 'F', 'A', 'U' }), isClass);

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

        public static UObject GetFunction(string fullName)
        {
            if (!_cachedObjects.TryGetValue(fullName, out var obj))
            {
                obj = App.Instance.Objects.FindObject<UObject>(fullName);
                _cachedObjects[fullName] = obj;
            }
            return obj;
        }

        public static T GetPropertyStruct<T>(IntPtr baseAddress, string fullName, bool isPtr = true) where T : MemoryObject, new()
        {
            if (!_cachedOffsets.TryGetValue(fullName, out var offset))
            {
                var property = App.Instance.Objects.FindObject<UProperty>(fullName);
                if (property != null)
                {
                    offset = property.Offset;
                    _cachedOffsets[fullName] = offset;
                }
                else
                    return null;
            }
            return new T { BaseAddress = isPtr ? App.Instance.Memory.ReadIntPtr(baseAddress, offset) : (baseAddress + offset) };
        }

        public static int GetPropertyOffset(string fullName)
        {
            if (!_cachedOffsets.TryGetValue(fullName, out var offset))
            {
                var property = App.Instance.Objects.FindObject<UProperty>(fullName);
                if (property != null)
                {
                    offset = property.Offset;
                    _cachedOffsets[fullName] = offset;
                }
                else
                    return 0;
            }
            return offset;
        }

        public override int ObjectSize => 0x28;
    }
}
