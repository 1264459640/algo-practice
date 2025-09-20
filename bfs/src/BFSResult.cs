using System.Collections.Generic;

namespace BFS
{
    /// <summary>
    /// 存储BFS遍历的结果。
    /// </summary>
    public class BFSResult
    {
        /// <summary>
        /// 顶点被访问的顺序。
        /// </summary>
        public List<int> VisitOrder { get; set; } = new List<int>();

        /// <summary>
        /// 从起始顶点到各顶点的距离（层数）。
        /// </summary>
        public Dictionary<int, int> Distance { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// 遍历过程中每个顶点的父节点。
        /// </summary>
        public Dictionary<int, int> Parent { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// 按层次分组的顶点列表。
        /// </summary>
        public List<List<int>> Levels { get; set; } = new List<List<int>>();

        /// <summary>
        /// 存储查找到的路径。
        /// </summary>
        public List<List<int>> Paths { get; set; } = new List<List<int>>();

        /// <summary>
        /// 图中连通分量的数量。
        /// </summary>
        public int ComponentCount { get; set; } = 0;

        /// <summary>
        /// 指示图是否为二分图。
        /// </summary>
        public bool IsBipartite { get; set; } = true;

        /// <summary>
        /// 二分图的着色结果（0或1表示两种颜色，-1表示未着色）。
        /// </summary>
        public Dictionary<int, int> Coloring { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// 最短路径的长度。
        /// </summary>
        public int ShortestPathLength { get; set; } = -1;

        /// <summary>
        /// 是否找到了目标顶点。
        /// </summary>
        public bool TargetFound { get; set; } = false;
    }
}