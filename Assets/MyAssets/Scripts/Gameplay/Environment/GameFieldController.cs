using UnityEngine;

namespace Zoo.Gameplay.Environment
{
    internal sealed class GameFieldController : MonoBehaviour, IGameFieldPointProvider
    {
        [SerializeField, Min(0f)] private float _width;
        [SerializeField, Min(0f)] private float _height;

        public float GroundHeight => transform.position.y;

        Vector3 IGameFieldPointProvider.GetPoint()
        {
            float halfWidth = _width * 0.5f;
            float halfHeight = _height * 0.5f;

            float x = Random.Range(transform.position.x - halfWidth, transform.position.x + halfWidth);
            float z = Random.Range(transform.position.z - halfHeight, transform.position.z + halfHeight);

            return new Vector3(x, GroundHeight, z);
        }

        public Rect GetFieldRect()
        {
            float halfWidth = _width * 0.5f;
            float halfHeight = _height * 0.5f;

            return Rect.MinMaxRect(
                transform.position.x - halfWidth,
                transform.position.z - halfHeight,
                transform.position.x + halfWidth,
                transform.position.z + halfHeight);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, new Vector3(_width, 0.1f, _height));
        }
    }
}
