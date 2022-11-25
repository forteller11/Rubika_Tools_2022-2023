using System;
using Unity.Collections;
using UnityEngine;

namespace Class06.InClassExamples
{
    public class CustomVectorClassExample : MonoBehaviour
    {
        public MyCustomVector myCustomVector;
        [TextArea] [ReadOnly] public string explanation = "This script uses a custom vector class and limits the x component to under 10";

        private void OnValidate()
        {
            if (myCustomVector.X > 10)
            {
                myCustomVector.X = 10;
            }
        }
    }

    [Serializable]
    public class MyCustomVector
    {
        public float X;
        public float y;
    }
}