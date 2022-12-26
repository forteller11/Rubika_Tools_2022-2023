using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Class10Exercise02 : EditorWindow
{
    readonly Rect CURVE_BOUNDARY = new Rect(0, 0, 1, 1);
    
    private int size = 64;
    private AnimationCurve curveR = AnimationCurve.Linear(0,1,1,0);
    private AnimationCurve curveG = AnimationCurve.Linear(0,1,1,0);
    private AnimationCurve curveB = AnimationCurve.Linear(0,1,1,0);
    private string folderPath = "Assets/";
    private string textureName = "radius";
    
    [MenuItem("Class10/"+nameof(Class10Exercise02))]
    static void Init()
    {
        CreateWindow<Class10Exercise02>().Show();
    }

    private void OnGUI()
    {
        #region radius settings
        size = EditorGUILayout.IntField(new GUIContent("Diameter", "In Pixels"), size);
        const int curveFieldMinHeight = 32;
        curveR = EditorGUILayout.CurveField("Red", curveR, Color.red, CURVE_BOUNDARY, GUILayout.MinHeight(curveFieldMinHeight));
        curveG = EditorGUILayout.CurveField("Green", curveG, Color.green, CURVE_BOUNDARY, GUILayout.MinHeight(curveFieldMinHeight));
        curveB = EditorGUILayout.CurveField("Blue", curveB, Color.blue, CURVE_BOUNDARY, GUILayout.MinHeight(curveFieldMinHeight));
        #endregion

        #region picking output paths
        EditorGUILayout.Space();
        textureName = EditorGUILayout.TextField($"File Name", textureName);
        if (GUILayout.Button("Pick Folder"))
        {
            string physicalPath = EditorUtility.OpenFolderPanel("Pick Folder", String.Empty, String.Empty);
            if (!string.IsNullOrWhiteSpace(physicalPath))
            {
                folderPath = FileUtil.GetProjectRelativePath(physicalPath) + Path.DirectorySeparatorChar;
            }
        }
        
        string outputPath = folderPath + textureName + ".asset";
        GUI.enabled = false;
        outputPath = EditorGUILayout.TextField($"Output Path", outputPath);
        GUI.enabled = true;
        #endregion

        #region generating and saving the texture
        EditorGUILayout.Space();
        if (!GUILayout.Button("Create Radial Texture")) 
            return;
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, true);
        var pixels = new Color[size * size];
        float halfSize = size / 2f;

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            var pixelDistanceFromCenter = Vector2.Distance(
                new Vector2(x,y), 
                new Vector2(halfSize, halfSize));
            float pixelDistanceFromCenterNormalized = Mathf.Clamp01(pixelDistanceFromCenter / halfSize);
                
            float r = curveR.Evaluate(pixelDistanceFromCenterNormalized);
            float g = curveG.Evaluate(pixelDistanceFromCenterNormalized);
            float b = curveB.Evaluate(pixelDistanceFromCenterNormalized);
            var color = new Color(r, g, b);
                
            int flatIndex = y * size + x;
            pixels[flatIndex] = color;
        }
        texture.SetPixels(pixels);
        texture.Apply(true, true);

        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(outputPath);
        AssetDatabase.CreateAsset(texture, uniquePath);
        
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Texture2D>(uniquePath));
        #endregion
    }
}
