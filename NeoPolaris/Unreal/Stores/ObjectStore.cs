using NeoPolaris.Memory;
using NeoPolaris.Unreal.Classes;
using System;
using System.Collections.Generic;

namespace NeoPolaris.Unreal.Stores
{
    internal class ObjectStore : MemoryObject
    {
        private static Dictionary<string, UObject> _cachedObjects = new();
        private static Dictionary<string, int> _cachedOffsets = new();

        public IntPtr Data => ReadIntPtr(0x10);
        public int Count => ReadInt32(0x1C);

        public ObjectStore(IntPtr baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public IntPtr GetObjectPtr(int index)
            => Memory.ReadIntPtr(Data, 0x18 * index);

        public T GetObject<T>(int index) where T : UObject, new()
        {
            var ptr = GetObjectPtr(index);
            return ptr != IntPtr.Zero ? new T { BaseAddress = ptr } : null;
        }

        public UObject this[int index] => GetObject<UObject>(index);

        public T FindObject<T>(string fullName) where T : UObject, new()
        {
            if (!_cachedObjects.TryGetValue(fullName, out var obj))
            {
                for (var i = 0; i < Count; i++)
                {
                    obj = GetObject<T>(i);
                    if (obj != null)
                    {
                        if (obj.GetFullName() == fullName)
                        {
                            _cachedObjects[fullName] = obj;
                            break;
                        }
                    }
                }
            }
            return obj.Cast<T>();
        }

        public int GetProperty(string fullName)
        {
            if (!_cachedOffsets.TryGetValue(fullName, out var offset))
            {
                var property = FindObject<UProperty>(fullName);
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

        public T GetProperty<T>(IntPtr baseAddress, string fullName, bool isPtr = true) where T : MemoryObject, new()
        {
            var offset = GetProperty(fullName);
            if (offset == 0)
                return null;
            return new T { BaseAddress = isPtr ? App.Instance.Memory.ReadIntPtr(baseAddress, offset) : (baseAddress + offset) };
        }

        public override int ObjectSize => 0;
    }
}