using System.IO;
using UnityEngine;

namespace Charly.SheetsToMaze.Utils
{
    public class AssetDBUtils
    {
        public static string AbsoluteToRelativePath(string absPath)
        {
            int assetsLength = "Assets/".Length;
            string relativeTo = Application.dataPath.Remove(Application.dataPath.Length - assetsLength);
            string relativePath = Path.GetRelativePath(relativeTo, absPath);
            return relativePath;
        }
    }
}