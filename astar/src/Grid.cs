using System;
using System.Collections.Generic;
using System.Linq;

namespace AStar
{
    /// <summary>
    /// 网格类，用于管理A*算法的地图
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// 网格宽度
        /// </summary>
        public int Width { get; private set; }
        
        /// <summary>
        /// 网格高度
        /// </summary>
        public int Height { get; private set; }
        
        /// <summary>
        /// 节点数组
        /// </summary>
        private Node[,] _nodes;
        
        /// <summary>
        /// 是否允许对角移动
        /// </summary>
        public bool AllowDiagonal { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="width">网格宽度</param>
        /// <param name="height">网格高度</param>
        /// <param name="allowDiagonal">是否允许对角移动</param>
        public Grid(int width, int height, bool allowDiagonal = true)
        {
            Width = width;
            Height = height;
            AllowDiagonal = allowDiagonal;
            _nodes = new Node[width, height];
            
            InitializeNodes();
        }
        
        /// <summary>
        /// 初始化所有节点
        /// </summary>
        private void InitializeNodes()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _nodes[x, y] = new Node(x, y);
                }
            }
        }
        
        /// <summary>
        /// 获取指定位置的节点
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>节点，如果坐标无效则返回null</returns>
        public Node GetNode(int x, int y)
        {
            if (IsValidPosition(x, y))
            {
                return _nodes[x, y];
            }
            return null;
        }
        
        /// <summary>
        /// 设置指定位置的节点为障碍物
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="isObstacle">是否为障碍物</param>
        public void SetObstacle(int x, int y, bool isObstacle)
        {
            if (IsValidPosition(x, y))
            {
                _nodes[x, y].IsObstacle = isObstacle;
            }
        }
        
        /// <summary>
        /// 检查位置是否有效
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>是否有效</returns>
        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
        
        /// <summary>
        /// 检查节点是否可通行
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>是否可通行</returns>
        public bool IsWalkable(int x, int y)
        {
            if (!IsValidPosition(x, y))
                return false;
            
            return !_nodes[x, y].IsObstacle;
        }
        
        /// <summary>
        /// 获取节点的邻居节点列表
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <returns>邻居节点列表</returns>
        public List<Node> GetNeighbors(Node node)
        {
            var neighbors = new List<Node>();
            
            // 四个基本方向
            var directions = new[]
            {
                new { x = 0, y = 1 },   // 上
                new { x = 1, y = 0 },   // 右
                new { x = 0, y = -1 },  // 下
                new { x = -1, y = 0 }   // 左
            };
            
            // 对角方向
            var diagonalDirections = new[]
            {
                new { x = 1, y = 1 },   // 右上
                new { x = 1, y = -1 },  // 右下
                new { x = -1, y = -1 }, // 左下
                new { x = -1, y = 1 }   // 左上
            };
            
            // 添加基本方向的邻居
            foreach (var dir in directions)
            {
                int newX = node.X + dir.x;
                int newY = node.Y + dir.y;
                
                if (IsWalkable(newX, newY))
                {
                    neighbors.Add(_nodes[newX, newY]);
                }
            }
            
            // 如果允许对角移动，添加对角方向的邻居
            if (AllowDiagonal)
            {
                foreach (var dir in diagonalDirections)
                {
                    int newX = node.X + dir.x;
                    int newY = node.Y + dir.y;
                    
                    if (IsWalkable(newX, newY))
                    {
                        // 检查对角移动是否被阻挡
                        bool horizontalBlocked = !IsWalkable(node.X + dir.x, node.Y);
                        bool verticalBlocked = !IsWalkable(node.X, node.Y + dir.y);
                        
                        // 如果两个相邻的直线方向都被阻挡，则不允许对角移动
                        if (!(horizontalBlocked && verticalBlocked))
                        {
                            neighbors.Add(_nodes[newX, newY]);
                        }
                    }
                }
            }
            
            return neighbors;
        }
        
        /// <summary>
        /// 计算两个节点之间的移动代价
        /// </summary>
        /// <param name="from">起始节点</param>
        /// <param name="to">目标节点</param>
        /// <returns>移动代价</returns>
        public float GetMoveCost(Node from, Node to)
        {
            int dx = Math.Abs(from.X - to.X);
            int dy = Math.Abs(from.Y - to.Y);
            
            // 对角移动的代价约为1.414（√2）
            if (dx == 1 && dy == 1)
            {
                return 1.414f;
            }
            
            // 直线移动的代价为1
            return 1.0f;
        }
        
        /// <summary>
        /// 重置所有节点的状态
        /// </summary>
        public void ResetNodes()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _nodes[x, y].Reset();
                }
            }
        }
        
        /// <summary>
        /// 随机生成障碍物
        /// </summary>
        /// <param name="obstaclePercentage">障碍物百分比（0-1）</param>
        /// <param name="random">随机数生成器</param>
        public void GenerateRandomObstacles(float obstaclePercentage, Random random = null)
        {
            if (random == null)
                random = new Random();
            
            int totalNodes = Width * Height;
            int obstacleCount = (int)(totalNodes * obstaclePercentage);
            
            // 清除现有障碍物
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _nodes[x, y].IsObstacle = false;
                }
            }
            
            // 随机放置障碍物
            for (int i = 0; i < obstacleCount; i++)
            {
                int x = random.Next(Width);
                int y = random.Next(Height);
                
                _nodes[x, y].IsObstacle = true;
            }
        }
        
        /// <summary>
        /// 创建迷宫样式的地图
        /// </summary>
        /// <param name="random">随机数生成器</param>
        public void GenerateMaze(Random random = null)
        {
            if (random == null)
                random = new Random();
            
            // 首先将所有节点设为障碍物
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _nodes[x, y].IsObstacle = true;
                }
            }
            
            // 使用递归回溯算法生成迷宫
            var stack = new Stack<Node>();
            var start = _nodes[1, 1];
            start.IsObstacle = false;
            start.IsVisited = true;
            stack.Push(start);
            
            while (stack.Count > 0)
            {
                var current = stack.Peek();
                var unvisitedNeighbors = GetUnvisitedMazeNeighbors(current);
                
                if (unvisitedNeighbors.Count > 0)
                {
                    var next = unvisitedNeighbors[random.Next(unvisitedNeighbors.Count)];
                    
                    // 移除当前节点和选中邻居之间的墙
                    int wallX = (current.X + next.X) / 2;
                    int wallY = (current.Y + next.Y) / 2;
                    _nodes[wallX, wallY].IsObstacle = false;
                    
                    next.IsObstacle = false;
                    next.IsVisited = true;
                    stack.Push(next);
                }
                else
                {
                    stack.Pop();
                }
            }
            
            // 重置访问状态
            ResetNodes();
        }
        
        /// <summary>
        /// 获取迷宫生成中未访问的邻居节点
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <returns>未访问的邻居节点列表</returns>
        private List<Node> GetUnvisitedMazeNeighbors(Node node)
        {
            var neighbors = new List<Node>();
            var directions = new[]
            {
                new { x = 0, y = 2 },   // 上
                new { x = 2, y = 0 },   // 右
                new { x = 0, y = -2 },  // 下
                new { x = -2, y = 0 }   // 左
            };
            
            foreach (var dir in directions)
            {
                int newX = node.X + dir.x;
                int newY = node.Y + dir.y;
                
                if (IsValidPosition(newX, newY) && !_nodes[newX, newY].IsVisited)
                {
                    neighbors.Add(_nodes[newX, newY]);
                }
            }
            
            return neighbors;
        }
        
        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns>所有节点的枚举</returns>
        public IEnumerable<Node> GetAllNodes()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    yield return _nodes[x, y];
                }
            }
        }
    }
}