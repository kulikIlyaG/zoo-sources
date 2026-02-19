using UnityEngine;

namespace Zoo.Gameplay.Environment
{
    public interface IViewBoundsField
    {
        bool IsInBounds(Vector3 point);
        Vector3 GetInBoundsPoint(float height, Rect limits);
    }
}