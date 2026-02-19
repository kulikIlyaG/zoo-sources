using UnityEngine;

namespace Zoo.Gameplay.Entities
{
    public interface ISpawnPositionProvider
    {
        Vector3 GetSpawnPosition();
    }

    internal sealed class SpawnPositionProviderZero : ISpawnPositionProvider
    {
        public Vector3 GetSpawnPosition()
        {
            return Vector3.zero;
        }
    }
}