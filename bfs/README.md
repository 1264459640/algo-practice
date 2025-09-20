# 宽度优先搜索 (BFS) 算法实现

## 项目简介

本项目实现了宽度优先搜索（Breadth-First Search, BFS）算法的多种变种和应用场景，包括基本遍历、最短路径查找、层次遍历、迷宫求解等功能。

## 项目结构

```
bfs/
├── src/
│   ├── BreadthFirstSearch.cs    # BFS核心算法实现
│   ├── Graph.cs                 # 图的数据结构
│   ├── BFSResult.cs            # BFS遍历结果存储
│   ├── MazeSolver.cs           # 迷宫求解器（BFS版本）
│   └── ConsoleDemo.cs          # 控制台演示程序
├── BFS.csproj                  # 项目配置文件
├── BFS.sln                     # 解决方案文件
└── README.md                   # 项目说明文档
```

## 功能特性

### 核心算法
- **基本BFS遍历**：使用队列实现的标准BFS算法
- **最短路径查找**：在无权图中查找最短路径
- **层次遍历**：按层次输出图的遍历结果
- **连通性检测**：检查图的连通性
- **最短距离计算**：计算从起点到各顶点的最短距离

### 实际应用
- **迷宫求解**：使用BFS找到迷宫的最短路径
- **图的连通分量**：分析图的连通性
- **二分图检测**：检测图是否为二分图

## 算法特点

### BFS vs DFS
- **BFS特点**：
  - 使用队列（FIFO）
  - 逐层遍历
  - 能找到最短路径（无权图）
  - 空间复杂度较高
  - 适合最短路径问题

- **DFS特点**：
  - 使用栈（LIFO）或递归
  - 深度优先遍历
  - 空间复杂度较低
  - 适合路径存在性问题

## 使用方法

### 编译和运行

```bash
# 编译项目
dotnet build

# 运行演示程序
dotnet run
```

### 基本用法示例

```csharp
// 创建图
var graph = new Graph(false); // false表示无向图
graph.AddEdge(0, 1);
graph.AddEdge(0, 2);
graph.AddEdge(1, 3);

// BFS遍历
var result = BreadthFirstSearch.BFS(graph, 0);
Console.WriteLine($"访问顺序: [{string.Join(", ", result.VisitOrder)}]");

// 查找最短路径
var path = BreadthFirstSearch.FindShortestPath(graph, 0, 3);
Console.WriteLine($"最短路径: [{string.Join(" → ", path)}]");
```

## 时间复杂度

- **时间复杂度**：O(V + E)，其中V是顶点数，E是边数
- **空间复杂度**：O(V)，用于存储队列和访问标记

## 应用场景

1. **最短路径问题**：在无权图中找最短路径
2. **层次遍历**：按层次处理数据
3. **连通性检测**：检查图的连通性
4. **迷宫求解**：找到迷宫的最短出路
5. **社交网络分析**：找到最近的朋友关系
6. **网络爬虫**：按层次爬取网页

## 学习资源

- [宽度优先搜索算法原理](docs/宽度优先搜索算法原理.md)
- [BFS与DFS的比较](docs/BFS与DFS比较.md)
- [实际应用案例](docs/应用案例.md)

## 许可证

本项目仅用于学习和教育目的。