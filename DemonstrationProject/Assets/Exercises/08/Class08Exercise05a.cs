using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Class08Exercise05a : EditorWindow
{
    private Object folder;
    private List<Texture2D> textures = new();
    private Vector2 scrollPosition;
    
    [MenuItem("class08/"+nameof(Class08Exercise05a))]
    static void Init()
    {
        var window = CreateWindow<Class08Exercise05a>();
        window.Show();
        window.titleContent = new GUIContent("Show Textures Under Folder");
    }
    
    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        folder = EditorGUILayout.ObjectField("Folder", folder, typeof(Object), false);
        if (EditorGUI.EndChangeCheck())
        {
            textures.Clear();
            string folderPath = AssetDatabase.GetAssetPath(folder);

            if (!AssetDatabase.IsValidFolder(folderPath))
                folder = null;

            if (folder == null)
                return;
            
            var guids = AssetDatabase.FindAssets("t:texture", new[] { folderPath });

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (texture != null)
                    textures.Add(texture);
            }
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUI.enabled = false;
        foreach (var texture in textures)
        {
            EditorGUILayout.ObjectField(texture, typeof(Texture2D), false);
        }
        GUI.enabled = true;
        GUILayout.EndScrollView();
    }
}