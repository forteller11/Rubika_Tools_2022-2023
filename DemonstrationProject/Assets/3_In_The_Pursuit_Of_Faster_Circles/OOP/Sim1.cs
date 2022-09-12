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
                _circles.Add(newCircle);
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
//make more oop (all logic in circle)
//non oop + parition
//data oriented (stack)
//jobified
//threaded