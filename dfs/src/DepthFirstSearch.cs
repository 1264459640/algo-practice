using System;
using System.Collections.Generic;
using System.Linq;

namespace DFS
{
    /// <summary>
    /// 深度优先搜索算法实现类。
    /// 包含多种DFS变种和应用场景的静态方法实现。
    /// </summary>
    public static class DepthFirstSearch
    {

        
        /// <summary>
        /// 基本的DFS遍历（递归版本）。
        /// 从给定的起始顶点开始遍历。
        /// </summary>
        /// <param name="graph">要遍历的图。</param>
        /// <param name="startVertex">遍历的起始顶点。</param>
        /// <returns>DFS遍历的结果。</returns>
        public static DFSResult DFSRecursive(Graph graph, int startVertex)
        {
            var result = new DFSResult();
            var visited = new HashSet<int>(); // 用于跟踪已访问的顶点
            var time = 0; // 用于记录发现和完成时间的时间戳
            
            DFSRecursiveHelper(graph, startVertex, visited, result, ref time, -1); // -1表示起始顶点没有父节点
            
            return result;
        }
        
        /// <summary>
        /// 递归DFS的辅助方法。
        /// </summary>
        private static void DFSRecursiveHelper(Graph graph, int vertex, HashSet<int> visited, 
            DFSResult result, ref int time, int parent)
        {
            // 标记当前顶点为已访问，并记录发现时间
            visited.Add(vertex);
            result.VisitOrder.Add(vertex);
            result.DiscoveryTime[vertex] = ++time;
            result.Parent[vertex] = parent;
            
            // 递归地访问所有未被访问的邻接顶点
            foreach (int neighbor in graph.GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    DFSRecursiveHelper(graph, neighbor, visited, result, ref time, vertex);
                }
            }
            
            // 当一个顶点的所有邻接顶点都被访问后，记录其完成时间
            result.FinishTime[vertex] = ++time;
        }
        
        /// <summary>
        /// 迭代方式的DFS遍历（使用栈）。
        /// </summary>
        /// <param name="graph">要遍历的图。</param>
        /// <param name="startVertex">遍历的起始顶点。</param>
        /// <returns>DFS遍历的结果。</returns>
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
                    
