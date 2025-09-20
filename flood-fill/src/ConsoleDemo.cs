using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FloodFillAlgorithm;

namespace FloodFill;

/// <summary>
/// 泛洪算法的控制台演示程序
/// </summary>
public class ConsoleDemo
{
    private static readonly Random Random = new();

    /// <summary>
    /// 运行所有演示
    /// </summary>
    public void RunAllDemos()
    {
        Console.WriteLine("=== 泛洪算法 (Flood Fill) 演示程序 ===\n");

        try
        {
            // 演示基本泛洪填充
            DemoBasicFloodFill();
            
            Console.WriteLine("\n" + new string('=', 50) + "\n");
            
            // 演示不同算法比较
            DemoAlgorithmComparison();
            
            Console.WriteLine("\n" + new string('=', 50) + "\n");
            
            // 演示连通性差异
            DemoConnectivityDifference();
            
            Console.WriteLine("\n" + new string('=', 50) + "\n");
            
            // 演示连通区域查找
            DemoConnectedRegions();
            
            Console.WriteLine("\n" + new string('=', 50) + "\n");
            
            // 演示图像处理应用
            DemoImageProcessing();
            
            Console.WriteLine("\n" + new string('=', 50) + "\n");
            
            // 性能测试
            PerformanceTest();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"演示过程中发生错误: {ex.Message}");
        }
    }

    public static void Main(string[] args)
    {
        var demo = new ConsoleDemo();
        demo.RunAllDemos();

        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }

    /// <summary>
    /// 演示基本的泛洪填充功能
    /// </summary>
    private static void DemoBasicFloodFill()
    {
        Console.WriteLine("1. 基本泛洪填充演示");
        Console.WriteLine("创建一个简单的网格并进行填充:");

        // 创建测试网格
        var data = new int[,]
        {
            {1, 1, 0, 0, 2},
            {1, 1, 0, 2, 2},
            {0, 0, 0, 2, 2},
            {3, 3, 0, 0, 0},
            {3, 3, 3, 0, 1}
        };

        var grid = new Grid(data);
        Console.WriteLine("原始网格:");
        Console.WriteLine(grid);

        // 执行泛洪填充
        var floodFill = new FloodFillAlgorithm.FloodFill();
        var result = floodFill.FillWithQueue(grid, 2, 2, 9);
        
        Console.WriteLine($"\n从位置 (2,2) 开始，将值 {result.OriginalValue} 填充为 {result.NewValue}:");
        Console.WriteLine(result.FilledGrid);
        
        Console.WriteLine($"填充统计: {result.GetPerformanceInfo()}");
        Console.WriteLine($"区域信息: {result.GetRegionInfo()}");
    }

    /// <summary>
    /// 演示不同算法的比较
    /// </summary>
    private static void DemoAlgorithmComparison()
    {
        Console.WriteLine("2. 不同算法实现比较");
        
        // 创建较大的测试网格
        var grid = new Grid(10, 10);
        grid.RandomFill(0, 3, 42); // 使用固定种子确保可重现
        
        Console.WriteLine("测试网格 (10x10):");
        Console.WriteLine(grid);

        var startRow = 5;
        var startCol = 5;
        var newValue = 9;

        Console.WriteLine($"\n从位置 ({startRow},{startCol}) 开始填充为值 {newValue}:\n");

        // 创建FloodFill实例
        var floodFill = new FloodFillAlgorithm.FloodFill();

        // 测试递归算法
        var recursiveResult = floodFill.FillRecursive(grid, startRow, startCol, newValue);
        Console.WriteLine($"递归算法: {recursiveResult.GetPerformanceInfo()}");

        // 测试栈算法
        var stackResult = floodFill.FillWithStack(grid, startRow, startCol, newValue);
        Console.WriteLine($"栈算法:   {stackResult.GetPerformanceInfo()}");

        // 测试队列算法
        var queueResult = floodFill.FillWithQueue(grid, startRow, startCol, newValue);
        Console.WriteLine($"队列算法: {queueResult.GetPerformanceInfo()}");

        Console.WriteLine($"\n所有算法填充的像素数量相同: {recursiveResult.FilledCount == stackResult.FilledCount && stackResult.FilledCount == queueResult.FilledCount}");
    }

    /// <summary>
    /// 演示4连通和8连通的差异
    /// </summary>
    private static void DemoConnectivityDifference()
    {
        Console.WriteLine("3. 连通性差异演示 (4连通 vs 8连通)");
        
        // 创建包含对角线连接的网格
        var data = new int[,]
        {
            {0, 1, 0, 0, 0},
            {1, 0, 1, 0, 0},
            {0, 1, 0, 1, 0},
            {0, 0, 1, 0, 1},
            {0, 0, 0, 1, 0}
        };

        var grid = new Grid(data);
        Console.WriteLine("原始网格 (对角线模式):");
        Console.WriteLine(grid);

        // 创建FloodFill实例
        var floodFill = new FloodFillAlgorithm.FloodFill();

        // 4连通填充
        var result4 = floodFill.FillWithQueue(grid, 0, 0, 9, ConnectivityType.FourConnected);
        Console.WriteLine("\n4连通填充结果:");
        Console.WriteLine(result4.FilledGrid);
        Console.WriteLine($"填充像素数: {result4.FilledCount}");

        // 8连通填充
        var result8 = floodFill.FillWithQueue(grid, 0, 0, 9, ConnectivityType.EightConnected);
        Console.WriteLine("\n8连通填充结果:");
        Console.WriteLine(result8.FilledGrid);
        Console.WriteLine($"填充像素数: {result8.FilledCount}");
    }

    /// <summary>
    /// 演示连通区域查找
    /// </summary>
    private static void DemoConnectedRegions()
    {
        Console.WriteLine("4. 连通区域查找演示");
        
        var data = new int[,]
        {
            {1, 1, 0, 2, 2},
            {1, 0, 0, 2, 0},
            {0, 0, 3, 0, 0},
            {4, 4, 3, 3, 5},
            {4, 0, 0, 5, 5}
        };

        var grid = new Grid(data);
        Console.WriteLine("原始网格:");
        Console.WriteLine(grid);

        // 创建FloodFill实例
        var floodFill = new FloodFillAlgorithm.FloodFill();
        
        // 查找所有连通区域
        var regions = floodFill.FindAllConnectedRegions(grid);
        
        Console.WriteLine($"\n找到 {regions.Count} 个连通区域:");
        
        for (int i = 0; i < regions.Count; i++)
        {
            var region = regions[i];
            var value = grid[region[0].row, region[0].col];
            Console.WriteLine($"区域 {i + 1}: 值={value}, 大小={region.Count}, 位置={string.Join(", ", region.Take(5).Select(p => $"({p.row},{p.col})"))}");
        }

        // 演示单个区域查找
        Console.WriteLine($"\n从位置 (0,0) 查找连通区域:");
        var singleRegion = floodFill.FindConnectedRegion(grid, 0, 0);
        Console.WriteLine($"区域大小: {singleRegion.Count}");
        Console.WriteLine($"区域位置: {string.Join(", ", singleRegion.Select(p => $"({p.row},{p.col})"))}");
    }

    /// <summary>
    /// 演示图像处理应用
    /// </summary>
    private static void DemoImageProcessing()
    {
        Console.WriteLine("5. 图像处理应用演示");
        Console.WriteLine("模拟简单的图像分割和区域标记:");

        // 创建模拟图像（0=背景，1=前景对象）
        var image = new Grid(8, 8);
        var imageData = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 1, 1, 0, 0, 1, 1, 0},
            {0, 1, 1, 0, 0, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 1, 1, 1, 0, 0, 0},
            {0, 0, 1, 1, 1, 0, 0, 0},
            {0, 0, 1, 1, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };
        
        image = new Grid(imageData);
        Console.WriteLine("原始图像 (0=背景, 1=前景):");
        Console.WriteLine(image);

        // 创建FloodFill实例
        var floodFill = new FloodFillAlgorithm.FloodFill();
        
        // 查找所有前景对象
        var objects = floodFill.FindAllConnectedRegions(image)
            .Where(region => image[region[0].row, region[0].col] == 1)
            .ToList();

        Console.WriteLine($"\n检测到 {objects.Count} 个前景对象:");
        
        // 为每个对象分配不同的标签
        var labeledImage = image.Clone();
        int label = 2; // 从2开始标记（0=背景，1=原始前景）
        
        foreach (var obj in objects)
        {
            Console.WriteLine($"对象 {label - 1}: 大小={obj.Count}, 边界框计算中...");
            
            // 使用泛洪填充为对象分配标签
            var result = floodFill.FillWithQueue(labeledImage, obj[0].row, obj[0].col, label);
            Console.WriteLine($"  {result.GetRegionInfo()}");
            
            label++;
        }

        Console.WriteLine("\n标记后的图像 (不同数字代表不同对象):");
        Console.WriteLine(labeledImage);
    }

    /// <summary>
    /// 性能测试
    /// </summary>
    private static void PerformanceTest()
    {
        Console.WriteLine("6. 性能测试");
        
        var sizes = new[] { 50, 100, 200 };
        var algorithms = new[]
        {
            (FloodFillAlgorithm.FloodFillAlgorithm.Recursive, "递归"),
            (FloodFillAlgorithm.FloodFillAlgorithm.Stack, "栈"),
            (FloodFillAlgorithm.FloodFillAlgorithm.Queue, "队列")
        };

        Console.WriteLine("网格大小 | 算法类型 | 填充像素 | 执行时间(ms) | 平均时间(μs/像素)");
        Console.WriteLine(new string('-', 70));

        foreach (var size in sizes)
        {
            var grid = new Grid(size, size);
            grid.RandomFill(0, 5, 123); // 固定种子
            
            var startRow = size / 2;
            var startCol = size / 2;
            var newValue = 99;

            foreach (var (algorithm, name) in algorithms)
            {
                try
                {
                    // 创建FloodFill实例
                    var floodFill = new FloodFillAlgorithm.FloodFill();
                    
                    FloodFillAlgorithm.FloodFillResult result = algorithm switch
                    {
                        FloodFillAlgorithm.FloodFillAlgorithm.Recursive => floodFill.FillRecursive(grid, startRow, startCol, newValue),
                        FloodFillAlgorithm.FloodFillAlgorithm.Stack => floodFill.FillWithStack(grid, startRow, startCol, newValue),
                        FloodFillAlgorithm.FloodFillAlgorithm.Queue => floodFill.FillWithQueue(grid, startRow, startCol, newValue),
                        _ => throw new ArgumentException("未知算法类型")
                    };

                    var avgTimePerPixel = result.FilledCount > 0 ? (result.ExecutionTimeMs * 1000) / result.FilledCount : 0;
                    Console.WriteLine($"{size,8}x{size,-3} | {name,6} | {result.FilledCount,8} | {result.ExecutionTimeMs,11:F2} | {avgTimePerPixel,13:F2}");
                }
                catch (StackOverflowException)
                {
                    Console.WriteLine($"{size,8}x{size,-3} | {name,6} | {"栈溢出",8} | {"N/A",11} | {"N/A",13}");
                }
            }
            
            if (size < sizes.Last())
                Console.WriteLine(new string('-', 70));
        }
    }
}