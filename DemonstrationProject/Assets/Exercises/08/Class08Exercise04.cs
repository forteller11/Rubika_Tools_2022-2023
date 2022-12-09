using System.IO;
using UnityEditor;

public class Class08Exercise04 : EditorWindow
{
    [MenuItem("class08/"+"Create Default Folders")]
    static void CreateDefaultFolders()
    {
        CreateFolderUnderAssetsIfDoesntExist("Models");
        CreateFolderUnderAssetsIfDoesntExist("Textures");
        CreateFolderUnderAssetsIfDoesntExist("Scripts");
        CreateFolderUnderAssetsIfDoesntExist("Prefabs");
        CreateFolderUnderAssetsIfDoesntExist("Sounds");
    }

    static void CreateFolderUnderAssetsIfDoesntExist(string folderName)
    {
        //combine the folder names with whatever directory separator is used on the OS '/' for windows.
        string fullPathName = Path.Combine("Assets", folderName);
        //if folder doesnt already exist....
        if (!AssetDatabase.IsValidFolder(fullPathName))
        {
            //create a new folder
            AssetDatabase.CreateFolder("Assets", folderName);
        }
    }
}