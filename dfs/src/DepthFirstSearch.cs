using System;
using System.Collections.Generic;
using System.Linq;

namespace DFS
{
    /// <summary>
    /// 深度优先搜索算法实现类
    /// 提供多种DFS变种和应用场景的实现
    /// </summary>
    public static class DepthFirstSearch
    {
        /// <summary>
        /// 图的表示方式：邻接表
        /// </summary>
        public class Graph
        {
            private readonly Dictionary<int, List<int>> _adjacencyList;
            private readonly bool _isDirected;
            
            public int VertexCount { get; private set; }
            public int EdgeCount { get; private set; }
            
            public Graph(bool isDirected = false)
            {
                _adjacencyList = new Dictionary<int, List<int>>();
                _isDirected = isDirected;
                VertexCount = 0;
                EdgeCount = 0;
            }
            
            /// <summary>
            /// 添加顶点
            /// </summary>
            public void AddVertex(int vertex)
            {
                if (!_adjacencyList.ContainsKey(vertex))
                {
                    _adjacencyList[vertex] = new List<int>();
                    VertexCount++;
                }
            }
            
            /// <summary>
            /// 添加边
            /// </summary>
            public void AddEdge(int from, int to)
            {
                AddVertex(from);
                AddVertex(to);
                
                _adjacencyList[from].Add(to);
                EdgeCount++;
                
                if (!_isDirected)
                {
                    _adjacencyList[to].Add(from);
                }
            }
            
            /// <summary>
            /// 获取顶点的邻接顶点
            /// </summary>
            public IEnumerable<int> GetNeighbors(int vertex)
            {
                return _adjacencyList.ContainsKey(vertex) ? _adjacencyList[vertex] : Enumerable.Empty<int>();
            }
            
            /// <summary>
            /// 获取所有顶点
            /// </summary>
            public IEnumerable<int> GetVertices()
            {
                return _adjacencyList.Keys;
            }
            
            /// <summary>
            /// 检查顶点是否存在
            /// </summary>
            public bool ContainsVertex(int vertex)
            {
                return _adjacencyList.ContainsKey(vertex);
            }
        }
        
        /// <summary>
        /// DFS遍历结果
        /// </summary>
        public class DFSResult
        {
            public List<int> VisitOrder { get; set; } = new List<int>();
            public Dictionary<int, int> DiscoveryTime { get; set; } = new Dictionary<int, int>();
            public Dictionary<int, int> FinishTime { get; set; } = new Dictionary<int, int>();
            public Dictionary<int, int> Parent { get; set; } = new Dictionary<int, int>();
            public List<List<int>> Paths { get; set; } = new List<List<int>>();
            public int ComponentCount { get; set; } = 0;
            public bool HasCycle { get; set; } = false;
        }
        
        /// <summary>
        /// 基本DFS遍历（递归版本）
        /// </summary>
        public static DFSResult DFSRecursive(Graph graph, int startVertex)
        {
            var result = new DFSResult();
            var visited = new HashSet<int>();
            var time = 0;
            
            DFSRecursiveHelper(graph, startVertex, visited, result, ref time, -1);
            
            return result;
        }
        
        private static void DFSRecursiveHelper(Graph graph, int vertex, HashSet<int> visited, 
            DFSResult result, ref int time, int parent)
        {
            // 标记为已访问
            visited.Add(vertex);
            result.VisitOrder.Add(vertex);
            result.DiscoveryTime[vertex] = ++time;
            result.Parent[vertex] = parent;
            
            // 遍历所有邻接顶点
            foreach (int neighbor in graph.GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    DFSRecursiveHelper(graph, neighbor, visited, result, ref time, vertex);
                }
            }
            
            result.FinishTime[vertex] = ++time;
        }
        
