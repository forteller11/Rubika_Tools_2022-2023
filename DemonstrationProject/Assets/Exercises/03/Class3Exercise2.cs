using UnityEngine;

public class Class3Exercise2 : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        //global down will always be (0,-1,0)
        Vector3 globalDown = Vector3.down;
        //local down will change with the rotation of the object
        //we can get the local down by getting the local up and inverting it
        Vector3 localDown = -transform.up;
        
        //these methods are used to draw the gizmos
        //we create a custom method so we can reuse instead of copy-paste similar code
        DrawLineToIntersection(globalDown, new Color(0.08f, 0.17f, 0.76f, 1f));
        DrawLineToIntersection(localDown, new Color(0.62f, 0.16f, 0.76f, 1f));
    }
    
    private void DrawLineToIntersection(Vector3 rayDirection, Color color)
    {
        //the origin of the ray at the origin of this gameobject, pointing in the rayDirection
        var ray = new Ray(transform.position, rayDirection);
        //perform the raycast, put all the intersection information into the hitInfo variable
        bool hasHit = Physics.Raycast(ray, out RaycastHit hitInfo);
        //if the ray has hit something....
        if (hasHit)
        {
            //set the color
            Gizmos.color = color;
            //draw the line from the center of the object to the hit position
            Gizmos.DrawLine(transform.position, hitInfo.point);
            //draw a sphere at the hit position
            Gizmos.DrawSphere(hitInfo.point, 0.05f);
        }
    }
}
