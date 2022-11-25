using Unity.Collections;
using UnityEngine;

namespace Class06.InClassExamples
{
    public class UnityVectorIntAndIntsExample : MonoBehaviour
    {
        [Header("ints to a vector")] 
        public int xSource;
        public int ySource;
        public Vector2Int VectorDestination1;
        public Vector2Int VectorDestination2;

        [Header("vector to ints")] 
        public Vector2Int VectorSource;
        public int xDestination;
        public int yDestination;

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
            VectorDestination2 = new Vector2Int(xSource, ySource);

            //Vector to floats
            xDestination = VectorSource.x;
            yDestination = VectorSource.y;
        }
    }
}