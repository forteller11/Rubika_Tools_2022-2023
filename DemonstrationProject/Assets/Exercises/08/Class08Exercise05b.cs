using UnityEditor;
using UnityEngine;

public class Class08Exercise05b : EditorWindow
{
    [MenuItem("class08/"+nameof(Class08Exercise05b))]

    static void Init()
    {
        var window = CreateWindow<Class08Exercise05b>();
        window.Show();
        window.titleContent = new GUIContent("Modify Texture Import Settings");
    }

    private Object folder;
    private void OnGUI()
    {
        folder = EditorGUILayout.ObjectField("Folder", folder, typeof(Object), false);
        string folderPath = AssetDatabase.GetAssetPath(folder);

        if (!AssetDatabase.IsValidFolder(folderPath))
            folder = null;

        if (folder == null)
            return;


        if (!GUILayout.Button("Reimport Textures"))
        {
            return;
        }
            
        var guids = AssetDatabase.FindAssets("t:texture", new[] { folderPath });

        int texturesModified = 0;
        for (var i = 0; i < guids.Length; i++)
        {
            var guid = guids[i];
            EditorUtility.DisplayProgressBar("reimporting textures", guid, ((float)i / guids.Length));
            
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
                continue;

            texturesModified++;
            importer.isReadable = false;
            importer.maxTextureSize = 64;
            importer.SaveAndReimport();
        }
        
        Debug.Log($"Reimported {texturesModified} textures");
        EditorUtility.ClearProgressBar();
    }
}
