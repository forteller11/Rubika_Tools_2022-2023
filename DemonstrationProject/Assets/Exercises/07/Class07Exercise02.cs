using System;
using UnityEngine;

public class Class07Exercise02 : MonoBehaviour
{
    public float time = 2;
    
    private void Update()
    {
        time += Time.deltaTime;
        
        float x = Mathf.Cos(time);
        float y = Mathf.Sin(time);
        float z = Mathf.Cos(time/2);
        
        Vector3 xyz = new Vector3(x, y, z);
        Vector3 xyzRot = xyz * 180;
        Vector3 xyzScale = xyz + Vector3.one * 1.1F;
        Vector3 xyzPos = xyz;
        
        transform.rotation = Quaternion.Euler(xyzRot);
        transform.localScale = xyzScale;
        transform.localPosition = xyzPos;
    }
}