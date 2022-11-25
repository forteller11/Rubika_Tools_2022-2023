using UnityEditor;
using UnityEngine;

public class Class06Exercise02A : MonoBehaviour
{
    public Vector3 StretchPerClick = Vector3.one;
}

[CustomEditor(typeof(Class06Exercise02A))]
public class Class06Exercise02AEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var e1 = (Class06Exercise02A) target;
        if (GUILayout.Button("Scale by: " + e1.StretchPerClick))
        {
            var newScale = e1.transform.localScale;
            newScale.Scale(e1.StretchPerClick);
            e1.transform.localScale = newScale;
        }
    }
}