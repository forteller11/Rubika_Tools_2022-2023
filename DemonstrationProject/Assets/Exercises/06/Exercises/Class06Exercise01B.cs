using UnityEditor;
using UnityEngine;

public class Class06Exercise01B : MonoBehaviour
{
    public Vector3 IncrementPerClick;
    public Transform Target;
}

[CustomEditor(typeof(Class06Exercise01B))]
public class Class06Exercise01BEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var e1 = (Class06Exercise01B) target;
        if (GUILayout.Button("Move By Increment: " + e1.IncrementPerClick))
        {
            e1.transform.position += e1.IncrementPerClick;
        }
        
        if (e1.Target != null && GUILayout.Button("Calculate necessary increment to advance to Target"))
        {
            //how much would we need to offset transform.position to reach target.position?
            //find the difference by subtracting one from the other
            e1.IncrementPerClick = e1.Target.position - e1.transform.position;
        }
    }

    public void OnSceneGUI()
    {
        var e1 = (Class06Exercise01B)target;

        Vector3 center = e1.transform.position + e1.IncrementPerClick;
        Vector3 size = Vector3.Scale(Vector3.one, e1.transform.lossyScale);
        Handles.DrawWireCube(center, size);
    }
}
