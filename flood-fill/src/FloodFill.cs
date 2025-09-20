using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FloodFillAlgorithm
{

/// <summary>
/// 泛洪算法的核心实现类
/// </summary>
public class FloodFill
{
    /// <summary>
    /// 递归实现的泛洪算法
    /// </summary>
    /// <param name="grid">要填充的网格</param>
    /// <param name="startRow">起始行</param>
    /// <param name="startCol">起始列</param>
    /// <param name="newValue">新值</param>
    /// <param name="connectivity">连通性类型</param>
    /// <returns>填充结果</returns>
    public FloodFillResult FillRecursive(Grid grid, int startRow, int startCol, int newValue, 
        ConnectivityType connectivity = ConnectivityType.FourConnected)
    {
        if (!grid.IsValidPosition(startRow, startCol))
            throw new ArgumentException($"起始位置 ({startRow}, {startCol}) 超出网格范围");

        var stopwatch = Stopwatch.StartNew();
        var workingGrid = grid.Clone();
        var filledPositions = new List<(int, int)>();
        var originalValue = workingGrid[startRow, startCol];

        // 如果新值和原值相同，不需要填充
        if (originalValue == newValue)
        {
            stopwatch.Stop();
            return new FloodFillResult(workingGrid, filledPositions, (startRow, startCol), 
                originalValue, newValue, stopwatch.Elapsed.TotalMilliseconds, 
                connectivity, FloodFillAlgorithm.Recursive);
        }

        FillRecursiveHelper(workingGrid, startRow, startCol, originalValue, newValue, 
            connectivity, filledPositions);

        stopwatch.Stop();
        return new FloodFillResult(workingGrid, filledPositions, (startRow, startCol), 
            originalValue, newValue, stopwatch.Elapsed.TotalMilliseconds, 
            connectivity, FloodFillAlgorithm.Recursive);
    }

    /// <summary>
    /// 递归填充的辅助方法
    /// </summary>
    private static void FillRecursiveHelper(Grid grid, int row, int col, int originalValue, int newValue,
        ConnectivityType connectivity, List<(int, int)> filledPositions)
    {
        // 边界检查
        if (!grid.IsValidPosition(row, col) || grid[row, col] != originalValue)
            return;

        // 填充当前位置
        grid[row, col] = newValue;
        filledPositions.Add((row, col));

        // 获取邻居并递归填充
        var neighbors = connectivity == ConnectivityType.FourConnected
            ? grid.GetNeighbors4(row, col)
            : grid.GetNeighbors8(row, col);

        foreach (var (neighborRow, neighborCol) in neighbors)
        {
            FillRecursiveHelper(grid, neighborRow, neighborCol, originalValue, newValue, 
                connectivity, filledPositions);
        }
    }

    /// <summary>
    /// 使用栈实现的泛洪算法
    /// </summary>
    /// <param name="grid">要填充的网格</param>
    /// <param name="startRow">起始行</param>
    /// <param name="startCol">起始列</param>
    /// <param name="newValue">新值</param>
    /// <param name="connectivity">连通性类型</param>
    /// <returns>填充结果</returns>
    public FloodFillResult FillWithStack(Grid grid, int startRow, int startCol, int newValue,
        ConnectivityType connectivity = ConnectivityType.FourConnected)
    {
        if (!grid.IsValidPosition(startRow, startCol))
            throw new ArgumentException($"起始位置 ({startRow}, {startCol}) 超出网格范围");

        var stopwatch = Stopwatch.StartNew();
        var workingGrid = grid.Clone();
        var filledPositions = new List<(int, int)>();
        var originalValue = workingGrid[startRow, startCol];

        // 如果新值和原值相同，不需要填充
        if (originalValue == newValue)
        {
            stopwatch.Stop();
            return new FloodFillResult(workingGrid, filledPositions, (startRow, startCol), 
                originalValue, newValue, stopwatch.Elapsed.TotalMilliseconds, 
                connectivity, FloodFillAlgorithm.Stack);
        }

        var stack = new Stack<(int row, int col)>();
        stack.Push((startRow, startCol));

        while (stack.Count > 0)
        {
            var (row, col) = stack.Pop();

            // 跳过无效位置或已经处理过的位置
            if (!workingGrid.IsValidPosition(row, col) || workingGrid[row, col] != originalValue)
                continue;

            // 填充当前位置
            workingGrid[row, col] = newValue;
            filledPositions.Add((row, col));

            // 将邻居加入栈
            var neighbors = connectivity == ConnectivityType.FourConnected
                ? workingGrid.GetNeighbors4(row, col)
                : workingGrid.GetNeighbors8(row, col);

            foreach (var neighbor in neighbors)
            {
                stack.Push(neighbor);
            }
        }

        stopwatch.Stop();
        return new FloodFillResult(workingGrid, filledPositions, (startRow, startCol), 
            originalValue, newValue, stopwatch.Elapsed.TotalMilliseconds, 
            connectivity, FloodFillAlgorithm.Stack);
    }

    /// <summary>
    /// 使用队列实现的泛洪算法
    /// </summary>
    /// <param name="grid">要填充的网格</param>
    /// <param name="startRow">起始行</param>
    /// <param name="startCol">起始列</param>
    /// <param name="newValue">新值</param>
    /// <param name="connectivity">连通性类型</param>
    /// <returns>填充结果</returns>
    public FloodFillResult FillWithQueue(Grid grid, int startRow, int startCol, int newValue,
        ConnectivityType connectivity = ConnectivityType.FourConnected)
    {
        if (!grid.IsValidPosition(startRow, startCol))
            throw new ArgumentException($"起始位置 ({startRow}, {startCol}) 超出网格范围");

        var stopwatch = Stopwatch.StartNew();
        var workingGrid = grid.Clone();
        var filledPositions = new List<(int, int)>();
        var originalValue = workingGrid[startRow, startCol];

        // 如果新值和原值相同，不需要填充
        if (originalValue == newValue)
        {
            stopwatch.Stop();
            return new FloodFillResult(workingGrid, filledPositions, (startRow, startCol), 
                originalValue, newValue, stopwatch.Elapsed.TotalMilliseconds, 
                connectivity, FloodFillAlgorithm.Queue);
        }

        var queue = new Queue<(int row, int col)>();
        var visited = new HashSet<(int, int)>();
        
        queue.Enqueue((startRow, startCol));
        visited.Add((startRow, startCol));

        while (queue.Count > 0)
        {
            var (row, col) = queue.Dequeue();

            // 跳过无效位置或值不匹配的位置
            if (!workingGrid.IsValidPosition(row, col) || workingGrid[row, col] != originalValue)
                continue;

            // 填充当前位置
            workingGrid[row, col] = newValue;
            filledPositions.Add((row, col));

            // 将未访问的邻居加入队列
            var neighbors = connectivity == ConnectivityType.FourConnected
                ? workingGrid.GetNeighbors4(row, col)
                : workingGrid.GetNeighbors8(row, col);

            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor) && workingGrid.IsValidPosition(neighbor.row, neighbor.col) 
                    && workingGrid[neighbor.row, neighbor.col] == originalValue)
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        stopwatch.Stop();
        return new FloodFillResult(workingGrid, filledPositions, (startRow, startCol), 
            originalValue, newValue, stopwatch.Elapsed.TotalMilliseconds, 
            connectivity, FloodFillAlgorithm.Queue);
    }

    /// <summary>
    /// 查找连通区域
    /// </summary>
    /// <param name="grid">网格</param>
    /// <param name="startRow">起始行</param>
    /// <param name="startCol">起始列</param>
    /// <param name="connectivity">连通性类型</param>
    /// <returns>连通区域的所有坐标</returns>
    public List<(int row, int col)> FindConnectedRegion(Grid grid, int startRow, int startCol,
        ConnectivityType connectivity = ConnectivityType.FourConnected)
    {
        if (!grid.IsValidPosition(startRow, startCol))
            throw new ArgumentException($"起始位置 ({startRow}, {startCol}) 超出网格范围");

        var region = new List<(int, int)>();
        var visited = new HashSet<(int, int)>();
        var targetValue = grid[startRow, startCol];
        var queue = new Queue<(int, int)>();

        queue.Enqueue((startRow, startCol));
        visited.Add((startRow, startCol));

        while (queue.Count > 0)
        {
            var (row, col) = queue.Dequeue();
            region.Add((row, col));

            var neighbors = connectivity == ConnectivityType.FourConnected
                ? grid.GetNeighbors4(row, col)
                : grid.GetNeighbors8(row, col);

            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor) && grid[neighbor.row, neighbor.col] == targetValue)
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return region;
    }

    /// <summary>
    /// 查找所有连通区域
    /// </summary>
    /// <param name="grid">网格</param>
    /// <param name="connectivity">连通性类型</param>
    /// <returns>所有连通区域的列表</returns>
    public List<List<(int row, int col)>> FindAllConnectedRegions(Grid grid,
        ConnectivityType connectivity = ConnectivityType.FourConnected)
    {
        var regions = new List<List<(int, int)>>();
        var globalVisited = new HashSet<(int, int)>();

        for (int row = 0; row < grid.Rows; row++)
        {
            for (int col = 0; col < grid.Cols; col++)
            {
                if (!globalVisited.Contains((row, col)))
                {
                    var region = FindConnectedRegion(grid, row, col, connectivity);
                    regions.Add(region);
                    
                    // 将这个区域的所有位置标记为已访问
                    foreach (var pos in region)
                    {
                        globalVisited.Add(pos);
                    }
                }
            }
        }

        return regions;
    }
}
}