using UnityEditor;
using UnityEngine;

public class Class09Exercise03 : EditorWindow
{
        private string name = nameof(Class09Exercise03) + " mesh";
        private Object folder;

        [MenuItem("Class09/Exercise 03")]
        private static void Init()
        { 
                var window = GetWindow<Class09Exercise03>();
                window.name = "Save a Mesh";
                window.Show();
        }
        private void OnGUI()
        {
                var potentialFolder = EditorGUILayout.ObjectField("Folder", folder, typeof(Object), false);
                
                //make sure the Object is actually a folder
                string potentialFolderPath = AssetDatabase.GetAssetPath(potentialFolder);
                bool isFolder = AssetDatabase.IsValidFolder(potentialFolderPath);
                if (potentialFolder == null || isFolder)
                { 
                        folder = potentialFolder;
                }

                if (folder == null)
                        return;

                if (GUILayout.Button("Create Mesh"))
                {
                        var mesh = new Mesh();
                        mesh.SetVertices(new []
                        {
                                new Vector3(-1,-1,0), //bottom left
                                new Vector3(1,1,0), //upper right
                                new Vector3(1,-1,0), //bottom right
                                new Vector3(-1,1,0), //upper left
                        });
                        mesh.SetIndices(new [] { 
                                0,2,1,
                                0,1,3
                        }, 
                        MeshTopology.Triangles, 
                                0);
                        mesh.RecalculateNormals();
                        mesh.RecalculateBounds();
                        mesh.MarkModified();
                        
                        //save the asset
                        var uniqueMeshPath = AssetDatabase.GenerateUniqueAssetPath(potentialFolderPath + '/' + name + ".asset");
                        AssetDatabase.CreateAsset(mesh, uniqueMeshPath);
                        
                        //load it so we can ping it (highlight it in the inspector)
                        var assetMesh = AssetDatabase.LoadAssetAtPath<Object>(uniqueMeshPath);
                        EditorGUIUtility.PingObject(assetMesh);
                }
        }
}
