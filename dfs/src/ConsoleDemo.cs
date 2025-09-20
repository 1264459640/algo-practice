

using System;
using System.Collections.Generic;
using System.Linq;

namespace DFS
{
    /// <summary>
    /// DFS算法控制台演示程序
    /// 提供命令行界面来测试各种DFS功能
    /// </summary>
    public class ConsoleDemo
    {
        public static void Main(string[] args)
        {
            // 设置控制台编码
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            
            Console.WriteLine("=== 深度优先搜索 (DFS) 算法演示 ===");
            Console.WriteLine();
            
            // 演示1：基本DFS遍历
            Demo1_BasicTraversal();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示2：路径查找
            Demo2_PathFinding();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示3：环检测
            Demo3_CycleDetection();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示4：拓扑排序
            Demo4_TopologicalSort();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示5：迷宫求解
            Demo5_MazeSolving();
            
            Console.WriteLine();
            Console.WriteLine("演示完成！按任意键退出...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// 演示1：基本DFS遍历
        /// </summary>
        private static void Demo1_BasicTraversal()
        {
            Console.WriteLine("【演示1：基本DFS遍历】");
            Console.WriteLine();
            
            // 创建示例图
            var graph = new Graph(false);
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(1, 4);
            graph.AddEdge(2, 5);
            graph.AddEdge(3, 6);
            graph.AddEdge(4, 6);
            graph.AddEdge(5, 6);
            
            Console.WriteLine("图结构 (无向图):");
            DisplayGraph(graph);
            Console.WriteLine();
            
            // 递归DFS
            var recursiveResult = DepthFirstSearch.DFSRecursive(graph, 0);
            Console.WriteLine("递归DFS遍历 (从顶点0开始):");
            Console.WriteLine($"访问顺序: [{string.Join(", ", recursiveResult.VisitOrder)}]");
            
            Console.WriteLine("\n时间戳信息:");
            foreach (var vertex in recursiveResult.DiscoveryTime.Keys.OrderBy(v => v))
            {
                Console.WriteLine($"顶点{vertex}: 发现时间={recursiveResult.DiscoveryTime[vertex]}, " +
                                $"完成时间={recursiveResult.FinishTime[vertex]}");
            }
            
            // 迭代DFS
            var iterativeResult = DepthFirstSearch.DFSIterative(graph, 0);
            Console.WriteLine($"\n迭代DFS遍历 (从顶点0开始):");
            Console.WriteLine($"访问顺序: [{string.Join(", ", iterativeResult.VisitOrder)}]");
            
            // 完整DFS
            var completeResult = DepthFirstSearch.DFSComplete(graph);
            Console.WriteLine($"\n完整DFS遍历 (所有连通分量):");
            Console.WriteLine($"访问顺序: [{string.Join(", ", completeResult.VisitOrder)}]");
            Console.WriteLine($"连通分量数: {completeResult.ComponentCount}");
        }
        
        /// <summary>
        /// 演示2：路径查找
        /// </summary>
        private static void Demo2_PathFinding()
        {
            Console.WriteLine("【演示2：路径查找】");
            Console.WriteLine();
            
            // 创建更复杂的图
            var graph = new Graph(false);
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(2, 3);
            graph.AddEdge(2, 4);
            graph.AddEdge(3, 5);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 6);
            
            Console.WriteLine("图结构:");
            DisplayGraph(graph);
            Console.WriteLine();
            
            int start = 0, target = 6;
            
            // 查找单条路径
            var path = DepthFirstSearch.FindPath(graph, start, target);
            Console.WriteLine($"从顶点{start}到顶点{target}的路径:");
            if (path.Count > 0)
            {
                Console.WriteLine($"路径: [{string.Join(" → ", path)}]");
                Console.WriteLine($"路径长度: {path.Count - 1}条边");
            }
            else
            {
                Console.WriteLine("未找到路径");
            }
            
            // 查找所有路径
            var allPaths = DepthFirstSearch.FindAllPaths(graph, start, target);
            Console.WriteLine($"\n所有可能路径 (共{allPaths.Count}条):");
            for (int i = 0; i < allPaths.Count; i++)
            {
                Console.WriteLine($"路径{i + 1}: [{string.Join(" → ", allPaths[i])}] (长度: {allPaths[i].Count - 1})");
            }
            
            if (allPaths.Count > 0)
            {
                var shortestPath = allPaths.OrderBy(p => p.Count).First();
                Console.WriteLine($"\n最短路径: [{string.Join(" → ", shortestPath)}] (长度: {shortestPath.Count - 1})");
            }
        }
        
        /// <summary>
        /// 演示3：环检测
        /// </summary>
        private static void Demo3_CycleDetection()
        {
            Console.WriteLine("【演示3：环检测】");
            Console.WriteLine();
            
            // 测试无环图
            Console.WriteLine("测试1: 无环无向图");
            var acyclicGraph = new Graph(false);
            acyclicGraph.AddEdge(0, 1);
            acyclicGraph.AddEdge(1, 2);
            acyclicGraph.AddEdge(2, 3);
            acyclicGraph.AddEdge(1, 4);
            
            DisplayGraph(acyclicGraph);
            bool hasCycle1 = DepthFirstSearch.HasCycleUndirected(acyclicGraph);
            Console.WriteLine($"是否存在环: {(hasCycle1 ? "是" : "否")}");
            Console.WriteLine();
            
            // 测试有环图
            Console.WriteLine("测试2: 有环无向图");
            var cyclicGraph = new Graph(false);
            cyclicGraph.AddEdge(0, 1);
            cyclicGraph.AddEdge(1, 2);
            cyclicGraph.AddEdge(2, 3);
            cyclicGraph.AddEdge(3, 0); // 形成环
            cyclicGraph.AddEdge(1, 4);
            
            DisplayGraph(cyclicGraph);
            bool hasCycle2 = DepthFirstSearch.HasCycleUndirected(cyclicGraph);
            Console.WriteLine($"是否存在环: {(hasCycle2 ? "是" : "否")}");
            Console.WriteLine();
            
            // 测试有向图环检测
            Console.WriteLine("测试3: 有向图环检测");
            var directedGraph = new Graph(true);
            directedGraph.AddEdge(0, 1);
            directedGraph.AddEdge(1, 2);
            directedGraph.AddEdge(2, 3);
            directedGraph.AddEdge(3, 1); // 形成环
            
            DisplayGraph(directedGraph);
            bool hasCycle3 = DepthFirstSearch.HasCycleDirected(directedGraph);
            Console.WriteLine($"是否存在环: {(hasCycle3 ? "是" : "否")}");
            
            // 连通性分析
            Console.WriteLine();
            Console.WriteLine("连通性分析:");
            bool isConnected = DepthFirstSearch.IsConnected(cyclicGraph);
            var components = DepthFirstSearch.GetConnectedComponents(cyclicGraph);
            Console.WriteLine($"图是否连通: {(isConnected ? "是" : "否")}");
            Console.WriteLine($"连通分量数: {components.Count}");
            for (int i = 0; i < components.Count; i++)
            {
                Console.WriteLine($"分量{i + 1}: [{string.Join(", ", components[i].OrderBy(v => v))}]");
            }
        }
        
        /// <summary>
        /// 演示4：拓扑排序
        /// </summary>
        private static void Demo4_TopologicalSort()
        {
            Console.WriteLine("【演示4：拓扑排序】");
            Console.WriteLine();
            
            // 创建DAG (有向无环图)
            var dag = new Graph(true);
            dag.AddEdge(5, 2);
            dag.AddEdge(5, 0);
            dag.AddEdge(4, 0);
            dag.AddEdge(4, 1);
            dag.AddEdge(2, 3);
            dag.AddEdge(3, 1);
            
            Console.WriteLine("有向无环图 (DAG):");
            DisplayGraph(dag);
            Console.WriteLine();
            
            try
            {
                var topologicalOrder = DepthFirstSearch.TopologicalSort(dag);
                Console.WriteLine("拓扑排序结果:");
                Console.WriteLine($"[{string.Join(" → ", topologicalOrder)}]");
                Console.WriteLine();
                Console.WriteLine("应用场景示例:");
                Console.WriteLine("假设这些数字代表任务，箭头表示依赖关系");
                Console.WriteLine("拓扑排序给出了任务的执行顺序，确保所有依赖都被满足");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }
            
            Console.WriteLine();
            
            // 测试有环的有向图
            Console.WriteLine("测试有环的有向图:");
            var cyclicDAG = new Graph(true);
            cyclicDAG.AddEdge(0, 1);
            cyclicDAG.AddEdge(1, 2);
            cyclicDAG.AddEdge(2, 0); // 形成环
            
            DisplayGraph(cyclicDAG);
            
            try
            {
                var topologicalOrder = DepthFirstSearch.TopologicalSort(cyclicDAG);
                Console.WriteLine($"拓扑排序结果: [{string.Join(" → ", topologicalOrder)}]");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 演示5：迷宫求解
        /// </summary>
        private static void Demo5_MazeSolving()
        {
            Console.WriteLine("【演示5：迷宫求解】");
            Console.WriteLine();
            
            // 创建迷宫 (0=通路, 1=墙)
            int[,] maze = {
                {0, 1, 0, 0, 0, 0},
                {0, 1, 0, 1, 1, 0},
                {0, 0, 0, 1, 0, 0},
                {1, 1, 0, 0, 0, 1},
                {0, 0, 0, 1, 0, 0},
                {0, 1, 0, 0, 0, 0}
            };
            
            Console.WriteLine("迷宫结构 (S=起点, E=终点, ■=墙, □=通路):");
            DisplayMaze(maze, 0, 0, 5, 5);
            Console.WriteLine();
            
            var solver = new MazeSolver(maze);
            
            // 求解路径
            int startRow = 0, startCol = 0;
            int endRow = 5, endCol = 5;
            
            var path = solver.SolveMaze(startRow, startCol, endRow, endCol);
            
            Console.WriteLine($"从({startRow},{startCol})到({endRow},{endCol})的路径求解:");
            if (path.Count > 0)
            {
                Console.WriteLine($"找到解决方案! 路径长度: {path.Count}步");
                Console.WriteLine("路径坐标:");
                for (int i = 0; i < path.Count; i++)
                {
                    Console.Write($"({path[i].row},{path[i].col})");
                    if (i < path.Count - 1) Console.Write(" → ");
                    if ((i + 1) % 8 == 0) Console.WriteLine(); // 每行显示8个坐标
                }
                Console.WriteLine();
                Console.WriteLine();
                
                Console.WriteLine("带路径的迷宫显示 (●=路径):");
                DisplayMazeWithPath(maze, path, startRow, startCol, endRow, endCol);
            }
            else
            {
                Console.WriteLine("迷宫无解!");
            }
            
            Console.WriteLine();
            
            // 查找所有路径
            var allPaths = solver.FindAllPaths(startRow, startCol, endRow, endCol);
            Console.WriteLine($"所有可能路径: 共找到{allPaths.Count}条路径");
            
            if (allPaths.Count > 0)
            {
                var pathLengths = allPaths.Select(p => p.Count).ToList();
                Console.WriteLine($"最短路径长度: {pathLengths.Min()}步");
                Console.WriteLine($"最长路径长度: {pathLengths.Max()}步");
                Console.WriteLine($"平均路径长度: {pathLengths.Average():F1}步");
                
                if (allPaths.Count <= 5)
                {
                    Console.WriteLine("\n所有路径详情:");
                    for (int i = 0; i < allPaths.Count; i++)
                    {
                        Console.WriteLine($"路径{i + 1} (长度{allPaths[i].Count}): " +
                                        $"({allPaths[i][0].Item1},{allPaths[i][0].Item2}) → ... → " +
                                        $"({allPaths[i].Last().Item1},{allPaths[i].Last().Item2})");
                    }
                }
            }
        }
        
        /// <summary>
        /// 显示图结构
        /// </summary>
        private static void DisplayGraph(Graph graph)
        {
            Console.WriteLine($"顶点数: {graph.VertexCount}, 边数: {graph.EdgeCount}");
            Console.WriteLine("邻接表:");
            foreach (int vertex in graph.GetVertices().OrderBy(v => v))
            {
                var neighbors = graph.GetNeighbors(vertex);
                Console.WriteLine($"  顶点{vertex}: [{string.Join(", ", neighbors)}]");
            }
        }
        
        /// <summary>
        /// 显示迷宫
        /// </summary>
        private static void DisplayMaze(int[,] maze, int startRow, int startCol, int endRow, int endCol)
        {
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == startRow && j == startCol)
                        Console.Write("S ");
                    else if (i == endRow && j == endCol)
                        Console.Write("E ");
                    else if (maze[i, j] == 1)
                        Console.Write("■ ");
                    else
                        Console.Write("□ ");
                }
                Console.WriteLine();
            }
        }
        
        /// <summary>
        /// 显示带路径的迷宫
        /// </summary>
        private static void DisplayMazeWithPath(int[,] maze, List<(int row, int col)> path, 
            int startRow, int startCol, int endRow, int endCol)
        {
            var pathSet = new HashSet<(int, int)>(path);
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == startRow && j == startCol)
                        Console.Write("S ");
                    else if (i == endRow && j == endCol)
                        Console.Write("E ");
                    else if (pathSet.Contains((i, j)))
                        Console.Write("● ");
                    else if (maze[i, j] == 1)
                        Console.Write("■ ");
                    else
                        Console.Write("□ ");
                }
                Console.WriteLine();
            }
        }
    }
}