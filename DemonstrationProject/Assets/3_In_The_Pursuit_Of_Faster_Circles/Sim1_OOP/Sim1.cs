using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Charly.PursuitFasterCircles.OOP
{
    //not only are things not buffered, but state and logic is super mixed... so circles have references to ALL 
    
    //mean speed 37ms
    //median 35.5ms
    public class Sim1 : MonoBehaviour
    {
        public static readonly ProfilerMarker MarkerSetup = new ProfilerMarker("Charly.PursuitFasterCircles.Sim1.Setup");
        public static readonly ProfilerMarker MarkerUpdate = new ProfilerMarker("Charly.PursuitFasterCircles.Sim1.Update");

        public Circle1 Prefab;

        private List<Circle1> _circles;

        public void Begin(SimSettings settings)
        {
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

        public void End()
        {
            using (MarkerSetup.Auto())
            {
                foreach (var circle in _circles)
                    circle.DestroyAndCleanup();

                _circles.Clear(); //this should be redundant
            }
        }
    }
}

//todo
//oop:
//- wrapping
//- keeping number of circles always the same
//parition
//pool

//data oriented CPU
//cache friendly (structs)
//BURST compiled
//threaded

//gpu
//compute
//shader (compatabile on mobile and older systems)