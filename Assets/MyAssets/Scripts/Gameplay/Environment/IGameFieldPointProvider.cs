using UnityEngine;

namespace Zoo.Gameplay.Environment
{
    public interface IGameFieldPointProvider
    {
        float GroundHeight { get; }
        Vector3 GetPoint();
        Rect GetFieldRect();
    }
}