        /// <summary>
        /// 迭代DFS遍历（使用栈）
        /// </summary>
        public static DFSResult DFSIterative(Graph graph, int startVertex)
        {
            var result = new DFSResult();
            var visited = new HashSet<int>();
            var stack = new Stack<int>();
            
            stack.Push(startVertex);
            
            while (stack.Count > 0)
            {
                int vertex = stack.Pop();
                
                if (!visited.Contains(vertex))
                {
                    visited.Add(vertex);
                    result.VisitOrder.Add(vertex);
                    
                    // 将邻接顶点压入栈（逆序以保持一致的遍历顺序）
                    var neighbors = graph.GetNeighbors(vertex).Reverse();
                    foreach (int neighbor in neighbors)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 完整图的DFS遍历（处理非连通图）
        /// </summary>
        public static DFSResult DFSComplete(Graph graph)
        {
            var result = new DFSResult();
            var visited = new HashSet<int>();
            var time = 0;
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    result.ComponentCount++;
                    DFSRecursiveHelper(graph, vertex, visited, result, ref time, -1);
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 路径查找DFS
        /// </summary>
        public static List<int> FindPath(Graph graph, int start, int target)
        {
            var visited = new HashSet<int>();
            var path = new List<int>();
            
            if (FindPathHelper(graph, start, target, visited, path))
            {
                return path;
            }
            
            return new List<int>(); // 未找到路径
        }
        
        private static bool FindPathHelper(Graph graph, int current, int target, 
            HashSet<int> visited, List<int> path)
        {
            visited.Add(current);
            path.Add(current);
            
            if (current == target)
            {
                return true; // 找到目标
            }
            
            foreach (int neighbor in graph.GetNeighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    if (FindPathHelper(graph, neighbor, target, visited, path))
                    {
                        return true;
                    }
                }
            }
            
            path.RemoveAt(path.Count - 1); // 回溯
            return false;
        }
        
        /// <summary>
        /// 查找所有路径
        /// </summary>
        public static List<List<int>> FindAllPaths(Graph graph, int start, int target)
        {
            var allPaths = new List<List<int>>();
            var visited = new HashSet<int>();
            var currentPath = new List<int>();
            
            FindAllPathsHelper(graph, start, target, visited, currentPath, allPaths);
            
            return allPaths;
        }
        
        private static void FindAllPathsHelper(Graph graph, int current, int target,
            HashSet<int> visited, List<int> currentPath, List<List<int>> allPaths)
        {
            visited.Add(current);
            currentPath.Add(current);
            
            if (current == target)
            {
                allPaths.Add(new List<int>(currentPath)); // 保存路径副本
            }
            else
            {
                foreach (int neighbor in graph.GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        FindAllPathsHelper(graph, neighbor, target, visited, currentPath, allPaths);
                    }
                }
            }
            
            // 回溯
            visited.Remove(current);
            currentPath.RemoveAt(currentPath.Count - 1);
        }
        
        /// <summary>
        /// 检测无向图中的环
        /// </summary>
        public static bool HasCycleUndirected(Graph graph)
        {
            var visited = new HashSet<int>();
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    if (HasCycleUndirectedHelper(graph, vertex, -1, visited))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        private static bool HasCycleUndirectedHelper(Graph graph, int vertex, int parent, HashSet<int> visited)
        {
            visited.Add(vertex);
            
            foreach (int neighbor in graph.GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    if (HasCycleUndirectedHelper(graph, neighbor, vertex, visited))
                    {
                        return true;
                    }
                }
                else if (neighbor != parent)
                {
                    return true; // 发现环
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// 检测有向图中的环
        /// </summary>
        public static bool HasCycleDirected(Graph graph)
        {
            var visited = new HashSet<int>();
            var recStack = new HashSet<int>();
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    if (HasCycleDirectedHelper(graph, vertex, visited, recStack))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        private static bool HasCycleDirectedHelper(Graph graph, int vertex, 
            HashSet<int> visited, HashSet<int> recStack)
        {
            visited.Add(vertex);
            recStack.Add(vertex);
            
            foreach (int neighbor in graph.GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    if (HasCycleDirectedHelper(graph, neighbor, visited, recStack))
                    {
                        return true;
                    }
                }
                else if (recStack.Contains(neighbor))
                {
                    return true; // 发现后向边，存在环
                }
            }
            
            recStack.Remove(vertex);
            return false;
        }
        
        /// <summary>
        /// 拓扑排序（基于DFS）
        /// </summary>
        public static List<int> TopologicalSort(Graph graph)
        {
            if (HasCycleDirected(graph))
            {
                throw new InvalidOperationException("图中存在环，无法进行拓扑排序");
            }
            
            var visited = new HashSet<int>();
            var stack = new Stack<int>();
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    TopologicalSortHelper(graph, vertex, visited, stack);
                }
            }
            
            return stack.ToList();
        }
        
        private static void TopologicalSortHelper(Graph graph, int vertex, 
            HashSet<int> visited, Stack<int> stack)
        {
            visited.Add(vertex);
            
            foreach (int neighbor in graph.GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    TopologicalSortHelper(graph, neighbor, visited, stack);
                }
            }
            
            stack.Push(vertex); // 完成时间逆序
        }
        
        /// <summary>
        /// 检查图的连通性
        /// </summary>
        public static bool IsConnected(Graph graph)
        {
            if (graph.VertexCount == 0) return true;
            
            var vertices = graph.GetVertices().ToList();
            if (vertices.Count == 0) return true;
            
            var result = DFSRecursive(graph, vertices[0]);
            return result.VisitOrder.Count == graph.VertexCount;
        }
        
        /// <summary>
        /// 获取连通分量
        /// </summary>
        public static List<List<int>> GetConnectedComponents(Graph graph)
        {
            var components = new List<List<int>>();
            var visited = new HashSet<int>();
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    var component = new List<int>();
                    GetConnectedComponentHelper(graph, vertex, visited, component);
                    components.Add(component);
                }
            }
            
            return components;
        }
        
