using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AStar
{
    /// <summary>
    /// A*寻路算法实现类
    /// 提供多种启发式函数和配置选项的A*寻路算法
    /// </summary>
    public static class AStarPathfinder
    {
        /// <summary>
        /// 使用A*算法寻找路径
        /// </summary>
        /// <param name="grid">网格地图</param>
        /// <param name="startX">起始X坐标</param>
        /// <param name="startY">起始Y坐标</param>
        /// <param name="endX">目标X坐标</param>
        /// <param name="endY">目标Y坐标</param>
        /// <param name="heuristicType">启发式函数类型</param>
        /// <returns>寻路结果</returns>
        public static AStarResult FindPath(Grid grid, int startX, int startY, int endX, int endY, 
            HeuristicType heuristicType = HeuristicType.Manhattan)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new AStarResult
            {
                HeuristicUsed = heuristicType,
                DiagonalMovementAllowed = grid.AllowDiagonal
            };
            
            try
            {
                // 验证输入参数
                if (!grid.IsValidPosition(startX, startY))
                {
                    result.ErrorMessage = $"起始位置 ({startX}, {startY}) 无效";
                    return result;
                }
                
                if (!grid.IsValidPosition(endX, endY))
                {
                    result.ErrorMessage = $"目标位置 ({endX}, {endY}) 无效";
                    return result;
                }
                
                var startNode = grid.GetNode(startX, startY);
                var endNode = grid.GetNode(endX, endY);
                
                result.StartNode = startNode;
                result.EndNode = endNode;
                
                // 重置网格状态
                grid.ResetNodes();
                
                // 执行A*算法
                result = ExecuteAStar(grid, startNode, endNode, heuristicType, result);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"寻路过程中发生错误: {ex.Message}";
            }
            finally
            {
                stopwatch.Stop();
                result.ExecutionTimeMs = (float)stopwatch.Elapsed.TotalMilliseconds;
            }
            
            return result;
        }
        
        /// <summary>
        /// 使用A*算法寻找路径（接受Node参数）
        /// </summary>
        /// <param name="grid">网格地图</param>
        /// <param name="startNode">起始节点</param>
        /// <param name="endNode">目标节点</param>
        /// <param name="heuristicType">启发式函数类型</param>
        /// <param name="allowDiagonal">是否允许对角移动</param>
        /// <returns>寻路结果</returns>
        public static AStarResult FindPath(Grid grid, Node startNode, Node endNode, 
            HeuristicType heuristicType = HeuristicType.Manhattan, bool allowDiagonal = false)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new AStarResult
            {
                HeuristicUsed = heuristicType,
                DiagonalMovementAllowed = allowDiagonal
            };
            
            try
            {
                // 验证输入参数
                if (startNode == null)
                {
                    result.ErrorMessage = "起始节点不能为空";
                    return result;
                }
                
                if (endNode == null)
                {
                    result.ErrorMessage = "目标节点不能为空";
                    return result;
                }
                
                result.StartNode = startNode;
                result.EndNode = endNode;
                
                if (startNode.IsObstacle)
                {
                    result.ErrorMessage = $"起始位置 ({startNode.X}, {startNode.Y}) 是障碍物";
                    return result;
                }
                
                if (endNode.IsObstacle)
                {
                    result.ErrorMessage = $"目标位置 ({endNode.X}, {endNode.Y}) 是障碍物";
                    return result;
                }
                
                // 重置网格状态
                grid.ResetNodes();
                
                // 执行A*算法
                result = ExecuteAStar(grid, startNode, endNode, heuristicType, result);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"寻路过程中发生错误: {ex.Message}";
            }
            finally
            {
                stopwatch.Stop();
                result.ExecutionTimeMs = (float)stopwatch.Elapsed.TotalMilliseconds;
            }
            
            return result;
        }
        
        /// <summary>
        /// 执行A*算法的核心逻辑
        /// </summary>
        /// <param name="grid">网格</param>
        /// <param name="startNode">起始节点</param>
        /// <param name="endNode">目标节点</param>
        /// <param name="heuristicType">启发式函数类型</param>
        /// <param name="result">结果对象</param>
        /// <returns>寻路结果</returns>
        private static AStarResult ExecuteAStar(Grid grid, Node startNode, Node endNode, 
            HeuristicType heuristicType, AStarResult result)
        {
            // 开放列表：待处理的节点
            var openList = new SortedSet<Node>();
            
            // 关闭列表：已处理的节点
            var closedList = new HashSet<Node>();
            
            // 初始化起始节点
            startNode.GCost = 0;
            startNode.HCost = CalculateHeuristic(startNode, endNode, heuristicType);
            startNode.Parent = null;
            startNode.IsInOpenList = true;
            
            openList.Add(startNode);
            
            int maxOpenListSize = 1;
            
            while (openList.Count > 0)
            {
                // 更新最大开放列表大小
                maxOpenListSize = Math.Max(maxOpenListSize, openList.Count);
                
                // 获取F值最小的节点
                var currentNode = openList.Min;
                openList.Remove(currentNode);
                currentNode.IsInOpenList = false;
                
                // 将当前节点添加到关闭列表
                closedList.Add(currentNode);
                currentNode.IsInClosedList = true;
                result.VisitedNodes.Add(currentNode);
                
                // 检查是否到达目标
                if (currentNode.Equals(endNode))
                {
                    result.PathFound = true;
                    result.Path = ReconstructPath(currentNode);
                    result.PathCost = currentNode.GCost;
                    result.MaxOpenListSize = maxOpenListSize;
                    return result;
                }
                
                // 检查所有邻居节点
                var neighbors = grid.GetNeighbors(currentNode);
                foreach (var neighbor in neighbors)
                {
                    // 跳过已在关闭列表中的节点
                    if (closedList.Contains(neighbor))
                        continue;
                    
                    // 计算从起点到邻居节点的代价
                    float tentativeGCost = currentNode.GCost + grid.GetMoveCost(currentNode, neighbor);
                    
                    // 如果邻居节点不在开放列表中，或者找到了更好的路径
                    if (!neighbor.IsInOpenList || tentativeGCost < neighbor.GCost)
                    {
                        // 更新邻居节点的信息
                        neighbor.Parent = currentNode;
                        neighbor.GCost = tentativeGCost;
                        neighbor.HCost = CalculateHeuristic(neighbor, endNode, heuristicType);
                        
                        // 如果邻居节点不在开放列表中，添加它
                        if (!neighbor.IsInOpenList)
                        {
                            openList.Add(neighbor);
                            neighbor.IsInOpenList = true;
                        }
                        else
                        {
                            // 如果已在开放列表中，需要重新排序
                            // 由于SortedSet不支持直接更新，需要先移除再添加
                            openList.Remove(neighbor);
                            openList.Add(neighbor);
                        }
                    }
                }
            }
            
            // 没有找到路径
            result.PathFound = false;
            result.MaxOpenListSize = maxOpenListSize;
            result.ErrorMessage = "无法找到从起点到终点的路径";
            return result;
        }
        
        /// <summary>
        /// 计算启发式函数值
        /// </summary>
        /// <param name="from">起始节点</param>
        /// <param name="to">目标节点</param>
        /// <param name="heuristicType">启发式函数类型</param>
        /// <returns>启发式值</returns>
        private static float CalculateHeuristic(Node from, Node to, HeuristicType heuristicType)
        {
            return heuristicType switch
            {
                HeuristicType.Manhattan => from.ManhattanDistance(to),
                HeuristicType.Euclidean => from.EuclideanDistance(to),
                HeuristicType.Diagonal => from.DiagonalDistance(to),
                HeuristicType.Chebyshev => Math.Max(Math.Abs(from.X - to.X), Math.Abs(from.Y - to.Y)),
                _ => from.ManhattanDistance(to)
            };
        }
        
        /// <summary>
        /// 重构路径
        /// </summary>
        /// <param name="endNode">终点节点</param>
        /// <returns>路径节点列表</returns>
        private static List<Node> ReconstructPath(Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;
            
            while (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            
            // 反转路径，使其从起点到终点
            path.Reverse();
            return path;
        }
        
        /// <summary>
        /// 寻找多条路径（如果存在）
        /// </summary>
        /// <param name="grid">网格地图</param>
        /// <param name="startX">起始X坐标</param>
        /// <param name="startY">起始Y坐标</param>
        /// <param name="endX">目标X坐标</param>
        /// <param name="endY">目标Y坐标</param>
        /// <param name="maxPaths">最大路径数量</param>
        /// <param name="heuristicType">启发式函数类型</param>
        /// <returns>多条路径的结果列表</returns>
        public static List<AStarResult> FindMultiplePaths(Grid grid, int startX, int startY, int endX, int endY, 
            int maxPaths = 3, HeuristicType heuristicType = HeuristicType.Manhattan)
        {
            var results = new List<AStarResult>();
            var originalGrid = new Grid(grid.Width, grid.Height, grid.AllowDiagonal);
            
            // 复制原始网格状态
            foreach (var node in grid.GetAllNodes())
            {
                originalGrid.SetObstacle(node.X, node.Y, node.IsObstacle);
            }
            
            for (int i = 0; i < maxPaths; i++)
            {
                var result = FindPath(grid, startX, startY, endX, endY, heuristicType);
                
                if (!result.PathFound)
                    break;
                
                results.Add(result);
                
                // 如果这是最后一次迭代，不需要阻塞路径
                if (i == maxPaths - 1)
                    break;
                
                // 阻塞当前找到的路径（除了起点和终点）
                for (int j = 1; j < result.Path.Count - 1; j++)
                {
                    var pathNode = result.Path[j];
                    grid.SetObstacle(pathNode.X, pathNode.Y, true);
                }
            }
            
            // 恢复原始网格状态
            foreach (var node in originalGrid.GetAllNodes())
            {
                grid.SetObstacle(node.X, node.Y, node.IsObstacle);
            }
            
            return results;
        }
        
        /// <summary>
        /// 计算两点之间的直线距离（不考虑障碍物）
        /// </summary>
        /// <param name="x1">起始X坐标</param>
        /// <param name="y1">起始Y坐标</param>
        /// <param name="x2">目标X坐标</param>
        /// <param name="y2">目标Y坐标</param>
        /// <param name="heuristicType">距离计算类型</param>
        /// <returns>距离值</returns>
        public static float CalculateDistance(int x1, int y1, int x2, int y2, HeuristicType heuristicType = HeuristicType.Euclidean)
        {
            return heuristicType switch
            {
                HeuristicType.Manhattan => Math.Abs(x1 - x2) + Math.Abs(y1 - y2),
                HeuristicType.Euclidean => (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)),
                HeuristicType.Diagonal => Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2)),
                HeuristicType.Chebyshev => Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2)),
                _ => (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))
            };
        }
        
        /// <summary>
        /// 验证路径的有效性
        /// </summary>
        /// <param name="grid">网格</param>
        /// <param name="path">路径</param>
        /// <returns>路径是否有效</returns>
        public static bool ValidatePath(Grid grid, List<Node> path)
        {
            if (path == null || path.Count == 0)
                return false;
            
            for (int i = 0; i < path.Count; i++)
            {
                var node = path[i];
                
                // 检查节点是否在网格范围内
                if (!grid.IsValidPosition(node.X, node.Y))
                    return false;
                
                // 检查节点是否为障碍物
                if (node.IsObstacle)
                    return false;
                
                // 检查相邻节点之间的连接是否有效
                if (i > 0)
                {
                    var prevNode = path[i - 1];
                    var neighbors = grid.GetNeighbors(prevNode);
                    
                    if (!neighbors.Contains(node))
                        return false;
                }
            }
            
            return true;
        }
    }
}