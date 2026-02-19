using UnityEngine;

namespace Zoo.Gameplay.Environment
{
    internal sealed class ViewCameraBoundsField : MonoBehaviour, IViewBoundsField
    {
        [SerializeField] private Camera _camera;
        [SerializeField, Range(0f, 0.49f)] private float _safeViewportPadding = 0.02f;
        
        public bool IsInBounds(Vector3 point)
        {
            if (_camera == null)
            {
                return true;
            }

            Vector3 viewport = _camera.WorldToViewportPoint(point);
            if (viewport.z <= 0f)
            {
                return false;
            }

            float min = _safeViewportPadding;
            float max = 1f - _safeViewportPadding;
            return viewport.x >= min && viewport.x <= max &&
                   viewport.y >= min && viewport.y <= max;
        }

        public Vector3 GetInBoundsPoint(float height, Rect limits)
        {
            if (_camera == null)
            {
                return ClampToLimits(new Vector3(limits.center.x, height, limits.center.y), height, limits);
            }

            float min = _safeViewportPadding;
            float max = 1f - _safeViewportPadding;
            float viewportX = Random.Range(min, max);
            float viewportY = Random.Range(min, max);

            Plane plane = new Plane(Vector3.up, new Vector3(0f, height, 0f));
            Ray ray = _camera.ViewportPointToRay(new Vector3(viewportX, viewportY, 0f));

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                return ClampToLimits(point, height, limits);
            }

            Vector3 fallback = _camera.transform.position + _camera.transform.forward * 5f;
            return ClampToLimits(fallback, height, limits);
        }

        private static Vector3 ClampToLimits(Vector3 point, float height, Rect limits)
        {
            float minX = Mathf.Min(limits.xMin, limits.xMax);
            float maxX = Mathf.Max(limits.xMin, limits.xMax);
            float minZ = Mathf.Min(limits.yMin, limits.yMax);
            float maxZ = Mathf.Max(limits.yMin, limits.yMax);

            return new Vector3(
                Mathf.Clamp(point.x, minX, maxX),
                height,
                Mathf.Clamp(point.z, minZ, maxZ));
        }
    }
}
