using NeoPolaris.Memory;
using NeoPolaris.Unreal.Classes;
using System;

namespace NeoPolaris.Unreal.Stores
{
    public class ObjectStore : MemoryObject
    {
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
            for (var i = 0; i < Count; i++)
            {
                var obj = GetObject<T>(i);
                if (obj != null)
                {
                    if (obj.GetFullName() == fullName)
                        return obj;
                }
            }
            return null;
        }

        public override int ObjectSize => 0;
    }
}