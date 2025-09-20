using System.Collections.Generic;
using System.Linq;

namespace BFS
{
    /// <summary>
    /// 图的表示方式：邻接表。
    /// 适用于稀疏图，节省空间。
    /// </summary>
    public class Graph
    {
        // 使用字典存储邻接表，键是顶点，值是邻接顶点列表。
        private readonly Dictionary<int, List<int>> _adjacencyList;
        // 标记图是否为有向图。
        private readonly bool _isDirected;

        /// <summary>
        /// 图中顶点的数量。
        /// </summary>
        public int VertexCount { get; private set; }

        /// <summary>
        /// 图中边的数量。
        /// </summary>
        public int EdgeCount { get; private set; }

        /// <summary>
        /// Graph构造函数。
        /// </summary>
        /// <param name="isDirected">如果为true，则创建有向图；否则创建无向图。</param>
        public Graph(bool isDirected = false)
        {
            _adjacencyList = new Dictionary<int, List<int>>();
            _isDirected = isDirected;
            VertexCount = 0;
            EdgeCount = 0;
        }

        /// <summary>
        /// 向图中添加一个顶点。
        /// 如果顶点已存在，则不进行任何操作。
        /// </summary>
        /// <param name="vertex">要添加的顶点。</param>
        public void AddVertex(int vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList[vertex] = new List<int>();
                VertexCount++;
            }
        }

        /// <summary>
        /// 向图中添加一条边。
        /// 如果顶点不存在，会自动创建。
        /// </summary>
        /// <param name="from">边的起始顶点。</param>
        /// <param name="to">边的结束顶点。</param>
        public void AddEdge(int from, int to)
        {
            // 确保两个顶点都存在于邻接表中
            AddVertex(from);
            AddVertex(to);

            // 添加从'from'到'to'的边
            if (!_adjacencyList[from].Contains(to))
            {
                _adjacencyList[from].Add(to);
                EdgeCount++;
            }


            // 如果是无向图，还需要添加从'to'到'from'的边
            if (!_isDirected)
            {
                if (!_adjacencyList[to].Contains(from))
                {
                    _adjacencyList[to].Add(from);
                }
            }
        }

        /// <summary>
        /// 获取一个顶点的所有邻接顶点。
        /// </summary>
        /// <param name="vertex">要查询的顶点。</param>
        /// <returns>一个包含所有邻接顶点的可枚举集合。</returns>
        public IEnumerable<int> GetNeighbors(int vertex)
        {
            return _adjacencyList.ContainsKey(vertex) ? _adjacencyList[vertex] : Enumerable.Empty<int>();
        }

        /// <summary>
        /// 获取图中的所有顶点。
        /// </summary>
        /// <returns>一个包含所有顶点的可枚举集合。</returns>
        public IEnumerable<int> GetVertices()
        {
            return _adjacencyList.Keys;
        }

        /// <summary>
        /// 检查图中是否存在指定的顶点。
        /// </summary>
        /// <param name="vertex">要检查的顶点。</param>
        /// <returns>如果顶点存在，则为true；否则为false。</returns>
        public bool ContainsVertex(int vertex)
        {
            return _adjacencyList.ContainsKey(vertex);
        }
    }
}