        private static void GetConnectedComponentHelper(Graph graph, int vertex, 
            HashSet<int> visited, List<int> component)
        {
            visited.Add(vertex);
            component.Add(vertex);
            
            foreach (int neighbor in graph.GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    GetConnectedComponentHelper(graph, neighbor, visited, component);
                }
            }
        }
    }
    
    /// <summary>
    /// 迷宫求解器（DFS应用示例）
    /// </summary>
    public class MazeSolver
    {
        private readonly int[,] _maze;
        private readonly int _rows;
        private readonly int _cols;
        private readonly int[] _dx = { -1, 1, 0, 0 }; // 上下左右
        private readonly int[] _dy = { 0, 0, -1, 1 };
        
        public MazeSolver(int[,] maze)
        {
            _maze = maze;
            _rows = maze.GetLength(0);
            _cols = maze.GetLength(1);
        }
        
        /// <summary>
        /// 求解迷宫路径
        /// </summary>
        public List<(int row, int col)> SolveMaze(int startRow, int startCol, int endRow, int endCol)
        {
            var visited = new bool[_rows, _cols];
            var path = new List<(int, int)>();
            
            if (SolveMazeHelper(startRow, startCol, endRow, endCol, visited, path))
            {
                return path;
            }
            
            return new List<(int, int)>(); // 无解
        }
        
        private bool SolveMazeHelper(int row, int col, int endRow, int endCol, 
            bool[,] visited, List<(int, int)> path)
        {
            // 检查边界和障碍物
            if (row < 0 || row >= _rows || col < 0 || col >= _cols || 
                _maze[row, col] == 1 || visited[row, col])
            {
                return false;
            }
            
            visited[row, col] = true;
            path.Add((row, col));
            
            // 到达终点
            if (row == endRow && col == endCol)
            {
                return true;
            }
            
            // 尝试四个方向
            for (int i = 0; i < 4; i++)
            {
                int newRow = row + _dx[i];
                int newCol = col + _dy[i];
                
                if (SolveMazeHelper(newRow, newCol, endRow, endCol, visited, path))
                {
                    return true;
                }
            }
            
            // 回溯
            path.RemoveAt(path.Count - 1);
            return false;
        }
        
        /// <summary>
        /// 查找迷宫中的所有路径
        /// </summary>
        public List<List<(int, int)>> FindAllPaths(int startRow, int startCol, int endRow, int endCol)
        {
            var allPaths = new List<List<(int, int)>>();
            var visited = new bool[_rows, _cols];
            var currentPath = new List<(int, int)>();
            
            FindAllPathsHelper(startRow, startCol, endRow, endCol, visited, currentPath, allPaths);
            
            return allPaths;
        }
        
        private void FindAllPathsHelper(int row, int col, int endRow, int endCol,
            bool[,] visited, List<(int, int)> currentPath, List<List<(int, int)>> allPaths)
        {
            if (row < 0 || row >= _rows || col < 0 || col >= _cols || 
                _maze[row, col] == 1 || visited[row, col])
            {
                return;
            }
            
            visited[row, col] = true;
            currentPath.Add((row, col));
            
            if (row == endRow && col == endCol)
            {
                allPaths.Add(new List<(int, int)>(currentPath));
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    int newRow = row + _dx[i];
                    int newCol = col + _dy[i];
                    FindAllPathsHelper(newRow, newCol, endRow, endCol, visited, currentPath, allPaths);
                }
            }
            
            // 回溯
            visited[row, col] = false;
            currentPath.RemoveAt(currentPath.Count - 1);
        }
    }
}