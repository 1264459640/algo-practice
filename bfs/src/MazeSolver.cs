using System.Collections.Generic;

namespace BFS
{
    /// <summary>
    /// 迷宫求解器，使用BFS算法找到最短路径。
    /// BFS保证找到的第一条路径就是最短路径。
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
        /// 使用BFS求解从起点到终点的最短迷宫路径。
        /// </summary>
        /// <returns>如果找到路径，则返回坐标列表；否则返回空列表。</returns>
        public List<(int row, int col)> SolveMaze(int startRow, int startCol, int endRow, int endCol)
        {
            var visited = new bool[_rows, _cols];
            var queue = new Queue<(int row, int col, List<(int, int)> path)>();
            
            // 起始位置
            var startPath = new List<(int, int)> { (startRow, startCol) };
            queue.Enqueue((startRow, startCol, startPath));
            visited[startRow, startCol] = true;
            
            while (queue.Count > 0)
            {
                var (currentRow, currentCol, path) = queue.Dequeue();
                
                // 如果到达终点，返回路径
                if (currentRow == endRow && currentCol == endCol)
                {
                    return path;
                }
                
                // 尝试向四个方向移动
                for (int i = 0; i < 4; i++)
                {
                    int nextRow = currentRow + _dx[i];
                    int nextCol = currentCol + _dy[i];
                    
                    // 检查新位置是否有效且未访问
                    if (IsValidPosition(nextRow, nextCol) && !visited[nextRow, nextCol])
                    {
                        visited[nextRow, nextCol] = true;
                        var newPath = new List<(int, int)>(path) { (nextRow, nextCol) };
                        queue.Enqueue((nextRow, nextCol, newPath));
                    }
                }
            }
            
            return new List<(int, int)>(); // 未找到路径
        }
        
        /// <summary>
        /// 使用BFS查找从起点到终点的所有最短路径。
        /// </summary>
        /// <returns>一个包含所有最短路径的列表。</returns>
        public List<List<(int row, int col)>> FindAllShortestPaths(int startRow, int startCol, int endRow, int endCol)
        {
            var allPaths = new List<List<(int, int)>>();
            var visited = new Dictionary<(int, int), int>(); // 存储到达每个位置的最短距离
            var queue = new Queue<(int row, int col, List<(int, int)> path, int distance)>();
            
            // 起始位置
            var startPath = new List<(int, int)> { (startRow, startCol) };
            queue.Enqueue((startRow, startCol, startPath, 0));
            visited[(startRow, startCol)] = 0;
            
            int shortestDistance = -1;
            
            while (queue.Count > 0)
            {
                var (currentRow, currentCol, path, distance) = queue.Dequeue();
                
                // 如果已经找到最短路径，且当前距离更长，则跳过
                if (shortestDistance != -1 && distance > shortestDistance)
                {
                    continue;
                }
                
                // 如果到达终点
                if (currentRow == endRow && currentCol == endCol)
                {
                    if (shortestDistance == -1)
                    {
                        shortestDistance = distance;
                    }
                    
                    if (distance == shortestDistance)
                    {
                        allPaths.Add(new List<(int, int)>(path));
                    }
                    continue;
                }
                
                // 尝试向四个方向移动
                for (int i = 0; i < 4; i++)
                {
                    int nextRow = currentRow + _dx[i];
                    int nextCol = currentCol + _dy[i];
                    int nextDistance = distance + 1;
                    
                    // 检查新位置是否有效
                    if (IsValidPosition(nextRow, nextCol))
                    {
                        var nextPos = (nextRow, nextCol);
                        
                        // 如果这个位置没有访问过，或者找到了更短的路径
                        if (!visited.ContainsKey(nextPos) || visited[nextPos] >= nextDistance)
                        {
                            visited[nextPos] = nextDistance;
                            var newPath = new List<(int, int)>(path) { nextPos };
                            queue.Enqueue((nextRow, nextCol, newPath, nextDistance));
                        }
                    }
                }
            }
            
            return allPaths;
        }
        
