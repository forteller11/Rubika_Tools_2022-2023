using UnityEditor;
using UnityEngine;

public class Class06Exercise01A : MonoBehaviour
{
    public Vector3 IncrementPerClick;
}

[CustomEditor(typeof(Class06Exercise01A))]
public class Class06Exercise01AEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var e1 = (Class06Exercise01A)target;
        if (GUILayout.Button("Move By Increment: " + e1.IncrementPerClick))
        {
            e1.transform.position += e1.IncrementPerClick;
        }
    }

    public void OnSceneGUI()
    {
        var e1 = (Class06Exercise01A)target;

        Vector3 center = e1.transform.position + e1.IncrementPerClick;
        Vector3 size = Vector3.Scale(Vector3.one, e1.transform.lossyScale);
        Handles.DrawWireCube(center, size);
    }
}
