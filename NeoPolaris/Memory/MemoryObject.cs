using System;

namespace NeoPolaris.Memory
{
    /// <summary>
    /// It's like <see cref="System.Object"/> but for raw memory.
    /// What more do you need to know? :')
    /// </summary>
    internal abstract class MemoryObject
    {
        public IMemory Memory => App.Instance.Memory;

        public IntPtr BaseAddress { get; set; }

        public int ReadInt32(int offset)
            => Memory.ReadInt32(BaseAddress, offset);

        public IntPtr ReadIntPtr(int offset)
            => Memory.ReadIntPtr(BaseAddress, offset);

        public T ReadStruct<T>(int offset, bool isPtr = true) where T : MemoryObject, new()
            => Memory.ReadStruct<T>(BaseAddress, offset, isPtr);

        public void WriteInt32(int offset, int value)
            => Memory.WriteInt32(BaseAddress, offset, value);

        public void WriteIntPtr(int offset, IntPtr value)
            => Memory.WriteIntPtr(BaseAddress, offset, value);

        public abstract int ObjectSize { get; }
    }
}
