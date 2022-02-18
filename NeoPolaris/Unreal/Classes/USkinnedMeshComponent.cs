﻿using NeoPolaris.Utilities;
using System.Runtime.InteropServices;

namespace NeoPolaris.Unreal.Classes
{
    public class USkinnedMeshComponent : UMeshComponent
    {
        public void SetSkeletalMesh(USkeletalMesh newMesh, bool bReinitPose)
        {
            var func = GetFunction("Function /Script/Engine.SkinnedMeshComponent.SetSkeletalMesh");
            var ptr = Marshal.AllocHGlobal(0x50);
            Win32.RtlFillMemory(ptr, 0x50, 0);
            Memory.WriteIntPtr(ptr, 0, newMesh.BaseAddress);
            Memory.WriteUInt8(ptr, 8, bReinitPose ? (byte) 1 : (byte) 0);
            App.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
        }
    }
}
