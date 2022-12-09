using UnityEditor;
using UnityEngine;

public class Class08Exercise02 : EditorWindow
{
    [MenuItem("class08/"+nameof(Class08Exercise02))]

    static void Init()
    {
        var window = CreateWindow<Class08Exercise02>();
        window.Show();
        window.titleContent = new GUIContent("Determine Path of Asset");
    }

    private Object asset;
    private void OnGUI()
    {
        asset = EditorGUILayout.ObjectField(asset, typeof(Object), false);
        string path = AssetDatabase.GetAssetPath(asset);

        GUI.enabled = false;
        EditorGUILayout.LabelField(path);
        GUI.enabled = true;
    }
}