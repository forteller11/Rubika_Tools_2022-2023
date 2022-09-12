using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Charly.PursuitFasterCircles.OOP
{
    public class Circle1 : MonoBehaviour
    {
        private Vector2 _velocity;
        private float _radius;
        private List<Circle1> _allCircles;
        private SimSettings _settings;

        public void Init(SimSettings settings, List<Circle1> allCircles)
        {
            _settings = settings;
            _allCircles = allCircles;

            float initialVelocityMagnitude = Random.Range(_settings.InitVelocityRange.x, _settings.InitVelocityRange.y);
            _velocity = Random.insideUnitCircle * initialVelocityMagnitude;
            
            Set3DPosition(new Vector2(
                Random.Range(_settings.BoundsX.x, _settings.BoundsX.y),
                Random.Range(_settings.BoundsY.x, _settings.BoundsY.y)));
            
            _radius = Random.Range(_settings.InitRadiusRange.x, _settings.InitRadiusRange.y);
            
            transform.localScale = new Vector3(_radius, _radius, _radius) * 2;
        }

        private void Update()
        {
            var newPos = Get2DPosition() + _velocity * Time.deltaTime;
            Set3DPosition(newPos);
        }

        private void LateUpdate()
        {
            foreach (var circle in _allCircles)
            {
                if (circle == this)
                    continue;
                
                var distance = Vector2.Distance(this.Get2DPosition(), circle.Get2DPosition());
                var overlapping = distance - this._radius - circle.GetRadius() <= 0;
                if (overlapping)
                {
                    Debug.Log(distance - _radius - circle.GetRadius());
                    Debug.Log($"overlapping at {name} with {circle.name}");
                }
            }
        }

        public Vector2 Get2DPosition()
        {
            return new Vector2(transform.position.x, transform.position.y);
        }
        
        public void Set3DPosition(Vector2 pos)
        {
            transform.position = new Vector3(pos.x, pos.y);
        }

        public float GetRadius()
        {
            return _radius;
        }
    }
}