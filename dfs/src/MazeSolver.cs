using System.Collections.Generic;

namespace DFS
{
    /// <summary>
    /// 迷宫求解器，作为DFS算法的一个实际应用示例。
    /// </summary>
    public class MazeSolver
    {
        private readonly int[,] _maze; // 迷宫的二维数组表示 (0=通路, 1=墙)
        private readonly int _rows;
        private readonly int _cols;
        // 定义四个移动方向：上、下、左、右
        private readonly int[] _dx = { -1, 1, 0, 0 };
        private readonly int[] _dy = { 0, 0, -1, 1 };

        public MazeSolver(int[,] maze)
        {
            _maze = maze;
            _rows = maze.GetLength(0);
            _cols = maze.GetLength(1);
        }

        /// <summary>
        /// 求解从起点到终点的单条迷宫路径。
        /// </summary>
        /// <returns>如果找到路径，则返回坐标列表；否则返回空列表。</returns>
        public List<(int row, int col)> SolveMaze(int startRow, int startCol, int endRow, int endCol)
        {
            var visited = new bool[_rows, _cols];
            var path = new List<(int, int)>();

            if (SolveMazeHelper(startRow, startCol, endRow, endCol, visited, path))
            {
                return path;
            }

            return new List<(int, int)>(); // 未找到路径
        }

        /// <summary>
        /// 查找从起点到终点的所有迷宫路径。
        /// </summary>
        /// <returns>一个包含所有路径的列表，每个路径都是一个坐标列表。</returns>
        public List<List<(int row, int col)>> FindAllPaths(int startRow, int startCol, int endRow, int endCol)
        {
            var allPaths = new List<List<(int, int)>>();
            var visited = new bool[_rows, _cols];
            var currentPath = new List<(int, int)>();

            FindAllPathsHelper(startRow, startCol, endRow, endCol, visited, currentPath, allPaths);

            return allPaths;
        }

        /// <summary>
        /// 查找所有路径的递归辅助方法。
        /// </summary>
        private void FindAllPathsHelper(int currentRow, int currentCol, int endRow, int endCol,
            bool[,] visited, List<(int row, int col)> currentPath, List<List<(int, int)>> allPaths)
        {
            // 检查当前位置是否越界、是否为墙或是否已访问
            if (currentRow < 0 || currentRow >= _rows || currentCol < 0 || currentCol >= _cols ||
                _maze[currentRow, currentCol] == 1 || visited[currentRow, currentCol])
            {
                return;
            }

            // 将当前位置标记为已访问，并添加到路径中
            visited[currentRow, currentCol] = true;
            currentPath.Add((currentRow, currentCol));

            // 如果到达终点，则找到一条路径，将其副本添加到结果列表中
            if (currentRow == endRow && currentCol == endCol)
            {
                allPaths.Add(new List<(int, int)>(currentPath));
            }
            else
            {
                // 尝试向四个方向移动
                for (int i = 0; i < 4; i++)
                {
                    int nextRow = currentRow + _dx[i];
                    int nextCol = currentCol + _dy[i];
                    FindAllPathsHelper(nextRow, nextCol, endRow, endCol, visited, currentPath, allPaths);
                }
            }

            // 回溯：将当前位置移出路径，并取消标记，以便在其他分支中继续查找
            visited[currentRow, currentCol] = false;
            currentPath.RemoveAt(currentPath.Count - 1);
        }

        /// <summary>
        /// 求解迷宫的递归辅助方法。
        /// </summary>
        private bool SolveMazeHelper(int currentRow, int currentCol, int endRow, int endCol,
            bool[,] visited, List<(int row, int col)> path)
        {
            // 检查当前位置是否越界、是否为墙或是否已访问
            if (currentRow < 0 || currentRow >= _rows || currentCol < 0 || currentCol >= _cols ||
                _maze[currentRow, currentCol] == 1 || visited[currentRow, currentCol])
            {
                return false;
            }

            // 将当前位置标记为已访问，并添加到路径中
            visited[currentRow, currentCol] = true;
            path.Add((currentRow, currentCol));

            // 如果到达终点，则找到路径
            if (currentRow == endRow && currentCol == endCol)
            {
                return true;
            }

            // 尝试向四个方向移动
            for (int i = 0; i < 4; i++)
            {
                int nextRow = currentRow + _dx[i];
                int nextCol = currentCol + _dy[i];

                if (SolveMazeHelper(nextRow, nextCol, endRow, endCol, visited, path))
                {
                    return true;
                }
            }

            // 如果所有方向都无法到达终点，则回溯
            path.RemoveAt(path.Count - 1);
            return false;
        }
    }
}