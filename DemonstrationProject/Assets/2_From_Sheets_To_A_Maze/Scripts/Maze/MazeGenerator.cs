using System;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.SheetsToMaze
{
    public class MazeGenerator
    {
        private static float2 cellSize = new float2(1, 1);
        public static GameObject Generate(SymbolAssetLink link, MazeFileDeserialized file)
        {
            var root = new GameObject("mazes");
            foreach (var maze in file.Mazes)
            {
                var mazeParent = new GameObject($"Maze: {maze.Name}");
                mazeParent.transform.SetParent(root.transform);
                
                for (int i = 0; i < maze.Grid.Dimensions.x; i++)
                {
                    for (int j = 0; j < maze.Grid.Dimensions.y; j++)
                    {
                        var cell = maze.Grid.GetSymbol(i, j);
                        var name = file.GetNameFromSymbol(cell);
                        if (name == null)
                        {
                            Debug.LogWarning($"Could not find {cell}'s name");
                            continue;
                        }
                        var asset = link.VisualFromName(name);

                        if (asset != null)
                        {
                            var position = new Vector3(i * cellSize.x, 0, j * cellSize.y);
                            var newCell = GameObject.Instantiate(asset, mazeParent.transform);
                            newCell.transform.position = position;
                        }
                    } 
                }
            }

            return root;
        }
    }
}