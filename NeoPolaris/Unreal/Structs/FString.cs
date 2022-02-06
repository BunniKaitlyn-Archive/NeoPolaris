using NeoPolaris.Memory;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NeoPolaris.Unreal.Structs
{
    /// <summary>
    /// Represents a string in UE4, offsets should *usually* never change here.
    /// </summary>
    internal class FString : MemoryObject
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

        public string Value
        {
            get
            {
                var buffer = Memory.ReadBytes(Data, 0, Count * 2);
                return Encoding.Unicode.GetString(buffer).TrimEnd('\0');
            }
            set
            {
                var buffer = Encoding.Unicode.GetBytes(value + '\0');
                Data = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, Data, buffer.Length);
                Count = Max = value.Length + 1;
            }
        }

        public FString(IntPtr baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public FString(string value = "")
        {
            BaseAddress = Marshal.AllocHGlobal(ObjectSize);
            Value = value;
        }

        public override string ToString()
            => Value;

        public override int ObjectSize => IntPtr.Size + 8;
    }
}
