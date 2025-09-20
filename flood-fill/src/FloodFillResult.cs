using System;
using System.Collections.Generic;
using System.Linq;

namespace FloodFillAlgorithm
{

/// <summary>
/// 泛洪算法的执行结果
/// </summary>
public class FloodFillResult
{
    /// <summary>
    /// 填充后的网格
    /// </summary>
    public Grid FilledGrid { get; }

    /// <summary>
    /// 被填充的位置列表
    /// </summary>
    public List<(int row, int col)> FilledPositions { get; }

    /// <summary>
    /// 填充的像素数量
    /// </summary>
    public int FilledCount => FilledPositions.Count;

    /// <summary>
    /// 起始位置
    /// </summary>
    public (int row, int col) StartPosition { get; }

    /// <summary>
    /// 原始颜色/值
    /// </summary>
    public int OriginalValue { get; }

    /// <summary>
    /// 新颜色/值
    /// </summary>
    public int NewValue { get; }

    /// <summary>
    /// 算法执行时间（毫秒）
    /// </summary>
    public double ExecutionTimeMs { get; }

    /// <summary>
    /// 使用的连通性类型（4连通或8连通）
    /// </summary>
    public ConnectivityType Connectivity { get; }

    /// <summary>
    /// 使用的算法类型
    /// </summary>
    public FloodFillAlgorithm Algorithm { get; }

    /// <summary>
    /// 连通区域的边界框
    /// </summary>
    public BoundingBox BoundingBox { get; }

    /// <summary>
    /// 初始化泛洪算法结果
    /// </summary>
    public FloodFillResult(
        Grid filledGrid,
        List<(int row, int col)> filledPositions,
        (int row, int col) startPosition,
        int originalValue,
        int newValue,
        double executionTimeMs,
        ConnectivityType connectivity,
        FloodFillAlgorithm algorithm)
    {
        FilledGrid = filledGrid ?? throw new ArgumentNullException(nameof(filledGrid));
        FilledPositions = filledPositions ?? throw new ArgumentNullException(nameof(filledPositions));
        StartPosition = startPosition;
        OriginalValue = originalValue;
        NewValue = newValue;
        ExecutionTimeMs = executionTimeMs;
        Connectivity = connectivity;
        Algorithm = algorithm;
        BoundingBox = CalculateBoundingBox(filledPositions);
    }

    /// <summary>
    /// 计算填充区域的边界框
    /// </summary>
    private BoundingBox CalculateBoundingBox(List<(int row, int col)> positions)
    {
        if (positions.Count == 0)
            return new BoundingBox(0, 0, 0, 0);

        int minRow = int.MaxValue, maxRow = int.MinValue;
        int minCol = int.MaxValue, maxCol = int.MinValue;

        foreach (var (row, col) in positions)
        {
            minRow = Math.Min(minRow, row);
            maxRow = Math.Max(maxRow, row);
            minCol = Math.Min(minCol, col);
            maxCol = Math.Max(maxCol, col);
        }

        return new BoundingBox(minRow, minCol, maxRow - minRow + 1, maxCol - minCol + 1);
    }

    /// <summary>
    /// 获取填充区域的周长（边界像素数）
    /// </summary>
    public int GetPerimeter()
    {
        var filledSet = new HashSet<(int, int)>(FilledPositions);
        int perimeter = 0;

        foreach (var (row, col) in FilledPositions)
        {
            var neighbors = Connectivity == ConnectivityType.FourConnected
                ? FilledGrid.GetNeighbors4(row, col)
                : FilledGrid.GetNeighbors8(row, col);

            foreach (var neighbor in neighbors)
            {
                if (!filledSet.Contains(neighbor))
                {
                    perimeter++;
                    break; // 只要有一个邻居不在填充区域内，就算边界
                }
            }
        }

        return perimeter;
    }

    /// <summary>
    /// 检查指定位置是否被填充
    /// </summary>
    public bool IsPositionFilled(int row, int col)
    {
        return FilledPositions.Contains((row, col));
    }

    /// <summary>
    /// 获取算法性能统计信息
    /// </summary>
    public string GetPerformanceInfo()
    {
        return $"算法: {Algorithm}, 连通性: {Connectivity}, " +
               $"填充像素: {FilledCount}, 执行时间: {ExecutionTimeMs:F2}ms, " +
               $"平均每像素: {(FilledCount > 0 ? ExecutionTimeMs / FilledCount : 0):F4}ms";
    }

    /// <summary>
    /// 获取区域统计信息
    /// </summary>
    public string GetRegionInfo()
    {
        var perimeter = GetPerimeter();
        var area = FilledCount;
        var compactness = area > 0 ? (4 * Math.PI * area) / (perimeter * perimeter) : 0;

        return $"区域面积: {area}, 周长: {perimeter}, 紧凑度: {compactness:F3}, " +
               $"边界框: {BoundingBox.Width}x{BoundingBox.Height}";
    }
}

/// <summary>
/// 连通性类型枚举
/// </summary>
public enum ConnectivityType
{
    /// <summary>4连通（上下左右）</summary>
    FourConnected,
    /// <summary>8连通（包括对角线）</summary>
    EightConnected
}

/// <summary>
/// 泛洪算法类型枚举
/// </summary>
public enum FloodFillAlgorithm
{
    /// <summary>递归实现</summary>
    Recursive,
    /// <summary>栈实现（深度优先）</summary>
    Stack,
    /// <summary>队列实现（广度优先）</summary>
    Queue
}

/// <summary>
/// 边界框结构
/// </summary>
public struct BoundingBox
{
    /// <summary>起始行</summary>
    public int Row { get; }
    /// <summary>起始列</summary>
    public int Col { get; }
    /// <summary>宽度</summary>
    public int Width { get; }
    /// <summary>高度</summary>
    public int Height { get; }

    public BoundingBox(int row, int col, int width, int height)
    {
        Row = row;
        Col = col;
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return $"({Row}, {Col}) {Width}x{Height}";
    }
}
}