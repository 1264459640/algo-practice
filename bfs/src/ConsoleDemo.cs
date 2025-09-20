using System;
using System.Collections.Generic;
using System.Linq;

namespace BFS
{
    /// <summary>
    /// BFS算法控制台演示程序
    /// 提供命令行界面来测试各种BFS功能
    /// </summary>
    public class ConsoleDemo
    {
        public static void Main(string[] args)
        {
            // 设置控制台编码
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            
            Console.WriteLine("=== 宽度优先搜索 (BFS) 算法演示 ===");
            Console.WriteLine();
            
            // 演示1：基本BFS遍历
            Demo1_BasicTraversal();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示2：最短路径查找
            Demo2_ShortestPath();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示3：层次遍历
            Demo3_LevelOrder();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示4：二分图检测
            Demo4_BipartiteCheck();
            
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
        /// 演示1：基本BFS遍历
        /// </summary>
        private static void Demo1_BasicTraversal()
        {
            Console.WriteLine("【演示1：基本BFS遍历】");
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
            
            // BFS遍历
            var result = BreadthFirstSearch.BFS(graph, 0);
            Console.WriteLine("BFS遍历 (从顶点0开始):");
            Console.WriteLine($"访问顺序: [{string.Join(", ", result.VisitOrder)}]");
            
            Console.WriteLine("\n距离信息:");
            foreach (var vertex in result.Distance.Keys.OrderBy(v => v))
            {
                Console.WriteLine($"顶点{vertex}: 距离={result.Distance[vertex]}");
            }
            
            // 完整BFS
            var completeResult = BreadthFirstSearch.BFSComplete(graph);
            Console.WriteLine($"\n完整BFS遍历 (所有连通分量):");
            Console.WriteLine($"访问顺序: [{string.Join(", ", completeResult.VisitOrder)}]");
            Console.WriteLine($"连通分量数: {completeResult.ComponentCount}");
            
            // 连通性检查
            bool isConnected = BreadthFirstSearch.IsConnected(graph);
            Console.WriteLine($"图是否连通: {(isConnected ? "是" : "否")}");
        }
        
        /// <summary>
        /// 演示2：最短路径查找
        /// </summary>
        private static void Demo2_ShortestPath()
        {
            Console.WriteLine("【演示2：最短路径查找】");
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
            graph.AddEdge(1, 7);
            graph.AddEdge(7, 6);
            
            Console.WriteLine("图结构:");
            DisplayGraph(graph);
            Console.WriteLine();
            
            int start = 0, target = 6;
            
            // 查找最短路径
            var shortestPath = BreadthFirstSearch.FindShortestPath(graph, start, target);
            Console.WriteLine($"从顶点{start}到顶点{target}的最短路径:");
            if (shortestPath.Count > 0)
            {
                Console.WriteLine($"路径: [{string.Join(" → ", shortestPath)}]");
                Console.WriteLine($"路径长度: {shortestPath.Count - 1}条边");
            }
            else
            {
                Console.WriteLine("未找到路径");
            }
            
            // 查找所有最短路径
            var allShortestPaths = BreadthFirstSearch.FindAllShortestPaths(graph, start, target);
            Console.WriteLine($"\n所有最短路径 (共{allShortestPaths.Count}条):");
            for (int i = 0; i < allShortestPaths.Count; i++)
            {
                Console.WriteLine($"路径{i + 1}: [{string.Join(" → ", allShortestPaths[i])}] (长度: {allShortestPaths[i].Count - 1})");
            }
            
            // 计算到所有顶点的最短距离
            var distances = BreadthFirstSearch.FindShortestDistances(graph, start);
            Console.WriteLine($"\n从顶点{start}到所有顶点的最短距离:");
            foreach (var kvp in distances.OrderBy(x => x.Key))
            {
                Console.WriteLine($"到顶点{kvp.Key}: {kvp.Value}步");
            }
        }
        
        /// <summary>
        /// 演示3：层次遍历
        /// </summary>
        private static void Demo3_LevelOrder()
        {
            Console.WriteLine("【演示3：层次遍历】");
            Console.WriteLine();
            
            // 创建树状图
            var graph = new Graph(false);
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(1, 4);
            graph.AddEdge(2, 5);
            graph.AddEdge(2, 6);
            graph.AddEdge(3, 7);
            graph.AddEdge(4, 8);
            
            Console.WriteLine("树状图结构:");
            DisplayGraph(graph);
            Console.WriteLine();
            
            var result = BreadthFirstSearch.BFSLevelOrder(graph, 0);
            Console.WriteLine("层次遍历结果:");
            Console.WriteLine($"访问顺序: [{string.Join(", ", result.VisitOrder)}]");
            Console.WriteLine();
            
            Console.WriteLine("按层次分组:");
            for (int i = 0; i < result.Levels.Count; i++)
            {
                Console.WriteLine($"第{i}层: [{string.Join(", ", result.Levels[i])}]");
            }
            
            Console.WriteLine();
            Console.WriteLine("应用场景示例:");
            Console.WriteLine("- 文件系统目录遍历");
            Console.WriteLine("- 组织架构层次显示");
            Console.WriteLine("- 网站页面层次爬取");
        }
        
        /// <summary>
        /// 演示4：二分图检测
        /// </summary>
        private static void Demo4_BipartiteCheck()
        {
            Console.WriteLine("【演示4：二分图检测】");
            Console.WriteLine();
            
            // 测试二分图
            Console.WriteLine("测试1: 二分图");
            var bipartiteGraph = new Graph(false);
            bipartiteGraph.AddEdge(0, 1);
            bipartiteGraph.AddEdge(0, 3);
            bipartiteGraph.AddEdge(1, 2);
            bipartiteGraph.AddEdge(2, 3);
            bipartiteGraph.AddEdge(1, 4);
            bipartiteGraph.AddEdge(3, 4);
            
            DisplayGraph(bipartiteGraph);
            var bipartiteResult = BreadthFirstSearch.CheckBipartite(bipartiteGraph);
            Console.WriteLine($"是否为二分图: {(bipartiteResult.IsBipartite ? "是" : "否")}");
            
            if (bipartiteResult.IsBipartite)
            {
                Console.WriteLine("着色结果:");
                var group0 = bipartiteResult.Coloring.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key).OrderBy(x => x);
                var group1 = bipartiteResult.Coloring.Where(kvp => kvp.Value == 1).Select(kvp => kvp.Key).OrderBy(x => x);
                Console.WriteLine($"组1 (颜色0): [{string.Join(", ", group0)}]");
                Console.WriteLine($"组2 (颜色1): [{string.Join(", ", group1)}]");
            }
            
            Console.WriteLine();
            
            // 测试非二分图
            Console.WriteLine("测试2: 非二分图 (包含奇数环)");
            var nonBipartiteGraph = new Graph(false);
            nonBipartiteGraph.AddEdge(0, 1);
            nonBipartiteGraph.AddEdge(1, 2);
            nonBipartiteGraph.AddEdge(2, 0); // 形成三角形（奇数环）
            nonBipartiteGraph.AddEdge(0, 3);
            
            DisplayGraph(nonBipartiteGraph);
            var nonBipartiteResult = BreadthFirstSearch.CheckBipartite(nonBipartiteGraph);
            Console.WriteLine($"是否为二分图: {(nonBipartiteResult.IsBipartite ? "是" : "否")}");
            
            Console.WriteLine();
            Console.WriteLine("二分图应用场景:");
            Console.WriteLine("- 任务分配问题");
            Console.WriteLine("- 匹配问题");
            Console.WriteLine("- 调度问题");
            Console.WriteLine("- 图着色问题");
        }
        
