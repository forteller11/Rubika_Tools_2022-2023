using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

namespace Charly.PursuitFasterCircles.OOP
{
    public class Circle1 : MonoBehaviour
    {
        private Vector2 _velocity;
        private float _radius;
        private List<Circle1> _allCircles;
        private SimSettings _settings;

        public void Init(SimSettings settings, List<Circle1> allCircles, Circle1 parent=null)
        {
            _settings = settings;
            _allCircles = allCircles;
            _allCircles.Add(this);

            float initialVelocityMagnitude = Random.Range(_settings.InitVelocityRange.x, _settings.InitVelocityRange.y);
            _velocity = Random.insideUnitCircle * initialVelocityMagnitude;

            _radius = Random.Range(_settings.InitRadiusRange.x, _settings.InitRadiusRange.y);
            
            if (parent == null)
            {
                Set3DPosition(new Vector2(
                    Random.Range(_settings.BoundsMin.x, _settings.BoundsMax.x),
                    Random.Range(_settings.BoundsMin.y, _settings.BoundsMax.y)));
                _radius = Random.Range(_settings.InitRadiusRange.x, _settings.InitRadiusRange.y);
            }
            else
            {
                Set3DPosition(parent.Get2DPosition() + Random.insideUnitCircle * parent._radius);
                _radius = Random.Range(_settings.PercentageRadiusOfDebrisRange.x, _settings.PercentageRadiusOfDebrisRange.y) * parent.GetRadius();
            }
            
            transform.localScale = new Vector3(_radius, _radius, _radius) * 2;
            
            //destroy self if too small
            if (_radius < _settings.MinRadiusUntilDestruction)
                DestroyAndCleanup();
        }

        private void Update()
        {
            using (Sim1.MarkerUpdate.Auto())
            {
                //replace any destroyed circles
                while (_allCircles.Count < _settings.SpawnNumber)
                {
                    var newCircle = Instantiate(this);
                    newCircle.Init(_settings, _allCircles);
                }

                //integrate velocity
                var newPos = Get2DPosition() + _velocity * Time.deltaTime;

                //wrap
                if (newPos.x > _settings.BoundsMax.x) newPos.x = _settings.BoundsMin.x;
                else if (newPos.x < _settings.BoundsMin.x) newPos.x = _settings.BoundsMax.x;

                if (newPos.y > _settings.BoundsMax.y) newPos.y = _settings.BoundsMin.y;
                else if (newPos.y < _settings.BoundsMin.y) newPos.y = _settings.BoundsMax.y;

                Set3DPosition(newPos);
            }
        }

        private void LateUpdate()
        {
            using (Sim1.MarkerUpdate.Auto())
            {
                for (int i = 0; i < _allCircles.Count; i++)
                {
                    var circle = _allCircles[i];
                    if (circle == this)
                        continue;

                    var distance = Vector2.Distance(this.Get2DPosition(), circle.Get2DPosition());
                    var overlapping = distance - this._radius - circle.GetRadius() <= 0;

                    if (!overlapping)
                        continue;

                    int children = Random.Range(_settings.DebrisOnDestruction.x, _settings.DebrisOnDestruction.y + 1);
                    for (int j = 0; j < children; j++)
                    {
                        var child = Instantiate(this);
                        child.Init(_settings, _allCircles, this);
                        Set3DPosition(Get2DPosition());
                    }

                    DestroyAndCleanup();
                    break;
                }
            }
        }

        public void DestroyAndCleanup()
        {
            _allCircles.Remove(this);
            Destroy(this.gameObject);
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