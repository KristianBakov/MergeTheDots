using System;
using Data;

namespace Utils
{
    public class GridUtils : MonoSingleton<GridUtils>
    {
        public static bool CheckDotIsNeighbour(int originalDotPos, int neighbourDotPos)
        {
            int gridSize = DataConstants.Instance.GridSize;
            int originalRow = originalDotPos / gridSize;
            int originalCol = originalDotPos % gridSize;
            int neighbourRow = neighbourDotPos / gridSize;
            int neighbourCol = neighbourDotPos % gridSize;

            bool isSameRow = originalRow == neighbourRow;
            bool isAdjacentRow = Math.Abs(originalRow - neighbourRow) == 1;
            bool isSameOrAdjacentColumn = Math.Abs(originalCol - neighbourCol) == 1 || originalCol == neighbourCol;

            bool isNeighbour = false;

            if ((originalDotPos - gridSize == neighbourDotPos || originalDotPos + gridSize == neighbourDotPos) && isSameOrAdjacentColumn)
            {
                // Directly above or below, within the same or adjacent column (for vertical neighbors)
                isNeighbour = true;
            }
            else if ((originalDotPos - 1 == neighbourDotPos || originalDotPos + 1 == neighbourDotPos) && isSameRow)
            {
                // Left or right, must be in the same row
                isNeighbour = true;
            }
            else if (isAdjacentRow && isSameOrAdjacentColumn && 
                     (originalDotPos - gridSize - 1 == neighbourDotPos || 
                      originalDotPos - gridSize + 1 == neighbourDotPos || 
                      originalDotPos + gridSize - 1 == neighbourDotPos || 
                      originalDotPos + gridSize + 1 == neighbourDotPos))
            {
                // Diagonal neighbors, must be in adjacent rows and columns
                isNeighbour = true;
            }

            
            //check onthe grid up, left right, down and diagonals
            return isNeighbour;
        }
    }
}
