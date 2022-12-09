using UnityEditor;
using UnityEngine;

public class Class08Exercise01 : EditorWindow
{
    [MenuItem("class08/"+nameof(Class08Exercise01))]

    static void Init()
    {
        var window = CreateWindow<Class08Exercise01>();
        window.Show();
        window.titleContent = new GUIContent("Find Asset At Path");
    }

    private string path = string.Empty;
    private void OnGUI()
    {
        path = EditorGUILayout.TextField(path);

        Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
        GUI.enabled = false;
        EditorGUILayout.ObjectField(asset, typeof(Object), false);
        GUI.enabled = true;
    }
}
