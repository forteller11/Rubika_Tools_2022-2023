using UnityEditor;
using UnityEngine;

public class Class08Exercise03 : EditorWindow
{
    const string guidToFind = "9d0d902f71acfe646a46cb625f815a49";
    [MenuItem("class08/Find Object with Guid \"" + guidToFind + "\"")]
    static void FindAndPingTheObject()
    {
        string path = AssetDatabase.GUIDToAssetPath(guidToFind);
        if (path != null)
        {
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            EditorGUIUtility.PingObject(asset); //focus in on the asset in the editor
        }
        else
        {
            Debug.LogWarning($"Could not find asset with guid {guidToFind}... make sure the associated .meta file is not missing");
        }
    }
}