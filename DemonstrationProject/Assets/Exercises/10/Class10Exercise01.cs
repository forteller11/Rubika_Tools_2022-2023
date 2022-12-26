using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Class10Exercise01 : EditorWindow
{
    private Texture2D texture;
    const int width = 512;
    const int height = 512;
    
    [MenuItem("Class10/"+nameof(Class10Exercise01))]
    static void Init()
    {
        CreateWindow<Class10Exercise01>().Show();
    }
    private void OnGUI()
    {
        if (texture != null)
        {
            float aspect = (float) width / height;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.width * aspect), texture);
        }

        if (GUILayout.Button("Random"))
        {
            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var colors = new Color[width * height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Random.ColorHSV();
            }
            texture.SetPixels(colors);
            texture.Apply();
            
        }
    }
}
