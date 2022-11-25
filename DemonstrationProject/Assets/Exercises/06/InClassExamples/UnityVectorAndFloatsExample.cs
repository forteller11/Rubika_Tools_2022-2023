using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Class06.InClassExamples
{
    public class UnityVectorAndFloatsExample : MonoBehaviour
    {
        [Header("floats to a vector")] 
        public float xSource;
        public float ySource;
        public Vector2 VectorDestination1;
        public Vector2 VectorDestination2;

        [Header("vector to floats")] 
        public Vector2 VectorSource;
        public float xDestination;
        public float yDestination;

        //the OnValidate method is called when values change the inspector (editor)
        void OnValidate()
        {
            //floats to a vector method #1
            //modify the x and y of the vector to be xSource and ySource
            VectorDestination1.x = xSource;
            VectorDestination1.y = ySource;

            //floats to a vector method #2
            //create a NEW vector with xSource and ySource as components
            //this "new" syntax is called a constructor
            VectorDestination2 = new Vector2(xSource, ySource);

            //Vector to floats
            xDestination = VectorSource.x;
            yDestination = VectorSource.y;
        }

    }
}