        /// <summary>
        /// 计算从起点到迷宫中所有可达位置的最短距离。
        /// </summary>
        /// <returns>一个字典，包含每个位置的最短距离。</returns>
        public Dictionary<(int row, int col), int> CalculateDistances(int startRow, int startCol)
        {
            var distances = new Dictionary<(int, int), int>();
            var visited = new bool[_rows, _cols];
            var queue = new Queue<(int row, int col, int distance)>();
            
            queue.Enqueue((startRow, startCol, 0));
            visited[startRow, startCol] = true;
            distances[(startRow, startCol)] = 0;
            
            while (queue.Count > 0)
            {
                var (currentRow, currentCol, distance) = queue.Dequeue();
                
                // 尝试向四个方向移动
                for (int i = 0; i < 4; i++)
                {
                    int nextRow = currentRow + _dx[i];
                    int nextCol = currentCol + _dy[i];
                    
                    if (IsValidPosition(nextRow, nextCol) && !visited[nextRow, nextCol])
                    {
                        visited[nextRow, nextCol] = true;
                        distances[(nextRow, nextCol)] = distance + 1;
                        queue.Enqueue((nextRow, nextCol, distance + 1));
                    }
                }
            }
            
            return distances;
        }
        
        /// <summary>
        /// 检查给定位置是否有效（在边界内且不是墙）。
        /// </summary>
        private bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row < _rows && 
                   col >= 0 && col < _cols && 
                   _maze[row, col] == 0; // 0表示通路
        }
        
        /// <summary>
        /// 检查迷宫中两点之间是否存在路径。
        /// </summary>
        /// <returns>如果存在路径返回true，否则返回false。</returns>
        public bool HasPath(int startRow, int startCol, int endRow, int endCol)
        {
            if (!IsValidPosition(startRow, startCol) || !IsValidPosition(endRow, endCol))
            {
                return false;
            }
            
            var visited = new bool[_rows, _cols];
            var queue = new Queue<(int row, int col)>();
            
            queue.Enqueue((startRow, startCol));
            visited[startRow, startCol] = true;
            
            while (queue.Count > 0)
            {
                var (currentRow, currentCol) = queue.Dequeue();
                
                if (currentRow == endRow && currentCol == endCol)
                {
                    return true;
                }
                
                for (int i = 0; i < 4; i++)
                {
                    int nextRow = currentRow + _dx[i];
                    int nextCol = currentCol + _dy[i];
                    
                    if (IsValidPosition(nextRow, nextCol) && !visited[nextRow, nextCol])
                    {
                        visited[nextRow, nextCol] = true;
                        queue.Enqueue((nextRow, nextCol));
                    }
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// 获取迷宫中所有连通区域。
        /// </summary>
        /// <returns>一个列表，每个子列表包含一个连通区域的所有位置。</returns>
        public List<List<(int row, int col)>> GetConnectedComponents()
        {
            var components = new List<List<(int, int)>>();
            var visited = new bool[_rows, _cols];
            
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (_maze[i, j] == 0 && !visited[i, j]) // 通路且未访问
                    {
                        var component = new List<(int, int)>();
                        var queue = new Queue<(int, int)>();
                        
                        queue.Enqueue((i, j));
                        visited[i, j] = true;
                        
                        while (queue.Count > 0)
                        {
                            var (row, col) = queue.Dequeue();
                            component.Add((row, col));
                            
                            for (int k = 0; k < 4; k++)
                            {
                                int nextRow = row + _dx[k];
                                int nextCol = col + _dy[k];
                                
                                if (IsValidPosition(nextRow, nextCol) && !visited[nextRow, nextCol])
                                {
                                    visited[nextRow, nextCol] = true;
                                    queue.Enqueue((nextRow, nextCol));
                                }
                            }
                        }
                        
                        components.Add(component);
                    }
                }
            }
            
            return components;
        }
    }
}