using UnityEngine;
using UnityEngine.UIElements;

namespace Charly.SheetsToMaze
{
    public class ColorLabel : VisualElement
    {
        public ColorLabel(string label, Color color)
        {
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.alignItems = new StyleEnum<Align>(Align.FlexEnd);
            Add(new Label(label));
            var colorEl = new VisualElement();
            colorEl.style.backgroundColor = color;
            colorEl.AddToClassList("small-color-label");
            Add(colorEl);
        }
    }
}