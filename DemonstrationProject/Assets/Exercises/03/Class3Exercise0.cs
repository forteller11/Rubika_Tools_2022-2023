using UnityEngine;

public class Class3Exercise0 : MonoBehaviour
{
    private void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = Camera.main.transform.position;
        Color lineColor = Color.green;
        Debug.DrawLine(start, end, lineColor);
    }
}
