using NeoPolaris.Memory;
using System;
using System.Collections.Generic;

namespace NeoPolaris.Unreal.Structs
{
    /// <summary>
    /// Represents an array in UE4, offsets should *usually* never change here.
    /// </summary>
    internal class TArray<T> : MemoryObject where T : MemoryObject, new()
    {
        public IntPtr Data
        {
            get => ReadIntPtr(0);
            set => WriteIntPtr(0, value);
        }

        public int Count
        {
            get => ReadInt32(IntPtr.Size);
            set => WriteInt32(IntPtr.Size, value);
        }

        public int Max
        {
            get => ReadInt32(IntPtr.Size + 4);
            set => WriteInt32(IntPtr.Size + 4, value);
        }

        public TArray()
        { }

        public TArray(IntPtr baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public TArray(IntPtr baseAddress, int elementSize, bool isPtr = true)
        {
            BaseAddress = baseAddress;
            ElementSize = elementSize;
            IsPtr = isPtr;
        }

        public T GetElement(int index)
        {
            if (IsPtr)
                return new() { BaseAddress = Memory.ReadIntPtr(Data, index * ElementSize) };
            else
                return new() { BaseAddress = Data + (index * ElementSize) };
        }

        public T this[int index] => GetElement(index);

        public T[] ToArray()
        {
            var elements = new List<T>();
            for (var i = 0; i < Count; i++)
                elements.Add(GetElement(i));
            return elements.ToArray();
        }

        public int ElementSize = IntPtr.Size;

        public bool IsPtr = true;

        public TArray<T> SetIsPtr(bool value)
        {
            IsPtr = value;
            return this;
        }

        public override int ObjectSize => ElementSize + 8;
    }
}
