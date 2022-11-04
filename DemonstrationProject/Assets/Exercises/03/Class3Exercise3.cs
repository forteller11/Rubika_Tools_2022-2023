using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Class3Exercise3 : MonoBehaviour
{
    public Color Color;
}

//only try compile the editor if the unity editor is present, otherwise builds will fail
#if UNITY_EDITOR
[CustomEditor(typeof(Class3Exercise3))]
public class Class3Exercise1Editor : Editor
{
    private void OnSceneGUI()
    {
        //cast(/convert) target into a Class3Exercise3 object so we can access it's unique data (Color)
        Class3Exercise3 targetCasted = (Class3Exercise3) target;
        
        //make all compatible handles draw with the targetCasted color
        Handles.color = targetCasted.Color;
        //convert the color to a position (Vector3) so we can later pass it into the various Handle methods
        Vector3 colorAsPosition = new Vector3(targetCasted.Color.r, targetCasted.Color.g, targetCasted.Color.b);
        
        //we get the forward vector editor/scene camera so we can make the solidDisc always face us in the editor
        Vector3 sceneCameraForwardDirection = SceneView.currentDrawingSceneView.camera.transform.forward;
        float radius = 0.03f;
        Handles.DrawSolidDisc(colorAsPosition, sceneCameraForwardDirection, radius);
        
        Handles.DrawWireCube(Vector3.one/2, Vector3.one);

        EditorGUI.BeginChangeCheck();
        //this method both draws the position handles (3 draggable arrows) and allows the user to modify it by dragging
        colorAsPosition = Handles.PositionHandle(colorAsPosition, Quaternion.identity);
        
        //convert the colorAsPosition back to a Color struct and save it back
        //make sure the values are clamped between 0 and 1
        targetCasted.Color = new Color(Mathf.Clamp01(colorAsPosition.x), Mathf.Clamp01(colorAsPosition.y), Mathf.Clamp01(colorAsPosition.z));
        
        //if the user has changed the positionHandle this update tick...
        if (EditorGUI.EndChangeCheck())
        {
            //we do this so the user can undo (ctrl-z) the changes the make via the position handle
            Undo.RecordObject(targetCasted, "Change Color of Object");
        }
    }
}
#endif
