using Reality.ModLoader;
using Reality.ModLoader.Unreal.Core;
using System;

namespace NeoPolaris.Unreal.Classes
{
    internal class USkinnedMeshComponent : UMeshComponent
    {
        public void SetSkeletalMesh(USkeletalMesh newMesh, bool bReinitPose)
        {
            var func = FindObject("Function /Script/Engine.SkinnedMeshComponent.SetSkeletalMesh");
            var ptr = FMemory.Malloc(IntPtr.Size + 1, 0);
            Memory.WriteIntPtr(ptr, 0, newMesh.BaseAddress);
            Memory.WriteUInt8(ptr, 8, bReinitPose ? (byte) 1 : (byte) 0);
            Loader.ProcessEvent(BaseAddress, func.BaseAddress, ptr);
        }
    }
}