                    // 将邻接顶点逆序压入栈，以确保遍历顺序与递归版本一致
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
        /// 对整个图进行DFS遍历，可以处理非连通图。
        /// </summary>
        /// <param name="graph">要遍历的图。</param>
        /// <returns>完整的DFS遍历结果。</returns>
        public static DFSResult DFSComplete(Graph graph)
        {
            var result = new DFSResult();
            var visited = new HashSet<int>();
            var time = 0;
            
            // 遍历图中的每一个顶点
            foreach (int vertex in graph.GetVertices())
            {
                // 如果顶点未被访问，说明它属于一个新的连通分量
                if (!visited.Contains(vertex))
                {
                    result.ComponentCount++;
                    DFSRecursiveHelper(graph, vertex, visited, result, ref time, -1);
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 使用DFS查找从起点到终点的单条路径。
        /// </summary>
        /// <param name="graph">要搜索的图。</param>
        /// <param name="start">起始顶点。</param>
        /// <param name="target">目标顶点。</param>
        /// <returns>如果找到路径，则返回顶点列表；否则返回空列表。</returns>
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
        
        /// <summary>
        /// 查找路径的递归辅助方法。
        /// </summary>
        private static bool FindPathHelper(Graph graph, int current, int target, 
            HashSet<int> visited, List<int> path)
        {
            visited.Add(current);
            path.Add(current);
            
            // 如果当前顶点是目标，则成功找到路径
            if (current == target)
            {
                return true;
            }
            
            // 遍历邻接顶点，继续搜索
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
            
            // 如果从当前顶点出发的所有路径都无法到达目标，则回溯
            path.RemoveAt(path.Count - 1);
            return false;
        }
        
        /// <summary>
        /// 查找从起点到终点的所有可能路径。
        /// </summary>
        /// <param name="graph">要搜索的图。</param>
        /// <param name="start">起始顶点。</param>
        /// <param name="target">目标顶点。</param>
        /// <returns>一个包含所有路径的列表。</returns>
        public static List<List<int>> FindAllPaths(Graph graph, int start, int target)
        {
            var allPaths = new List<List<int>>();
            var visited = new HashSet<int>();
            var currentPath = new List<int>();
            
            FindAllPathsHelper(graph, start, target, visited, currentPath, allPaths);
            
            return allPaths;
        }
        
        /// <summary>
        /// 查找所有路径的递归辅助方法。
        /// </summary>
        private static void FindAllPathsHelper(Graph graph, int current, int target,
            HashSet<int> visited, List<int> currentPath, List<List<int>> allPaths)
        {
            visited.Add(current);
            currentPath.Add(current);
            
            if (current == target)
            {
                // 找到一条路径，将其副本添加到结果列表中
                allPaths.Add(new List<int>(currentPath));
            }
            else
            {
                // 继续在邻接顶点中搜索
                foreach (int neighbor in graph.GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        FindAllPathsHelper(graph, neighbor, target, visited, currentPath, allPaths);
                    }
                }
            }
            
            // 回溯，以便在其他分支中继续查找
            visited.Remove(current);
            currentPath.RemoveAt(currentPath.Count - 1);
        }
        
        /// <summary>
        /// 检测无向图中是否存在环。
        /// </summary>
        /// <param name="graph">要检查的无向图。</param>
        /// <returns>如果存在环，则为true；否则为false。</returns>
        public static bool HasCycleUndirected(Graph graph)
        {
            var visited = new HashSet<int>();
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    // 对每个未访问的连通分量进行环检测
                    if (HasCycleUndirectedHelper(graph, vertex, -1, visited))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// 无向图环检测的递归辅助方法。
        /// </summary>
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
                // 如果邻接顶点已被访问，并且它不是当前顶点的父节点，则说明存在环
                else if (neighbor != parent)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// 检测有向图中是否存在环。
        /// </summary>
        /// <param name="graph">要检查的有向图。</param>
        /// <returns>如果存在环，则为true；否则为false。</returns>
        public static bool HasCycleDirected(Graph graph)
        {
            var visited = new HashSet<int>(); // 存储已访问过的顶点
            var recStack = new HashSet<int>(); // 存储当前递归栈中的顶点
            
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
        
        /// <summary>
        /// 有向图环检测的递归辅助方法。
        /// </summary>
        private static bool HasCycleDirectedHelper(Graph graph, int vertex, 
            HashSet<int> visited, HashSet<int> recStack)
        {
            visited.Add(vertex);
            recStack.Add(vertex); // 将当前顶点加入递归栈
            
            foreach (int neighbor in graph.GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    if (HasCycleDirectedHelper(graph, neighbor, visited, recStack))
                    {
                        return true;
                    }
                }
                // 如果邻接顶点在递归栈中，说明存在后向边，即有环
                else if (recStack.Contains(neighbor))
                {
                    return true;
                }
            }
            
            recStack.Remove(vertex); // 离开当前递归层，将顶点移出递归栈
            return false;
        }
        
        /// <summary>
        /// 对有向无环图（DAG）进行拓扑排序。
        /// </summary>
        /// <param name="graph">要排序的有向图。</param>
        /// <returns>一个包含拓扑排序后顶点的列表。</returns>
        /// <exception cref="InvalidOperationException">如果图中存在环，则抛出异常。</exception>
        public static List<int> TopologicalSort(Graph graph)
        {
            if (HasCycleDirected(graph))
            {
                throw new InvalidOperationException("图中存在环，无法进行拓扑排序");
            }
            
            var visited = new HashSet<int>();
            var stack = new Stack<int>(); // 用于存储拓扑排序的结果
            
            foreach (int vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex))
                {
                    TopologicalSortHelper(graph, vertex, visited, stack);
                }
            }
            
            return stack.ToList(); // 栈中顶点的顺序就是拓扑排序的结果
        }
        
        /// <summary>
        /// 拓扑排序的递归辅助方法。
        /// </summary>
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
            
            // 当一个顶点的所有后代都被访问后，将该顶点压入栈
            stack.Push(vertex);
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
            
            // 从任意顶点开始DFS，如果能访问到所有顶点，则图是连通的
            var result = DFSRecursive(graph, vertices[0]);
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
                    var component = new List<int>();
                    GetConnectedComponentHelper(graph, vertex, visited, component);
                    components.Add(component);
                }
            }
            
            return components;
        }
        
        /// <summary>
        /// 获取连通分量的递归辅助方法。
        /// </summary>
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

}