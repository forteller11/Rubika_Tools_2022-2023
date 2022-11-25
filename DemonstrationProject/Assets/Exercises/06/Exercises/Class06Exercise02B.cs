using UnityEditor;
using UnityEngine;

public class Class06Exercise02B : MonoBehaviour
{
    public Vector3 StretchPerClick = Vector3.one;
    public Transform Target;
}

[CustomEditor(typeof(Class06Exercise02B))]
public class Class06Exercise02BEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var e1 = (Class06Exercise02B) target;
        if (GUILayout.Button("Scale by: " + e1.StretchPerClick))
        {
            var newScale = e1.transform.localScale;
            newScale.Scale(e1.StretchPerClick);
            e1.transform.localScale = newScale;
        }
        
        if (e1.Target != null && GUILayout.Button("Calculate necessary stretch to acquire target scaling"))
        {
            Vector3 tar = e1.Target.localScale;
            Vector3 scl = e1.transform.localScale;
            //there is no Vector3.InverseScale function so we have to do the component-wise math manually
            e1.StretchPerClick = new Vector3(tar.x / scl.x, tar.y / scl.y, tar.z / scl.z);
        }
    }
}