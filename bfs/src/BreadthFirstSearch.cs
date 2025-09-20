using System;
using System.Collections.Generic;
using System.Linq;

namespace BFS
{
    /// <summary>
    /// 宽度优先搜索算法实现类。
    /// 包含多种BFS变种和应用场景的静态方法实现。
    /// </summary>
    public static class BreadthFirstSearch
    {
        /// <summary>
        /// 基本的BFS遍历。
        /// 从给定的起始顶点开始遍历。
        /// </summary>
        /// <param name="graph">要遍历的图。</param>
        /// <param name="startVertex">遍历的起始顶点。</param>
        /// <returns>BFS遍历的结果。</returns>
        public static BFSResult BFS(Graph graph, int startVertex)
        {
            var result = new BFSResult();
            var visited = new HashSet<int>();
            var queue = new Queue<int>();
            
            queue.Enqueue(startVertex);
            visited.Add(startVertex);
            result.Distance[startVertex] = 0;
            result.Parent[startVertex] = -1; // 起始顶点没有父节点
            
            while (queue.Count > 0)
            {
                int vertex = queue.Dequeue();
                result.VisitOrder.Add(vertex);
                
                foreach (int neighbor in graph.GetNeighbors(vertex))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                        result.Distance[neighbor] = result.Distance[vertex] + 1;
                        result.Parent[neighbor] = vertex;
                    }
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 层次遍历BFS，按层次分组返回结果。
        /// </summary>
        /// <param name="graph">要遍历的图。</param>
        /// <param name="startVertex">遍历的起始顶点。</param>
        /// <returns>包含层次信息的BFS遍历结果。</returns>
        public static BFSResult BFSLevelOrder(Graph graph, int startVertex)
        {
            var result = new BFSResult();
            var visited = new HashSet<int>();
            var queue = new Queue<int>();
            
            queue.Enqueue(startVertex);
            visited.Add(startVertex);
            result.Distance[startVertex] = 0;
            result.Parent[startVertex] = -1;
            
            int currentLevel = 0;
            
            while (queue.Count > 0)
            {
                int levelSize = queue.Count;
                var currentLevelVertices = new List<int>();
                
                for (int i = 0; i < levelSize; i++)
                {
                    int vertex = queue.Dequeue();
                    result.VisitOrder.Add(vertex);
                    currentLevelVertices.Add(vertex);
                    
                    foreach (int neighbor in graph.GetNeighbors(vertex))
                    {
                        if (!visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            queue.Enqueue(neighbor);
                            result.Distance[neighbor] = currentLevel + 1;
                            result.Parent[neighbor] = vertex;
                        }
                    }
                }
                
                result.Levels.Add(currentLevelVertices);
                currentLevel++;
            }
            
            return result;
        }
        
        /// <summary>
        /// 对整个图进行BFS遍历，可以处理非连通图。
        /// </summary>
        /// <param name="graph">要遍历的图。</param>
        /// <returns>完整的BFS遍历结果。</returns>
        public static BFSResult BFSComplete(Graph graph)
        {
            var result = new BFSResult();
            var visited = new HashSet<int>();
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    result.ComponentCount++;
                    var componentResult = BFS(graph, vertex);
                    
                    // 合并结果
                    result.VisitOrder.AddRange(componentResult.VisitOrder);
                    foreach (var kvp in componentResult.Distance)
                    {
                        result.Distance[kvp.Key] = kvp.Value;
                    }
                    foreach (var kvp in componentResult.Parent)
                    {
                        result.Parent[kvp.Key] = kvp.Value;
                    }
                    
                    // 标记已访问的顶点
                    foreach (int v in componentResult.VisitOrder)
                    {
                        visited.Add(v);
                    }
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 使用BFS查找从起点到终点的最短路径。
        /// </summary>
        /// <param name="graph">要搜索的图。</param>
        /// <param name="start">起始顶点。</param>
        /// <param name="target">目标顶点。</param>
        /// <returns>如果找到路径，则返回顶点列表；否则返回空列表。</returns>
        public static List<int> FindShortestPath(Graph graph, int start, int target)
        {
            var result = BFS(graph, start);
            
            if (!result.Parent.ContainsKey(target))
            {
                return new List<int>(); // 未找到路径
            }
            
            // 重构路径
            var path = new List<int>();
            int current = target;
            
            while (current != -1)
            {
                path.Add(current);
                current = result.Parent.ContainsKey(current) ? result.Parent[current] : -1;
            }
            
            path.Reverse();
            return path;
        }
        
        /// <summary>
        /// 计算从起点到所有其他顶点的最短距离。
        /// </summary>
        /// <param name="graph">要分析的图。</param>
        /// <param name="startVertex">起始顶点。</param>
        /// <returns>包含距离信息的字典。</returns>
        public static Dictionary<int, int> FindShortestDistances(Graph graph, int startVertex)
        {
            var result = BFS(graph, startVertex);
            return result.Distance;
        }
        
        /// <summary>
        /// 检查图是否为二分图。
        /// </summary>
        /// <param name="graph">要检查的图。</param>
        /// <returns>如果是二分图，则返回着色结果；否则返回null。</returns>
        public static BFSResult CheckBipartite(Graph graph)
        {
            var result = new BFSResult();
            var visited = new HashSet<int>();
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    if (!CheckBipartiteComponent(graph, vertex, visited, result))
                    {
                        result.IsBipartite = false;
                        return result;
                    }
                }
            }
            
            result.IsBipartite = true;
            return result;
        }
        
