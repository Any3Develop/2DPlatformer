using System;
using UnityEngine;

namespace Surfaces
{
    public class UniversalSurface : Surface
    {
        [SerializeField] private Collider2D surfaceCollider;
        [SerializeField] private GravityByAngle[] mapping = Array.Empty<GravityByAngle>(); // we can use GravityProfile here instead for more flexibility

        private CircleCollider2D _pointCollider;
        private Transform _selfTransform;
        private Transform _pointTransform;

        protected override void Awake()
        {
            base.Awake();
            if (mapping is null || mapping.Length == 0)
                mapping = new GravityByAngle[]{new (){angleStart = 0, angleEnd = 360, gravityForce = Physics2D.gravity.y}};
            
            _selfTransform = transform;
            var helper = new GameObject("GravityPoint");
            _pointTransform = helper.transform;
            _pointTransform.SetParent(_selfTransform);
            _pointTransform.localPosition = Vector3.zero;

            _pointCollider = helper.AddComponent<CircleCollider2D>();
            _pointCollider.radius = 0.001f;
            _pointCollider.isTrigger = true;
            _pointCollider.excludeLayers = -1;
            _pointCollider.includeLayers = ~0;
        }

        public override void GetGravityModifier(Vector2 worldPoint, ref float force, ref Vector2 direction)
        {
            _pointTransform.position = worldPoint;
            direction = surfaceCollider.Distance(_pointCollider).normal;
            
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) 
                angle += 360f;
            
            foreach (var area in mapping)
            {
                if (angle >= area.angleStart && angle <= area.angleEnd)
                {
                    force = area.gravityForce;
                    break;
                }
            }
        }
    }
}