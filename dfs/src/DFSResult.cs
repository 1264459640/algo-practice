using System.Collections.Generic;

namespace DFS
{
    /// <summary>
    /// 存储DFS遍历的结果。
    /// </summary>
    public class DFSResult
    {
        /// <summary>
        /// 顶点被访问的顺序。
        /// </summary>
        public List<int> VisitOrder { get; set; } = new List<int>();

        /// <summary>
        /// 顶点被首次发现的时间戳。
        /// </summary>
        public Dictionary<int, int> DiscoveryTime { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// 顶点的所有邻接顶点都已被访问后的完成时间戳。
        /// </summary>
        public Dictionary<int, int> FinishTime { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// 遍历过程中每个顶点的父节点。
        /// </summary>
        public Dictionary<int, int> Parent { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// 存储查找到的路径。
        /// </summary>
        public List<List<int>> Paths { get; set; } = new List<List<int>>();

        /// <summary>
        /// 图中连通分量的数量。
        /// </summary>
        public int ComponentCount { get; set; } = 0;

        /// <summary>
        /// 指示图中是否存在环。
        /// </summary>
        public bool HasCycle { get; set; } = false;
    }
}