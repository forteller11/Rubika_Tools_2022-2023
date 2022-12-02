using System;
using UnityEngine;

namespace Exercises._07
{
    public class Class07Exercise04 : MonoBehaviour
    {
        public Vector3 EulerAngles;

        private void OnValidate()
        {
            Quaternion xRotation = Quaternion.AngleAxis(EulerAngles.x, Vector3.right);
            Quaternion yRotation = Quaternion.AngleAxis(EulerAngles.y, Vector3.up);
            Quaternion zRotation = Quaternion.AngleAxis(EulerAngles.z, Vector3.forward);

            Quaternion eulerRotation = yRotation * xRotation * zRotation;
            transform.rotation = eulerRotation;
        }
    }
}