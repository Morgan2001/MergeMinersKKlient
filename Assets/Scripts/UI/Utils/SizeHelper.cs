using UI.GameplayPanel.MergePanel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Utils
{
    public static class SizeHelper
    {
        public static float CalculateCellSize(GridLayoutGroup grid, Rect rect, FieldSize size)
        {
            var spacing = grid.spacing.x;
            var cellSize = rect.width / size.Width - spacing;
            var mergeFieldHeight = rect.height;
            if ((cellSize + spacing) * size.Height > mergeFieldHeight)
            {
                cellSize = mergeFieldHeight / size.Height - spacing;
            }
            return cellSize;
        }
    }
}