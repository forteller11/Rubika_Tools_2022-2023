using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Class10Exercise03 : EditorWindow
{ 
        [MenuItem("Class10/"+nameof(Class10Exercise03))]
        static void Init()
        {
                GetWindow<Class10Exercise03>().Show();
        }

        private Texture2D texture;
        private Vector2Int gbOffset;
        private void OnGUI()
        {

                texture = EditorGUILayout.ObjectField(texture, typeof(Texture2D), false) as Texture2D;
                gbOffset = EditorGUILayout.Vector2IntField(nameof(gbOffset), gbOffset);

                if (texture == null)
                        return;

                if (GUILayout.Button("Chromatic Abberate"))
                {
                        var originalTexturePath = AssetDatabase.GetAssetPath(texture);
                        
                        var textureImporter = (TextureImporter) TextureImporter.GetAtPath(originalTexturePath);
                        textureImporter.isReadable = true;
                        var settings = textureImporter.GetDefaultPlatformTextureSettings();
                        settings.format = TextureImporterFormat.RGBA32;
                        textureImporter.SetPlatformTextureSettings(settings);
                        textureImporter.SaveAndReimport();

                        var newTex = Instantiate(texture);
                        var pixels = newTex.GetPixels();
                        var width = newTex.width;
                        var height = newTex.height;
                        
                        for (int y = 0; y < height; y++)
                        for (int x = 0; x < width; x++)
                        {
                                int baseIndex = ToFlatIndex(x, y, width, height);
                                int gIndex = ToFlatIndex(x+gbOffset.x, y, width, height);
                                int bIndex = ToFlatIndex(x, y+gbOffset.y, width, height);

                                Color basePixel = pixels[baseIndex];
                                float g = pixels[gIndex].g;
                                float b = pixels[bIndex].b;

                                pixels[baseIndex] = new Color(basePixel.r, g, b, basePixel.a);
                        }
                        
                        newTex.SetPixels(pixels);
                        newTex.Apply();
                        
                        string logicalPath = 
                                Path.GetDirectoryName(originalTexturePath) +
                                Path.DirectorySeparatorChar + 
                                Path.GetFileNameWithoutExtension(originalTexturePath) + 
                                "_chromatic.png";
                        string uniqueLogicalPah = AssetDatabase.GenerateUniqueAssetPath(logicalPath);
                        try
                        {
                                string physicalPath = FileUtil.GetPhysicalPath(uniqueLogicalPah);
                                var bytes = ImageConversion.EncodeToPNG(newTex);
                                File.WriteAllBytes(physicalPath, bytes);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                        }
                        catch (Exception e)
                        {
                                Debug.LogError(e.Message);
                                return;
                        }

                        var newPng = AssetDatabase.LoadAssetAtPath<Texture2D>(uniqueLogicalPah);
                        EditorGUIUtility.PingObject(newPng);
                }

                static int ToFlatIndex(int x, int y, int width, int height)
                {
                        int xBounded = Mathf.Clamp(x, 0, width-1);
                        int yBounded = Mathf.Clamp(y, 0, height-1);
                        return xBounded + yBounded * width;
                }
        }
}
