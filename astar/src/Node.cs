using System;

namespace AStar
{
    /// <summary>
    /// A*算法中的节点类
    /// 表示网格中的一个位置点，包含位置信息和路径查找所需的各种值
    /// </summary>
    public class Node : IComparable<Node>
    {
        /// <summary>
        /// 节点在网格中的X坐标
        /// </summary>
        public int X { get; set; }
        
        /// <summary>
        /// 节点在网格中的Y坐标
        /// </summary>
        public int Y { get; set; }
        
        /// <summary>
        /// G值：从起始节点到当前节点的实际代价
        /// </summary>
        public float GCost { get; set; }
        
        /// <summary>
        /// H值：从当前节点到目标节点的启发式估计代价
        /// </summary>
        public float HCost { get; set; }
        
        /// <summary>
        /// F值：G值和H值的总和，用于节点优先级比较
        /// </summary>
        public float FCost => GCost + HCost;
        
        /// <summary>
        /// 父节点：用于回溯路径
        /// </summary>
        public Node Parent { get; set; }
        
        /// <summary>
        /// 节点是否为障碍物
        /// </summary>
        public bool IsObstacle { get; set; }
        
        /// <summary>
        /// 节点是否已被访问
        /// </summary>
        public bool IsVisited { get; set; }
        
        /// <summary>
        /// 节点是否在开放列表中
        /// </summary>
        public bool IsInOpenList { get; set; }
        
        /// <summary>
        /// 节点是否在关闭列表中
        /// </summary>
        public bool IsInClosedList { get; set; }
        
        /// <summary>
        /// 节点是否为起始节点
        /// </summary>
        public bool IsStart { get; set; }
        
        /// <summary>
        /// 节点是否为结束节点
        /// </summary>
        public bool IsEnd { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="isObstacle">是否为障碍物</param>
        public Node(int x, int y, bool isObstacle = false)
        {
            X = x;
            Y = y;
            IsObstacle = isObstacle;
            GCost = 0;
            HCost = 0;
            Parent = null;
            IsVisited = false;
            IsInOpenList = false;
            IsInClosedList = false;
        }
        
        /// <summary>
        /// 重置节点状态，用于重新开始寻路
        /// </summary>
        public void Reset()
        {
            GCost = 0;
            HCost = 0;
            Parent = null;
            IsVisited = false;
            IsInOpenList = false;
            IsInClosedList = false;
        }
        
        /// <summary>
        /// 计算到另一个节点的曼哈顿距离
        /// </summary>
        /// <param name="other">目标节点</param>
        /// <returns>曼哈顿距离</returns>
        public float ManhattanDistance(Node other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
        
        /// <summary>
        /// 计算到另一个节点的欧几里得距离
        /// </summary>
        /// <param name="other">目标节点</param>
        /// <returns>欧几里得距离</returns>
        public float EuclideanDistance(Node other)
        {
            float dx = X - other.X;
            float dy = Y - other.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        
        /// <summary>
        /// 计算到另一个节点的对角距离
        /// </summary>
        /// <param name="other">目标节点</param>
        /// <returns>对角距离</returns>
        public float DiagonalDistance(Node other)
        {
            float dx = Math.Abs(X - other.X);
            float dy = Math.Abs(Y - other.Y);
            return Math.Max(dx, dy);
        }
        
        /// <summary>
        /// 实现IComparable接口，用于优先队列排序
        /// 按F值升序排列，F值相同时按H值升序排列
        /// </summary>
        /// <param name="other">比较的节点</param>
        /// <returns>比较结果</returns>
        public int CompareTo(Node other)
        {
            if (other == null) return 1;
            
            int fCompare = FCost.CompareTo(other.FCost);
            if (fCompare == 0)
            {
                return HCost.CompareTo(other.HCost);
            }
            return fCompare;
        }
        
        /// <summary>
        /// 重写Equals方法
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            if (obj is Node other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }
        
        /// <summary>
        /// 重写GetHashCode方法
        /// </summary>
        /// <returns>哈希码</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        
        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            return $"Node({X}, {Y}) - F:{FCost:F1} G:{GCost:F1} H:{HCost:F1}";
        }
    }
}