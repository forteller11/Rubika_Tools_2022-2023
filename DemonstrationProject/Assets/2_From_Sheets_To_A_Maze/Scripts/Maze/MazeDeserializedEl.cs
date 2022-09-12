using UnityEngine;
using UnityEngine.UIElements;

namespace Charly.SheetsToMaze
{
    public class MazeFileDeserializedEl : VisualElement
    {
        private MazeFileDeserialized _maze;
        public MazeFileDeserializedEl(MazeFileDeserialized file)
        {
            _maze = file;
            
            Add(new ColorLabel("Primary", _maze.Primary));
            Add(new ColorLabel("Secondary", _maze.Secondary));
            
            var separator = new VisualElement();
            separator.AddToClassList("separator1");
            Add(separator);

            foreach (var maze in file.Mazes)
            {
                var gridEl = new VisualElement();
                Add(gridEl);
                var gridName = new Label(){text = maze.Name};
                gridName.AddToClassList("h3");
                gridEl.Add(gridName);

                for (int i = 0; i < maze.Grid.Dimensions.y; i++)
                {
                    var rowEl = new VisualElement();
                    rowEl.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                    const float size = 16;
                    rowEl.style.height = size;
                    
                    gridEl.Add(rowEl);
                    
                    int boundsL = i * maze.Grid.Dimensions.x;
                    var cells = maze.Grid.Symbols.GetRange(boundsL, maze.Grid.Dimensions.x);

                    foreach (var cell in cells)
                    {
                        var cellEl = new Label()
                        {
                            text = cell.ToString(), 
                            tooltip = file.GetNameFromSymbol(cell)
                        };

                        cellEl.style.height = size;
                        cellEl.style.width = size;
                        cellEl.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
                        rowEl.Add(cellEl);
                    }
                }
                
                var divider = new VisualElement();
                divider.AddToClassList("separator1");
                gridEl.Add(divider);  
            }
        }
    }
}