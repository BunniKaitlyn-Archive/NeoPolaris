using NeoPolaris.Utilities;
using System;
using System.Runtime.InteropServices;
using static NeoPolaris.Utilities.Win32;

namespace NeoPolaris.Memory
{
    internal static class MemoryUtil
    {
        public static IntPtr FindPattern(string pattern, string mask)
        {
            var process = GetCurrentProcess();
            var handle = GetModuleHandle(null);

            GetModuleInformation(process, handle, out var info, (uint) Marshal.SizeOf<MODULEINFO>());

            var buffer = new byte[info.SizeOfImage];
            if (ReadProcessMemory(process, info.lpBaseOfDll, buffer, (int) info.SizeOfImage, out _))
            {
                for (var i = 0; i < info.SizeOfImage; i++)
                {
                    var found = true;

                    for (var j = 0; j < mask.Length; j++)
                    {
                        found = mask[j] == '?' || buffer[j + i] == pattern[j];
                        if (!found)
                            break;
                    }

                    if (found)
                        return IntPtr.Add(handle, i);
                }
            }

            return IntPtr.Zero;
        }

        public static IntPtr GetAddressFromOffset(IntPtr offset, int p0 = 5, int p1 = 1)
            => new(offset.ToInt64() + p0 + App.Instance.Memory.ReadInt32(offset, p1));

        public static T GetNativeFunc<T>(IntPtr target) where T : Delegate
        {
            var func = Marshal.GetDelegateForFunctionPointer<T>(target);
            DelegateStore.Add(func);
            return func;
        }

        public static T GetNativeFuncWithOffset<T>(IntPtr target) where T : Delegate
            => GetNativeFunc<T>(GetAddressFromOffset(target));
        public static T GetNativeFuncFromPattern<T>(string pattern, string mask) where T : Delegate
            => GetNativeFunc<T>(FindPattern(pattern, mask));
        public static T GetNativeFuncFromPatternWithOffset<T>(string pattern, string mask) where T : Delegate
            => GetNativeFuncWithOffset<T>(FindPattern(pattern, mask));
    }
}
