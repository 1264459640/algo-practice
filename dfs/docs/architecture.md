# 架构文档

本文档概述了DFS项目的体系结构。

## 类图

```mermaid
classDiagram
    class DepthFirstSearch {
        <<static>>
        +DFSRecursive(Graph, int): DFSResult
        +DFSIterative(Graph, int): DFSResult
        +DFSComplete(Graph): DFSResult
        +FindPath(Graph, int, int): List~int~
        +FindAllPaths(Graph, int, int): List~List~int~~
        +HasCycleUndirected(Graph): bool
        +HasCycleDirected(Graph): bool
        +TopologicalSort(Graph): List~int~
        +IsConnected(Graph): bool
        +GetConnectedComponents(Graph): List~List~int~~
    }

    class Graph {
        -Dictionary~int, List~int~~ _adjacencyList
        -bool _isDirected
        +VertexCount: int
        +EdgeCount: int
        +Graph(bool)
        +AddVertex(int)
        +AddEdge(int, int)
        +GetNeighbors(int): IEnumerable~int~
        +GetVertices(): IEnumerable~int~
        +ContainsVertex(int): bool
    }

    class DFSResult {
        +VisitOrder: List~int~
        +DiscoveryTime: Dictionary~int, int~
        +FinishTime: Dictionary~int, int~
        +Parent: Dictionary~int, int~
        +Paths: List~List~int~~
        +ComponentCount: int
        +HasCycle: bool
    }

    class MazeSolver {
        -int[,] _maze
        -int _rows
        -int _cols
        -int[] _dx
        -int[] _dy
        +MazeSolver(int[,])
        +SolveMaze(int, int, int, int): List~(int, int)~
        +FindAllPaths(int, int, int, int): List~List~(int, int)~~
    }

    DepthFirstSearch ..> Graph
    DepthFirstSearch ..> DFSResult
```

## 类说明

### `Graph.cs`

`Graph` 类使用邻接表表示图形数据结构。它可用于创建有向图和无向图。它提供了添加顶点和边以及检索有关图形信息的方法。

### `DFSResult.cs`

`DFSResult` 类是一个数据容器，用于存储深度优先搜索遍历的结果。这包括访问顶点的顺序、发现和完成时间、父指针以及找到的任何路径。

### `DepthFirstSearch.cs`

这是一个静态类，包含深度优先搜索算法的各种实现。其中包括DFS的递归和迭代版本，以及用于查找路径、检测周期、执行拓扑排序和查找连接组件的方法。

### `MazeSolver.cs`

`MazeSolver` 类提供了DFS算法在解决迷宫问题中的实际应用。它可以在基于网格的迷宫中找到从起点到终点的单条路径或所有可能路径。