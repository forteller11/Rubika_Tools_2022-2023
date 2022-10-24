using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace Charly.PursuitFasterCircles.OOP
{
    //not only are things not buffered, but state and logic is super mixed... so circles have references to ALL 
    
    //mean speed 37ms
    //median 35.5ms
    public class Sim1 : MonoBehaviour
    {
        public static readonly ProfilerMarker MarkerSetup = new ProfilerMarker("Charly.PursuitFasterCircles.Sim1.Setup");
        public static readonly ProfilerMarker MarkerUpdate = new ProfilerMarker("Charly.PursuitFasterCircles.Sim1.Update");
        
        public SimSettingsSO SimSettingsSo;
        public Circle1 Prefab;
        private List<Circle1> _circles;

        void OnEnable()
        {
            var settings = SimSettingsSo.Settings;
            using (MarkerSetup.Auto())
            {
                _circles = new List<Circle1>();
                for (int i = 0; i < settings.SpawnNumber; i++)
                {
                    var newCircle = Instantiate(Prefab);
                    newCircle.name = $"OOP Circle [{i}]";

                    newCircle.Init(settings, _circles);
                }
            }
        }

        void OnDisable()
        {
            using (MarkerSetup.Auto())
            {
                for (int i = _circles.Count -1; i > 0; i--)
                    _circles[i].DestroyAndCleanup();

                _circles.Clear(); //this should be redundant
            }
        }
    }
}