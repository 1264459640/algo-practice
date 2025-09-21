using System;
using AStar;

namespace AStarProgram
{
    public class TestAStar
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== A* 算法功能测试 ===");
            Console.WriteLine();

            // 创建一个10x10的网格
            var grid = new Grid(10, 10);
            
            // 添加一些障碍物
            grid.SetObstacle(3, 3, true);
            grid.SetObstacle(3, 4, true);
            grid.SetObstacle(3, 5, true);
            grid.SetObstacle(4, 3, true);
            grid.SetObstacle(5, 3, true);
            
            Console.WriteLine("网格创建完成，大小: 10x10");
            Console.WriteLine("障碍物位置: (3,3), (3,4), (3,5), (4,3), (5,3)");
            Console.WriteLine();
            
            // 测试基本寻路
            Console.WriteLine("测试1: 基本寻路 - 从(0,0)到(9,9)");
            var result1 = AStarPathfinder.FindPath(grid, 0, 0, 9, 9, HeuristicType.Manhattan);
            
            if (result1.PathFound)
            {
                Console.WriteLine($"✓ 路径找到！");
                Console.WriteLine($"  路径长度: {result1.PathLength}");
                Console.WriteLine($"  路径代价: {result1.PathCost:F2}");
                Console.WriteLine($"  执行时间: {result1.ExecutionTimeMs:F2}ms");
                Console.WriteLine($"  访问节点数: {result1.NodesVisited}");
                Console.WriteLine($"  搜索效率: {result1.CalculateSearchEfficiency():F2}%");
            }
            else
            {
                Console.WriteLine($"✗ 路径未找到: {result1.ErrorMessage}");
            }
            Console.WriteLine();
            
            // 测试不同启发式函数
            Console.WriteLine("测试2: 比较不同启发式函数");
            var heuristics = new[] { HeuristicType.Manhattan, HeuristicType.Euclidean, HeuristicType.Diagonal };
            
            foreach (var heuristic in heuristics)
            {
                var result = AStarPathfinder.FindPath(grid, 1, 1, 8, 8, heuristic);
                Console.WriteLine($"  {heuristic}: 路径长度={result.PathLength}, 代价={result.PathCost:F2}, 时间={result.ExecutionTimeMs:F2}ms");
            }
            Console.WriteLine();
            
            // 测试无路径情况
            Console.WriteLine("测试3: 无路径情况");
            // 创建完全被障碍物包围的目标点
            for (int x = 6; x <= 8; x++)
            {
                for (int y = 6; y <= 8; y++)
                {
                    if (x != 7 || y != 7) // 除了中心点
                    {
                        grid.SetObstacle(x, y, true);
                    }
                }
            }
            
            var result3 = AStarPathfinder.FindPath(grid, 0, 0, 7, 7, HeuristicType.Manhattan);
            if (result3.PathFound)
            {
                Console.WriteLine($"✓ 意外找到路径，长度: {result3.PathLength}");
            }
            else
            {
                Console.WriteLine($"✓ 正确识别无路径情况: {result3.ErrorMessage}");
            }
            Console.WriteLine();
            
            Console.WriteLine("=== 测试完成 ===");
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}