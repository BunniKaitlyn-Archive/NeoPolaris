﻿using System;
using System.Runtime.InteropServices;
using static NeoPolaris.Utilities.Win32;

namespace NeoPolaris.Utilities
{
    internal static class MinHook
    {
        private static IntPtr _handle;

        public static IntPtr MH_ALL_HOOKS = IntPtr.Zero;
        public enum MH_STATUS
        {
            MH_UNKNOWN = -1,
            MH_OK = 0,
            MH_ERROR_ALREADY_INITIALIZED,
            MH_ERROR_NOT_INITIALIZED,
            MH_ERROR_ALREADY_CREATED,
            MH_ERROR_NOT_CREATED,
            MH_ERROR_ENABLED,
            MH_ERROR_DISABLED,
            MH_ERROR_NOT_EXECUTABLE,
            MH_ERROR_UNSUPPORTED_FUNCTION,
            MH_ERROR_MEMORY_ALLOC,
            MH_ERROR_MEMORY_PROTECT,
            MH_ERROR_MODULE_NOT_FOUND,
            MH_ERROR_FUNCTION_NOT_FOUND
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate MH_STATUS MH_InitializeType();
        public static MH_InitializeType MH_Initialize;

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate MH_STATUS MH_CreateHookType(IntPtr pTarget, IntPtr pDetour, out IntPtr ppOriginal);
        public static MH_CreateHookType MH_CreateHook;

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate MH_STATUS MH_EnableHookType(IntPtr pTarget);
        public static MH_EnableHookType MH_EnableHook;

        static MinHook()
        {
            _handle = LoadLibraryFromResource($"MinHook.{(IntPtr.Size == 4 ? "x86" : "x64")}.dll");
            if (_handle == IntPtr.Zero)
                throw new Exception("Failed to load MinHook native.");

            MH_Initialize = GetExport<MH_InitializeType>(_handle, "MH_Initialize");
            MH_CreateHook = GetExport<MH_CreateHookType>(_handle, "MH_CreateHook");
            MH_EnableHook = GetExport<MH_EnableHookType>(_handle, "MH_EnableHook");

            var status = MH_Initialize();
            if (status is not MH_STATUS.MH_OK)
                throw new Exception($"Failed to initalize MinHook native. Status = {status}");
        }

        public static MH_STATUS CreateHook<T>(IntPtr target, T detour, out T trampoline) where T : Delegate
        {
            DelegateStore.Add(detour);

            var status = MH_CreateHook(target, Marshal.GetFunctionPointerForDelegate(detour), out var original);
            if (status is not MH_STATUS.MH_OK)
            {
                trampoline = null;
                return status;
            }

            trampoline = Marshal.GetDelegateForFunctionPointer<T>(original);
            DelegateStore.Add(trampoline);

            return status;
        }

        public static MH_STATUS EnableHook(IntPtr target)
            => MH_EnableHook(target);
        public static MH_STATUS EnableAllHooks()
            => MH_EnableHook(MH_ALL_HOOKS);
    }
}
