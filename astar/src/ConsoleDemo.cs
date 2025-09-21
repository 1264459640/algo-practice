using System;
using System.Collections.Generic;
using System.Linq;

namespace AStar
{
    /// <summary>
    /// A*算法控制台演示程序
    /// 提供交互式的A*寻路算法演示
    /// </summary>
    public class ConsoleDemo
    {
        private Grid _grid;
        private readonly Random _random = new Random();
        
        /// <summary>
        /// 运行演示程序
        /// </summary>
        public void Run()
        {
            Console.WriteLine("=== A* 寻路算法演示程序 ===");
            Console.WriteLine();
            
            // 初始化网格
            InitializeGrid();
            
            bool running = true;
            while (running)
            {
                ShowMenu();
                var choice = Console.ReadLine();
                
                switch (choice?.ToLower())
                {
                    case "1":
                        BasicPathfindingDemo();
                        break;
                    case "2":
                        CompareHeuristicsDemo();
                        break;
                    case "3":
                        MultiplePathsDemo();
                        break;
                    case "4":
                        RandomObstaclesDemo();
                        break;
                    case "5":
                        MazeDemo();
                        break;
                    case "6":
                        PerformanceTestDemo();
                        break;
                    case "7":
                        CustomMapDemo();
                        break;
                    case "8":
                        ShowGridVisualization();
                        break;
                    case "9":
                        InitializeGrid();
                        break;
                    case "0":
                    case "q":
                    case "quit":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("无效选择，请重试。");
                        break;
                }
                
                if (running)
                {
                    Console.WriteLine("\n按任意键继续...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            
            Console.WriteLine("感谢使用 A* 寻路算法演示程序！");
        }
        
        /// <summary>
        /// 显示主菜单
        /// </summary>
        private void ShowMenu()
        {
            Console.WriteLine("请选择演示功能：");
            Console.WriteLine("1. 基础寻路演示");
            Console.WriteLine("2. 启发式函数比较");
            Console.WriteLine("3. 多路径寻找");
            Console.WriteLine("4. 随机障碍物测试");
            Console.WriteLine("5. 迷宫寻路");
            Console.WriteLine("6. 性能测试");
            Console.WriteLine("7. 自定义地图");
            Console.WriteLine("8. 显示当前网格");
            Console.WriteLine("9. 重新初始化网格");
            Console.WriteLine("0. 退出程序");
            Console.Write("请输入选择 (0-9): ");
        }
        
        /// <summary>
        /// 初始化网格
        /// </summary>
        private void InitializeGrid()
        {
            Console.WriteLine("初始化网格...");
            Console.Write("请输入网格宽度 (默认20): ");
            var widthInput = Console.ReadLine();
            int width = string.IsNullOrEmpty(widthInput) ? 20 : int.TryParse(widthInput, out int w) ? w : 20;
            
            Console.Write("请输入网格高度 (默认15): ");
            var heightInput = Console.ReadLine();
            int height = string.IsNullOrEmpty(heightInput) ? 15 : int.TryParse(heightInput, out int h) ? h : 15;
            
            Console.Write("是否允许对角移动? (y/n, 默认y): ");
            var diagonalInput = Console.ReadLine()?.ToLower();
            bool allowDiagonal = diagonalInput != "n" && diagonalInput != "no";
            
            _grid = new Grid(width, height, allowDiagonal);
            
            Console.WriteLine($"网格已初始化: {width}x{height}, 对角移动: {(allowDiagonal ? "允许" : "禁止")}");
        }
        
        /// <summary>
        /// 基础寻路演示
        /// </summary>
        private void BasicPathfindingDemo()
        {
            Console.WriteLine("=== 基础寻路演示 ===");
            
            // 添加一些随机障碍物
            _grid.GenerateRandomObstacles(0.2f, _random);
            
            // 设置起点和终点
            int startX = 1, startY = 1;
            int endX = _grid.Width - 2, endY = _grid.Height - 2;
            
            Console.WriteLine($"起点: ({startX}, {startY})");
            Console.WriteLine($"终点: ({endX}, {endY})");
            Console.WriteLine("障碍物密度: 20%");
            
            // 执行寻路
            var result = AStarPathfinder.FindPath(_grid, startX, startY, endX, endY);
            
            // 显示结果
            Console.WriteLine("\n=== 寻路结果 ===");
            Console.WriteLine(result.ToString());
            Console.WriteLine();
            Console.WriteLine(result.GetPerformanceStats());
            
            if (result.PathFound)
            {
                Console.WriteLine();
                Console.WriteLine(result.GetPathDetails());
                
                // 可视化路径
                Console.WriteLine("\n=== 路径可视化 ===");
                VisualizePathOnGrid(result);
            }
        }
        
        /// <summary>
        /// 启发式函数比较演示
        /// </summary>
        private void CompareHeuristicsDemo()
        {
            Console.WriteLine("=== 启发式函数比较演示 ===");
            
            // 生成测试地图
            _grid.GenerateRandomObstacles(0.15f, _random);
            
            int startX = 2, startY = 2;
            int endX = _grid.Width - 3, endY = _grid.Height - 3;
            
            Console.WriteLine($"测试路径: ({startX}, {startY}) -> ({endX}, {endY})");
            Console.WriteLine("障碍物密度: 15%");
            Console.WriteLine();
            
            var heuristics = new[]
            {
                HeuristicType.Manhattan,
                HeuristicType.Euclidean,
                HeuristicType.Diagonal,
                HeuristicType.Chebyshev
            };
            
            Console.WriteLine("启发式函数比较结果:");
            Console.WriteLine("".PadRight(80, '-'));
            Console.WriteLine($"{"函数类型",-12} {"路径长度",-8} {"路径代价",-10} {"访问节点",-8} {"执行时间",-10} {"效率",-8}");
            Console.WriteLine("".PadRight(80, '-'));
            
            foreach (var heuristic in heuristics)
            {
                var result = AStarPathfinder.FindPath(_grid, startX, startY, endX, endY, heuristic);
                
                if (result.PathFound)
                {
                    Console.WriteLine($"{heuristic,-12} {result.PathLength,-8} {result.PathCost,-10:F2} {result.NodesVisited,-8} {result.ExecutionTimeMs,-10:F2} {result.CalculateSearchEfficiency(),-8:F3}");
                }
                else
                {
                    Console.WriteLine($"{heuristic,-12} {"失败",-8} {"N/A",-10} {result.NodesVisited,-8} {result.ExecutionTimeMs,-10:F2} {"N/A",-8}");
                }
            }
            
            Console.WriteLine("".PadRight(80, '-'));
        }
        
        /// <summary>
        /// 多路径寻找演示
        /// </summary>
        private void MultiplePathsDemo()
        {
            Console.WriteLine("=== 多路径寻找演示 ===");
            
            // 创建一个有多条可能路径的地图
            _grid.GenerateRandomObstacles(0.1f, _random);
            
            int startX = 1, startY = 1;
            int endX = _grid.Width - 2, endY = _grid.Height - 2;
            
            Console.WriteLine($"寻找从 ({startX}, {startY}) 到 ({endX}, {endY}) 的多条路径");
            Console.WriteLine();
            
            var results = AStarPathfinder.FindMultiplePaths(_grid, startX, startY, endX, endY, 3);
            
            Console.WriteLine($"找到 {results.Count} 条路径:");
            Console.WriteLine();
            
            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];
                Console.WriteLine($"路径 {i + 1}:");
                Console.WriteLine($"  长度: {result.PathLength} 节点");
                Console.WriteLine($"  代价: {result.PathCost:F2}");
                Console.WriteLine($"  访问节点: {result.NodesVisited}");
                Console.WriteLine($"  执行时间: {result.ExecutionTimeMs:F2} ms");
                Console.WriteLine();
            }
        }
        
        /// <summary>
        /// 随机障碍物测试演示
        /// </summary>
        private void RandomObstaclesDemo()
        {
            Console.WriteLine("=== 随机障碍物测试演示 ===");
            
            int startX = 0, startY = 0;
            int endX = _grid.Width - 1, endY = _grid.Height - 1;
            
            var obstacleRates = new[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f };
            
            Console.WriteLine($"测试路径: ({startX}, {startY}) -> ({endX}, {endY})");
            Console.WriteLine();
            Console.WriteLine("障碍物密度对寻路性能的影响:");
            Console.WriteLine("".PadRight(70, '-'));
            Console.WriteLine($"{"障碍物密度",-10} {"成功率",-8} {"平均路径长度",-12} {"平均访问节点",-12} {"平均时间",-10}");
            Console.WriteLine("".PadRight(70, '-'));
            
            foreach (var rate in obstacleRates)
            {
                int successCount = 0;
                float totalPathLength = 0;
                float totalNodesVisited = 0;
                float totalTime = 0;
                int testCount = 10;
                
                for (int i = 0; i < testCount; i++)
                {
                    _grid.GenerateRandomObstacles(rate, _random);
                    var result = AStarPathfinder.FindPath(_grid, startX, startY, endX, endY);
                    
                    if (result.PathFound)
                    {
                        successCount++;
                        totalPathLength += result.PathLength;
                    }
                    
                    totalNodesVisited += result.NodesVisited;
                    totalTime += (float)result.ExecutionTimeMs;
                }
                
                float successRate = (float)successCount / testCount * 100;
                float avgPathLength = successCount > 0 ? totalPathLength / successCount : 0;
                float avgNodesVisited = totalNodesVisited / testCount;
                float avgTime = totalTime / testCount;
                
                Console.WriteLine($"{rate * 100,-10:F0}% {successRate,-8:F1}% {avgPathLength,-12:F1} {avgNodesVisited,-12:F1} {avgTime,-10:F2}ms");
            }
            
            Console.WriteLine("".PadRight(70, '-'));
        }
        
        /// <summary>
        /// 迷宫演示
        /// </summary>
        private void MazeDemo()
        {
            Console.WriteLine("=== 迷宫寻路演示 ===");
            
            // 生成迷宫
            Console.WriteLine("生成迷宫...");
            _grid.GenerateMaze(_random);
            
            // 设置起点和终点
            int startX = 1, startY = 1;
            int endX = _grid.Width - 2, endY = _grid.Height - 2;
            
            // 确保起点和终点不是障碍物
            _grid.SetObstacle(startX, startY, false);
            _grid.SetObstacle(endX, endY, false);
            
            Console.WriteLine($"迷宫寻路: ({startX}, {startY}) -> ({endX}, {endY})");
            
            var result = AStarPathfinder.FindPath(_grid, startX, startY, endX, endY);
            
            Console.WriteLine("\n=== 迷宫寻路结果 ===");
            Console.WriteLine(result.ToString());
            Console.WriteLine();
            Console.WriteLine(result.GetPerformanceStats());
            
            if (result.PathFound)
            {
                Console.WriteLine("\n=== 迷宫路径可视化 ===");
                VisualizePathOnGrid(result);
            }
        }
        
        /// <summary>
        /// 性能测试演示
        /// </summary>
        private void PerformanceTestDemo()
        {
            Console.WriteLine("=== 性能测试演示 ===");
            
            var gridSizes = new[] { 
                new { Width = 20, Height = 20 },
                new { Width = 50, Height = 50 },
                new { Width = 100, Height = 100 }
            };
            
            Console.WriteLine("不同网格大小的性能测试:");
            Console.WriteLine("".PadRight(70, '-'));
            Console.WriteLine($"{"网格大小",-10} {"平均时间",-10} {"平均访问节点",-12} {"平均路径长度",-12} {"成功率",-8}");
            Console.WriteLine("".PadRight(70, '-'));
            
            foreach (var size in gridSizes)
            {
                var testGrid = new Grid(size.Width, size.Height, true);
                testGrid.GenerateRandomObstacles(0.2f, _random);
                
                int testCount = 5;
                int successCount = 0;
                float totalTime = 0;
                float totalNodesVisited = 0;
                float totalPathLength = 0;
                
                for (int i = 0; i < testCount; i++)
                {
                    int startX = _random.Next(size.Width / 4);
                    int startY = _random.Next(size.Height / 4);
                    int endX = size.Width - 1 - _random.Next(size.Width / 4);
                    int endY = size.Height - 1 - _random.Next(size.Height / 4);
                    
                    var result = AStarPathfinder.FindPath(testGrid, startX, startY, endX, endY);
                    
                    if (result.PathFound)
                    {
                        successCount++;
                        totalPathLength += result.PathLength;
                    }
                    
                    totalTime += (float)result.ExecutionTimeMs;
                    totalNodesVisited += result.NodesVisited;
                }
                
                float avgTime = totalTime / testCount;
                float avgNodesVisited = totalNodesVisited / testCount;
                float avgPathLength = successCount > 0 ? totalPathLength / successCount : 0;
                float successRate = (float)successCount / testCount * 100;
                
                Console.WriteLine($"{size.Width}x{size.Height,-6} {avgTime,-10:F2}ms {avgNodesVisited,-12:F0} {avgPathLength,-12:F1} {successRate,-8:F1}%");
            }
            
            Console.WriteLine("".PadRight(70, '-'));
        }
        
        /// <summary>
        /// 自定义地图演示
        /// </summary>
        private void CustomMapDemo()
        {
            Console.WriteLine("=== 自定义地图演示 ===");
            Console.WriteLine("在网格上手动放置障碍物");
            Console.WriteLine("输入坐标格式: x,y (例如: 5,3)");
            Console.WriteLine("输入 'done' 完成障碍物设置");
            Console.WriteLine("输入 'clear' 清除所有障碍物");
            Console.WriteLine("输入 'show' 显示当前网格");
            
            // 清除现有障碍物
            foreach (var node in _grid.GetAllNodes())
            {
                _grid.SetObstacle(node.X, node.Y, false);
            }
            
            while (true)
            {
                Console.Write("请输入障碍物坐标或命令: ");
                var input = Console.ReadLine()?.Trim().ToLower();
                
                if (input == "done")
                    break;
                
                if (input == "clear")
                {
                    foreach (var node in _grid.GetAllNodes())
                    {
                        _grid.SetObstacle(node.X, node.Y, false);
                    }
                    Console.WriteLine("已清除所有障碍物");
                    continue;
                }
                
                if (input == "show")
                {
                    ShowGridVisualization();
                    continue;
                }
                
                if (input?.Contains(',') == true)
                {
                    var parts = input.Split(',');
                    if (parts.Length == 2 && 
                        int.TryParse(parts[0].Trim(), out int x) && 
                        int.TryParse(parts[1].Trim(), out int y))
                    {
                        if (_grid.IsValidPosition(x, y))
                        {
                            _grid.SetObstacle(x, y, true);
                            Console.WriteLine($"在 ({x}, {y}) 放置障碍物");
                        }
                        else
                        {
                            Console.WriteLine($"坐标 ({x}, {y}) 超出网格范围");
                        }
                    }
                    else
                    {
                        Console.WriteLine("无效的坐标格式");
                    }
                }
                else
                {
                    Console.WriteLine("无效输入");
                }
            }
            
            // 获取起点和终点
            Console.Write("请输入起点坐标 (x,y): ");
            var startInput = Console.ReadLine()?.Split(',');
            Console.Write("请输入终点坐标 (x,y): ");
            var endInput = Console.ReadLine()?.Split(',');
            
            if (startInput?.Length == 2 && endInput?.Length == 2 &&
                int.TryParse(startInput[0].Trim(), out int startX) &&
                int.TryParse(startInput[1].Trim(), out int startY) &&
                int.TryParse(endInput[0].Trim(), out int endX) &&
                int.TryParse(endInput[1].Trim(), out int endY))
            {
                var result = AStarPathfinder.FindPath(_grid, startX, startY, endX, endY);
                
                Console.WriteLine("\n=== 自定义地图寻路结果 ===");
                Console.WriteLine(result.ToString());
                
                if (result.PathFound)
                {
                    Console.WriteLine("\n=== 路径可视化 ===");
                    VisualizePathOnGrid(result);
                }
            }
            else
            {
                Console.WriteLine("无效的坐标输入");
            }
        }
        
        /// <summary>
        /// 显示网格可视化
        /// </summary>
        private void ShowGridVisualization()
        {
            Console.WriteLine("=== 当前网格状态 ===");
            Console.WriteLine("符号说明: . = 空地, # = 障碍物");
            Console.WriteLine();
            
            // 显示列号
            Console.Write("   ");
            for (int x = 0; x < _grid.Width; x++)
            {
                Console.Write($"{x % 10}");
            }
            Console.WriteLine();
            
            // 显示网格
            for (int y = 0; y < _grid.Height; y++)
            {
                Console.Write($"{y,2} ");
                for (int x = 0; x < _grid.Width; x++)
                {
                    var node = _grid.GetNode(x, y);
                    Console.Write(node.IsObstacle ? '#' : '.');
                }
                Console.WriteLine();
            }
        }
        
        /// <summary>
        /// 在网格上可视化路径
        /// </summary>
        /// <param name="result">寻路结果</param>
        private void VisualizePathOnGrid(AStarResult result)
        {
            if (!result.PathFound || result.Path == null)
            {
                Console.WriteLine("没有路径可显示");
                return;
            }
            
            Console.WriteLine("符号说明: . = 空地, # = 障碍物, * = 路径, S = 起点, E = 终点, o = 访问过的节点");
            Console.WriteLine();
            
            // 创建显示字符数组
            char[,] display = new char[_grid.Width, _grid.Height];
            
            // 初始化显示数组
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    var node = _grid.GetNode(x, y);
                    display[x, y] = node.IsObstacle ? '#' : '.';
                }
            }
            
            // 标记访问过的节点
            foreach (var node in result.VisitedNodes)
            {
                if (!node.IsObstacle)
                {
                    display[node.X, node.Y] = 'o';
                }
            }
            
            // 标记路径
            foreach (var node in result.Path)
            {
                display[node.X, node.Y] = '*';
            }
            
            // 标记起点和终点
            if (result.Path.Count > 0)
            {
                var start = result.Path[0];
                var end = result.Path[result.Path.Count - 1];
                display[start.X, start.Y] = 'S';
                display[end.X, end.Y] = 'E';
            }
            
            // 显示列号
            Console.Write("   ");
            for (int x = 0; x < _grid.Width; x++)
            {
                Console.Write($"{x % 10}");
            }
            Console.WriteLine();
            
            // 显示网格
            for (int y = 0; y < _grid.Height; y++)
            {
                Console.Write($"{y,2} ");
                for (int x = 0; x < _grid.Width; x++)
                {
                    Console.Write(display[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}