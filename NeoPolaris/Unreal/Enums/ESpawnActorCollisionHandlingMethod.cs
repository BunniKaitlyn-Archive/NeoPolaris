namespace NeoPolaris.Unreal.Enums
{
    internal enum ESpawnActorCollisionHandlingMethod
    {
        Undefined = 0,
        AlwaysSpawn = 1,
        AdjustIfPossibleButAlwaysSpawn = 2,
        AdjustIfPossibleButDontSpawnIfColliding = 3,
        DontSpawnIfColliding = 4,
        ESpawnActorCollisionHandlingMethod_MAX = 5
    }
}
