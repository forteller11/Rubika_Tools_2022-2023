using System;
using System.Collections;
using System.Collections.Generic;
using Charly.PursuitFasterCircles.OOP;
using UnityEngine;

namespace Charly.PursuitFasterCircles
{
    //sim where circles bump into eachother and then the smaller one explodes into numerous smaller circles...
    public class CirclesProfiler : MonoBehaviour
    {
        public SimSettings Settings;
        public Sim1 Sim1;

        private void OnEnable()
        {
            Sim1.Begin(Settings);
        }

        private void OnDisable()
        {
            Sim1.End();
        }
    }
}

