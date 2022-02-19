﻿using System;
using System.Runtime.InteropServices;

namespace NeoPolaris.Unreal.Classes
{
    internal class USkinnedMeshComponent : UMeshComponent
    {
        public void SetSkeletalMesh(USkeletalMesh newMesh, bool bReinitPose)
        {
            var func = FindObject("Function /Script/Engine.SkinnedMeshComponent.SetSkeletalMesh");
            var ptr = Marshal.AllocHGlobal(IntPtr.Size + 1);
            Memory.WriteIntPtr(ptr, 0, newMesh.BaseAddress);
            Memory.WriteUInt8(ptr, 8, bReinitPose ? (byte) 1 : (byte) 0);
            App.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
        }
    }
}
