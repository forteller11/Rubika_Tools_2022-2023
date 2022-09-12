using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Charly.PursuitFasterCircles.OOP
{
    
    //not only are things not buffered, but state and logic is super mixed... so circles have references to ALL 
    public class Sim1 : MonoBehaviour
    {
        public Circle1 Prefab;

        private List<Circle1> _circles;

        private bool _on;

        public void Begin(SimSettings settings)
        {
            _on = true;
            
            _circles = new List<Circle1>();
            for (int i = 0; i < settings.SpawnNumber; i++)
            {
                var newCircle = Instantiate(Prefab);
                newCircle.name = $"OOP Circle [{i}]";
 
                newCircle.Init(settings, _circles);
            }
        }

        public void End()
        {
            _on = false;
            foreach (var circle in _circles)
                Destroy(circle);

            _circles.Clear();
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