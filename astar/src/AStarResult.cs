using System;
using System.Collections.Generic;
using System.Linq;

namespace AStar
{
    /// <summary>
    /// A*算法结果类
    /// 包含寻路结果、路径信息和性能统计
    /// </summary>
    public class AStarResult
    {
        /// <summary>
        /// 是否找到路径
        /// </summary>
        public bool PathFound { get; set; }
        
        /// <summary>
        /// 找到的路径（节点列表）
        /// </summary>
        public List<Node> Path { get; set; }
        
        /// <summary>
        /// 路径长度（节点数量）
        /// </summary>
        public int PathLength => Path?.Count ?? 0;
        
        /// <summary>
        /// 路径总代价
        /// </summary>
        public float PathCost { get; set; }
        
        /// <summary>
        /// 搜索过程中访问的节点列表（按访问顺序）
        /// </summary>
        public List<Node> VisitedNodes { get; set; }
        
        /// <summary>
        /// 访问的节点数量
        /// </summary>
        public int NodesVisited => VisitedNodes?.Count ?? 0;
        
        /// <summary>
        /// 开放列表中的最大节点数量
        /// </summary>
        public int MaxOpenListSize { get; set; }
        
        /// <summary>
        /// 算法执行时间（毫秒）
        /// </summary>
        public double ExecutionTimeMs { get; set; }
        
        /// <summary>
        /// 起始节点
        /// </summary>
        public Node StartNode { get; set; }
        
        /// <summary>
        /// 目标节点
        /// </summary>
        public Node EndNode { get; set; }
        
        /// <summary>
        /// 使用的启发式函数类型
        /// </summary>
        public HeuristicType HeuristicUsed { get; set; }
        
        /// <summary>
        /// 是否允许对角移动
        /// </summary>
        public bool DiagonalMovementAllowed { get; set; }
        
        /// <summary>
        /// 错误信息（如果有）
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AStarResult()
        {
            Path = new List<Node>();
            VisitedNodes = new List<Node>();
            PathFound = false;
            PathCost = 0;
            MaxOpenListSize = 0;
            ExecutionTimeMs = 0;
            ErrorMessage = string.Empty;
        }
        
        /// <summary>
        /// 计算路径的实际代价
        /// </summary>
        /// <returns>路径总代价</returns>
        public float CalculateActualPathCost()
        {
            if (Path == null || Path.Count < 2)
                return 0;
            
            float totalCost = 0;
            for (int i = 0; i < Path.Count - 1; i++)
            {
                var current = Path[i];
                var next = Path[i + 1];
                
                // 计算移动代价
                int dx = Math.Abs(current.X - next.X);
                int dy = Math.Abs(current.Y - next.Y);
                
                if (dx == 1 && dy == 1)
                {
                    totalCost += 1.414f; // 对角移动
                }
                else
                {
                    totalCost += 1.0f; // 直线移动
                }
            }
            
            return totalCost;
        }
        
        /// <summary>
        /// 获取路径的坐标点列表
        /// </summary>
        /// <returns>坐标点列表</returns>
        public List<(int X, int Y)> GetPathCoordinates()
        {
            return Path?.Select(node => (node.X, node.Y)).ToList() ?? new List<(int, int)>();
        }
        
        /// <summary>
        /// 获取访问节点的坐标点列表
        /// </summary>
        /// <returns>坐标点列表</returns>
        public List<(int X, int Y)> GetVisitedCoordinates()
        {
            return VisitedNodes?.Select(node => (node.X, node.Y)).ToList() ?? new List<(int, int)>();
        }
        
        /// <summary>
        /// 计算搜索效率（找到的路径长度 / 访问的节点数量）
        /// </summary>
        /// <returns>搜索效率</returns>
        public float CalculateSearchEfficiency()
        {
            if (NodesVisited == 0 || !PathFound)
                return 0;
            
            return (float)PathLength / NodesVisited;
        }
        
        /// <summary>
        /// 获取性能统计信息
        /// </summary>
        /// <returns>性能统计字符串</returns>
        public string GetPerformanceStats()
        {
            var stats = new List<string>();
            
            stats.Add($"路径找到: {(PathFound ? "是" : "否")}");
            
            if (PathFound)
            {
                stats.Add($"路径长度: {PathLength} 节点");
                stats.Add($"路径代价: {PathCost:F2}");
                stats.Add($"实际代价: {CalculateActualPathCost():F2}");
            }
            
            stats.Add($"访问节点: {NodesVisited}");
            stats.Add($"最大开放列表: {MaxOpenListSize}");
            stats.Add($"执行时间: {ExecutionTimeMs:F2} ms");
            stats.Add($"启发式函数: {HeuristicUsed}");
            stats.Add($"对角移动: {(DiagonalMovementAllowed ? "允许" : "禁止")}");
            
            if (PathFound)
            {
                stats.Add($"搜索效率: {CalculateSearchEfficiency():F3}");
            }
            
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                stats.Add($"错误: {ErrorMessage}");
            }
            
            return string.Join("\n", stats);
        }
        
        /// <summary>
        /// 获取详细的路径信息
        /// </summary>
        /// <returns>路径信息字符串</returns>
        public string GetPathDetails()
        {
            if (!PathFound || Path == null || Path.Count == 0)
                return "未找到路径";
            
            var details = new List<string>();
            details.Add($"路径详情 (共 {PathLength} 步):");
            
            for (int i = 0; i < Path.Count; i++)
            {
                var node = Path[i];
                string step = $"  {i + 1}. ({node.X}, {node.Y})";
                
                if (i == 0)
                    step += " [起点]";
                else if (i == Path.Count - 1)
                    step += " [终点]";
                
                details.Add(step);
            }
            
            return string.Join("\n", details);
        }
        
        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>结果摘要</returns>
        public override string ToString()
        {
            if (PathFound)
            {
                return $"A*寻路成功: 路径长度={PathLength}, 代价={PathCost:F2}, 访问节点={NodesVisited}, 用时={ExecutionTimeMs:F2}ms";
            }
            else
            {
                return $"A*寻路失败: 访问节点={NodesVisited}, 用时={ExecutionTimeMs:F2}ms" + 
                       (string.IsNullOrEmpty(ErrorMessage) ? "" : $", 错误={ErrorMessage}");
            }
        }
    }
    
    /// <summary>
    /// 启发式函数类型枚举
    /// </summary>
    public enum HeuristicType
    {
        /// <summary>
        /// 曼哈顿距离（适用于四方向移动）
        /// </summary>
        Manhattan,
        
        /// <summary>
        /// 欧几里得距离（适用于任意方向移动）
        /// </summary>
        Euclidean,
        
        /// <summary>
        /// 对角距离（适用于八方向移动）
        /// </summary>
        Diagonal,
        
        /// <summary>
        /// 切比雪夫距离
        /// </summary>
        Chebyshev
    }
}