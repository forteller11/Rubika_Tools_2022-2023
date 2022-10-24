using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Pool;

namespace Charly.PursuitFasterCircles.Sim2
{
    //todo gameobject pool + speed up structure
    public class Sim2 : MonoBehaviour
    {
        public static readonly ProfilerMarker MarkerSetup = new ProfilerMarker("Charly.PursuitFasterCircles.Sim1.Setup");
        public static readonly ProfilerMarker MarkerUpdate = new ProfilerMarker("Charly.PursuitFasterCircles.Sim1.Update");

        public SimSettingsSO SimSettingsSo;
        public Circle2 Prefab;

        private List<Circle2> _circles;

        public static ObjectPool<Circle2> _circlePool;

        void OnEnable()
        {
            var settings = SimSettingsSo.Settings;
            _circlePool = new ObjectPool<Circle2>(CreateCircle, defaultCapacity: settings.SpawnNumber);
            using (MarkerSetup.Auto())
            {
                _circles = new List<Circle2>();
                for (int i = 0; i < settings.SpawnNumber; i++)
                {
                    var newCircle = _circlePool.Get();
                    newCircle.Init(settings, _circles);
                }
            }
        }

        void OnDisable()
        {
            using (MarkerSetup.Auto())
            {
                foreach (var circle in _circles)
                    circle.DestroyAndCleanup();

                _circles.Clear(); //this should be redundant
            }
        }

        private Circle2 CreateCircle()
        {
            var newCircle = Instantiate(Prefab);
            newCircle.name = $"OOP Circle [{_circlePool.CountAll}]";
            return newCircle;
        }
        
        
    }
}

//parition
//pool

//data oriented CPU
//cache friendly (structs)
//BURST compiled
//threaded

//gpu
//compute
//shader (compatabile on mobile and older systems)