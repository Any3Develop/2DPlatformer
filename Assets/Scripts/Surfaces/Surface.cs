using UnityEngine;

namespace Surfaces
{
    public abstract class Surface : MonoBehaviour
    {
        public GameObject Container { get; private set; }

        protected virtual void Awake()
        {
            Container = gameObject;
        }

        public abstract void GetGravityModifier(Vector2 worldPoint, ref float force, ref Vector2 direction);
    }
}