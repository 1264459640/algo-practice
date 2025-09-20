using System;
using System.Collections.Generic;
using System.Text;

namespace FloodFillAlgorithm
{
    /// <summary>
    /// 表示一个二维网格，用于泛洪算法的数据结构
    /// </summary>
    public class Grid
    {
    private readonly int[,] _data;
    private readonly int _rows;
    private readonly int _cols;

    /// <summary>
    /// 获取网格的行数
    /// </summary>
    public int Rows => _rows;

    /// <summary>
    /// 获取网格的列数
    /// </summary>
    public int Cols => _cols;

    /// <summary>
    /// 获取网格的宽度（列数的别名）
    /// </summary>
    public int Width => _cols;

    /// <summary>
    /// 获取网格的高度（行数的别名）
    /// </summary>
    public int Height => _rows;

    /// <summary>
    /// 初始化指定大小的网格
    /// </summary>
    /// <param name="rows">行数</param>
    /// <param name="cols">列数</param>
    public Grid(int rows, int cols)
    {
        if (rows <= 0 || cols <= 0)
            throw new ArgumentException("网格大小必须大于0");

        _rows = rows;
        _cols = cols;
        _data = new int[rows, cols];
    }

    /// <summary>
    /// 使用二维数组初始化网格
    /// </summary>
    /// <param name="data">初始数据</param>
    public Grid(int[,] data)
    {
        _data = (int[,])data.Clone();
        _rows = data.GetLength(0);
        _cols = data.GetLength(1);
    }

    /// <summary>
    /// 获取或设置指定位置的值
    /// </summary>
    /// <param name="row">行索引</param>
    /// <param name="col">列索引</param>
    /// <returns>指定位置的值</returns>
    public int this[int row, int col]
    {
        get
        {
            if (!IsValidPosition(row, col))
                throw new IndexOutOfRangeException($"位置 ({row}, {col}) 超出网格范围");
            return _data[row, col];
        }
        set
        {
            if (!IsValidPosition(row, col))
                throw new IndexOutOfRangeException($"位置 ({row}, {col}) 超出网格范围");
            _data[row, col] = value;
        }
    }

    /// <summary>
    /// 检查指定位置是否在网格范围内
    /// </summary>
    /// <param name="row">行索引</param>
    /// <param name="col">列索引</param>
    /// <returns>如果位置有效返回true，否则返回false</returns>
    public bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < _rows && col >= 0 && col < _cols;
    }

    /// <summary>
    /// 获取指定位置的4连通邻居
    /// </summary>
    /// <param name="row">行索引</param>
    /// <param name="col">列索引</param>
    /// <returns>有效的邻居位置列表</returns>
    public List<(int row, int col)> GetNeighbors4(int row, int col)
    {
        var neighbors = new List<(int, int)>();
        var directions = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) }; // 上下左右

        foreach (var (dr, dc) in directions)
        {
            int newRow = row + dr;
            int newCol = col + dc;
            if (IsValidPosition(newRow, newCol))
            {
                neighbors.Add((newRow, newCol));
            }
        }

        return neighbors;
    }

    /// <summary>
    /// 获取指定位置的8连通邻居
    /// </summary>
    /// <param name="row">行索引</param>
    /// <param name="col">列索引</param>
    /// <returns>有效的邻居位置列表</returns>
    public List<(int row, int col)> GetNeighbors8(int row, int col)
    {
        var neighbors = new List<(int, int)>();
        var directions = new[]
        {
            (-1, -1), (-1, 0), (-1, 1),  // 上排
            (0, -1),           (0, 1),   // 左右
            (1, -1),  (1, 0),  (1, 1)    // 下排
        };

        foreach (var (dr, dc) in directions)
        {
            int newRow = row + dr;
            int newCol = col + dc;
            if (IsValidPosition(newRow, newCol))
            {
                neighbors.Add((newRow, newCol));
            }
        }

        return neighbors;
    }

    /// <summary>
    /// 创建网格的深拷贝
    /// </summary>
    /// <returns>新的网格实例</returns>
    public Grid Clone()
    {
        return new Grid(_data);
    }

    /// <summary>
    /// 填充整个网格为指定值
    /// </summary>
    /// <param name="value">填充值</param>
    public void Fill(int value)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                _data[i, j] = value;
            }
        }
    }

    /// <summary>
    /// 随机填充网格
    /// </summary>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值（不包含）</param>
    /// <param name="seed">随机种子</param>
    public void RandomFill(int minValue, int maxValue, int? seed = null)
    {
        var random = seed.HasValue ? new Random(seed.Value) : new Random();
        
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                _data[i, j] = random.Next(minValue, maxValue);
            }
        }
    }

    /// <summary>
    /// 将网格转换为字符串表示
    /// </summary>
    /// <returns>网格的字符串表示</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                sb.Append($"{_data[i, j],3}");
                if (j < _cols - 1) sb.Append(" ");
            }
            if (i < _rows - 1) sb.AppendLine();
        }
        return sb.ToString();
    }

    /// <summary>
    /// 获取网格的原始数据数组
    /// </summary>
    /// <returns>二维数组的拷贝</returns>
    public int[,] GetData()
    {
        return (int[,])_data.Clone();
    }
    }
}