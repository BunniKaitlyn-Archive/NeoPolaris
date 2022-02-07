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
        {
            ElementSize = IntPtr.Size;
        }

        public TArray(IntPtr baseAddress)
        {
            BaseAddress = baseAddress;
            ElementSize = IntPtr.Size;
        }

        public TArray(IntPtr baseAddress, int elementSize)
        {
            BaseAddress = baseAddress;
            ElementSize = elementSize;
        }

        public T GetElement(int index)
            => new() { BaseAddress = Memory.ReadIntPtr(Data, index * ElementSize) };

        public T this[int index] => GetElement(index);

        public T[] ToArray()
        {
            var elements = new List<T>();
            for (var i = 0; i < Count; i++)
                elements.Add(GetElement(i));
            return elements.ToArray();
        }

        public int ElementSize { get; }

        public override int ObjectSize => ElementSize + 8;
    }
}
