using UnityEngine;

public class Class06Exercise00 : MonoBehaviour
{
    public Vector2 mousePosition2D;
    public float mouseHorizontalPosition;
    public float mouseVerticalPosition;

    public void Update()
    {
        mousePosition2D = Input.mousePosition;
        mouseHorizontalPosition = mousePosition2D.x;
        mouseVerticalPosition = mousePosition2D.y;
    }
}