        /// <summary>
        /// 检查单个连通分量是否为二分图的辅助方法。
        /// </summary>
        private static bool CheckBipartiteComponent(Graph graph, int startVertex, 
            HashSet<int> visited, BFSResult result)
        {
            var queue = new Queue<int>();
            
            queue.Enqueue(startVertex);
            visited.Add(startVertex);
            result.Coloring[startVertex] = 0; // 起始顶点着色为0
            
            while (queue.Count > 0)
            {
                int vertex = queue.Dequeue();
                
                foreach (int neighbor in graph.GetNeighbors(vertex))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                        // 邻接顶点着相反的颜色
                        result.Coloring[neighbor] = 1 - result.Coloring[vertex];
                    }
                    else if (result.Coloring[neighbor] == result.Coloring[vertex])
                    {
                        // 相邻顶点有相同颜色，不是二分图
                        return false;
                    }
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// 检查图是否是连通的（仅适用于无向图）。
        /// </summary>
        /// <param name="graph">要检查的图。</param>
        /// <returns>如果图是连通的，则为true；否则为false。</returns>
        public static bool IsConnected(Graph graph)
        {
            if (graph.VertexCount == 0) return true;
            
            var vertices = graph.GetVertices().ToList();
            if (vertices.Count == 0) return true;
            
            var result = BFS(graph, vertices[0]);
            return result.VisitOrder.Count == graph.VertexCount;
        }
        
        /// <summary>
        /// 获取图的所有连通分量（仅适用于无向图）。
        /// </summary>
        /// <param name="graph">要分析的图。</param>
        /// <returns>一个列表，其中每个子列表代表一个连通分量。</returns>
        public static List<List<int>> GetConnectedComponents(Graph graph)
        {
            var components = new List<List<int>>();
            var visited = new HashSet<int>();
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    var componentResult = BFS(graph, vertex);
                    components.Add(componentResult.VisitOrder);
                    
                    foreach (int v in componentResult.VisitOrder)
                    {
                        visited.Add(v);
                    }
                }
            }
            
            return components;
        }
        
        /// <summary>
        /// 查找从起点到终点的所有最短路径。
        /// </summary>
        /// <param name="graph">要搜索的图。</param>
        /// <param name="start">起始顶点。</param>
        /// <param name="target">目标顶点。</param>
        /// <returns>一个包含所有最短路径的列表。</returns>
        public static List<List<int>> FindAllShortestPaths(Graph graph, int start, int target)
        {
            var allPaths = new List<List<int>>();
            var visited = new HashSet<int>();
            var queue = new Queue<(int vertex, List<int> path)>();
            var distances = new Dictionary<int, int>();
            
            queue.Enqueue((start, new List<int> { start }));
            distances[start] = 0;
            int targetDistance = -1;
            
            while (queue.Count > 0)
            {
                var (vertex, path) = queue.Dequeue();
                
                if (vertex == target)
                {
                    if (targetDistance == -1)
                    {
                        targetDistance = path.Count - 1;
                    }
                    
                    if (path.Count - 1 == targetDistance)
                    {
                        allPaths.Add(new List<int>(path));
                    }
                    continue;
                }
                
                if (targetDistance != -1 && path.Count - 1 >= targetDistance)
                {
                    continue; // 已经找到更短或等长的路径
                }
                
                foreach (int neighbor in graph.GetNeighbors(vertex))
                {
                    if (!path.Contains(neighbor))
                    {
                        int newDistance = path.Count;
                        
                        if (!distances.ContainsKey(neighbor) || distances[neighbor] >= newDistance)
                        {
                            distances[neighbor] = newDistance;
                            var newPath = new List<int>(path) { neighbor };
                            queue.Enqueue((neighbor, newPath));
                        }
                    }
                }
            }
            
            return allPaths;
        }
    }
}