        /// <summary>
        /// 演示5：迷宫求解
        /// </summary>
        private static void Demo5_MazeSolving()
        {
            Console.WriteLine("【演示5：迷宫求解 (BFS保证最短路径)】");
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
            
            // 求解最短路径
            int startRow = 0, startCol = 0;
            int endRow = 5, endCol = 5;
            
            var shortestPath = solver.SolveMaze(startRow, startCol, endRow, endCol);
            
            Console.WriteLine($"从({startRow},{startCol})到({endRow},{endCol})的最短路径求解:");
            if (shortestPath.Count > 0)
            {
                Console.WriteLine($"找到最短路径! 路径长度: {shortestPath.Count - 1}步");
                Console.WriteLine("路径坐标:");
                for (int i = 0; i < shortestPath.Count; i++)
                {
                    Console.Write($"({shortestPath[i].row},{shortestPath[i].col})");
                    if (i < shortestPath.Count - 1) Console.Write(" → ");
                    if ((i + 1) % 8 == 0) Console.WriteLine(); // 每行显示8个坐标
                }
                Console.WriteLine();
                Console.WriteLine();
                
                Console.WriteLine("带最短路径的迷宫显示 (●=路径):");
                DisplayMazeWithPath(maze, shortestPath, startRow, startCol, endRow, endCol);
            }
            else
            {
                Console.WriteLine("迷宫无解!");
            }
            
            Console.WriteLine();
            
            // 查找所有最短路径
            var allShortestPaths = solver.FindAllShortestPaths(startRow, startCol, endRow, endCol);
            Console.WriteLine($"所有最短路径: 共找到{allShortestPaths.Count}条等长最短路径");
            
            if (allShortestPaths.Count > 0)
            {
                Console.WriteLine($"最短路径长度: {allShortestPaths[0].Count - 1}步");
                
                if (allShortestPaths.Count <= 3)
                {
                    Console.WriteLine("\n所有最短路径详情:");
                    for (int i = 0; i < allShortestPaths.Count; i++)
                    {
                        Console.WriteLine($"路径{i + 1}: ({allShortestPaths[i][0].Item1},{allShortestPaths[i][0].Item2}) → ... → " +
                                        $"({allShortestPaths[i].Last().Item1},{allShortestPaths[i].Last().Item2})");
                    }
                }
            }
            
            // 距离计算
            Console.WriteLine();
            var distances = solver.CalculateDistances(startRow, startCol);
            Console.WriteLine($"从起点({startRow},{startCol})到各位置的最短距离:");
            Console.WriteLine("距离矩阵 (∞表示不可达):");
            
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == 1)
                    {
                        Console.Write("■■ "); // 墙
                    }
                    else if (distances.ContainsKey((i, j)))
                    {
                        Console.Write($"{distances[(i, j)],2} ");
                    }
                    else
                    {
                        Console.Write("∞∞ ");
                    }
                }
                Console.WriteLine();
            }
            
            Console.WriteLine();
            Console.WriteLine("BFS在迷宫求解中的优势:");
            Console.WriteLine("- 保证找到最短路径");
            Console.WriteLine("- 可以找到所有最短路径");
            Console.WriteLine("- 适合无权图的最短路径问题");
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