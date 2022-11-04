
using UnityEngine;

public class Class3Exercise1 : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (transform.childCount > 0 &&  //does this transform have any children?
            transform.localScale != Vector3.one) //does this transform scale itself (and its children)?
        {
            //set the color of the gizmos
            Gizmos.color = new Color(1f, 0.39f, 0.17f, 0.3f);
            
            //draw a wireframe cube at the position and scale of the transform
            Gizmos.DrawWireCube(transform.position, transform.localScale/3);
            
            //draw the icon at the path: Assets/Gizmos/circle.png (allows the user to select the object by clicking on the icon)
            Gizmos.DrawIcon(transform.position, "circle.png", false);
        }
    }
}
