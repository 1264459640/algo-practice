using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using DFS;

/// <summary>
/// DFS算法测试场景
/// 提供可视化的深度优先搜索演示
/// </summary>
public partial class DFSTestScene : Control
{
    // UI控件引用
    private VBoxContainer _mainContainer;
    private Label _titleLabel;
    private HBoxContainer _buttonContainer;
    private Button _createGraphButton;
    private Button _dfsTraversalButton;
    private Button _findPathButton;
    private Button _detectCycleButton;
    private Button _solveMazeButton;
    private Button _resetButton;
    
    // 显示区域
    private RichTextLabel _graphDisplay;
    private RichTextLabel _resultDisplay;
    private GridContainer _mazeGrid;
    
    // 数据
    private DepthFirstSearch.Graph _currentGraph;
    private int[,] _maze;
    private readonly Random _random = new Random();
    
    public override void _Ready()
    {
        GetUIReferences();
        ConnectSignals();
        InitializeData();
        UpdateDisplay();
    }
    
    /// <summary>
    /// 获取UI节点引用
    /// </summary>
    private void GetUIReferences()
    {
        // 获取场景中预定义的节点
        _mainContainer = GetNode<VBoxContainer>("MainContainer");
        _titleLabel = GetNode<Label>("MainContainer/TitleLabel");
        _buttonContainer = GetNode<HBoxContainer>("MainContainer/ButtonContainer");
        _createGraphButton = GetNode<Button>("MainContainer/ButtonContainer/CreateGraphButton");
        _dfsTraversalButton = GetNode<Button>("MainContainer/ButtonContainer/DFSTraversalButton");
        _findPathButton = GetNode<Button>("MainContainer/ButtonContainer/FindPathButton");
        _detectCycleButton = GetNode<Button>("MainContainer/ButtonContainer/DetectCycleButton");
        _solveMazeButton = GetNode<Button>("MainContainer/ButtonContainer/SolveMazeButton");
        _resetButton = GetNode<Button>("MainContainer/ButtonContainer/ResetButton");
        _graphDisplay = GetNode<RichTextLabel>("MainContainer/GraphDisplay");
        _resultDisplay = GetNode<RichTextLabel>("MainContainer/ResultDisplay");
        _mazeGrid = GetNode<GridContainer>("MainContainer/MazeGrid");
    }
    
    /// <summary>
    /// 连接信号
    /// </summary>
    private void ConnectSignals()
    {
        _createGraphButton.Pressed += OnCreateGraphPressed;
        _dfsTraversalButton.Pressed += OnDFSTraversalPressed;
        _findPathButton.Pressed += OnFindPathPressed;
        _detectCycleButton.Pressed += OnDetectCyclePressed;
        _solveMazeButton.Pressed += OnSolveMazePressed;
        _resetButton.Pressed += OnResetPressed;
    }
    
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitializeData()
    {
        CreateSampleGraph();
        CreateSampleMaze();
    }
    
    /// <summary>
    /// 更新显示
    /// </summary>
    private void UpdateDisplay()
    {
        UpdateGraphDisplay();
        UpdateMazeDisplay();
        _resultDisplay.Text = "[color=green]DFS算法演示就绪[/color]\n\n点击按钮开始测试各种DFS功能。";
    }
    
    /// <summary>
    /// 创建示例图
    /// </summary>
    private void CreateSampleGraph()
    {
        _currentGraph = new DepthFirstSearch.Graph(false); // 无向图
        
        // 添加顶点和边创建一个示例图
        _currentGraph.AddEdge(0, 1);
        _currentGraph.AddEdge(0, 2);
        _currentGraph.AddEdge(1, 3);
        _currentGraph.AddEdge(1, 4);
        _currentGraph.AddEdge(2, 5);
        _currentGraph.AddEdge(3, 6);
        _currentGraph.AddEdge(4, 6);
        _currentGraph.AddEdge(5, 6);
    }
    
    /// <summary>
    /// 创建示例迷宫
    /// </summary>
    private void CreateSampleMaze()
    {
        // 创建一个简单的迷宫 (0=通路, 1=墙)
        _maze = new int[,] {
            {0, 1, 0, 0, 0},
            {0, 1, 0, 1, 0},
            {0, 0, 0, 1, 0},
            {1, 1, 0, 0, 0},
            {0, 0, 0, 1, 0}
        };
    }
    
    /// <summary>
    /// 更新图显示
    /// </summary>
    private void UpdateGraphDisplay()
    {
        var display = "[b]当前图结构:[/b]\n\n";
        display += $"顶点数: {_currentGraph.VertexCount}\n";
        display += $"边数: {_currentGraph.EdgeCount}\n\n";
        display += "[b]邻接表:[/b]\n";
        
        foreach (int vertex in _currentGraph.GetVertices().OrderBy(v => v))
        {
            var neighbors = _currentGraph.GetNeighbors(vertex);
            display += $"顶点 {vertex}: [{string.Join(", ", neighbors)}]\n";
        }
        
        _graphDisplay.Text = display;
    }
    
    /// <summary>
    /// 更新迷宫显示
    /// </summary>
    private void UpdateMazeDisplay()
    {
        // 清除现有子节点
        foreach (Node child in _mazeGrid.GetChildren())
        {
            child.QueueFree();
        }
        
        int rows = _maze.GetLength(0);
        int cols = _maze.GetLength(1);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var button = new Button();
                button.Text = _maze[i, j] == 1 ? "■" : "□";
                button.CustomMinimumSize = new Vector2(30, 30);
                
                var styleBox = new StyleBoxFlat();
                if (_maze[i, j] == 1)
                {
                    styleBox.BgColor = Colors.DarkGray; // 墙
                    button.AddThemeColorOverride("font_color", Colors.White);
                }
                else
                {
                    styleBox.BgColor = Colors.LightGray; // 通路
                    button.AddThemeColorOverride("font_color", Colors.Black);
                }
                
                button.AddThemeStyleboxOverride("normal", styleBox);
                _mazeGrid.AddChild(button);
            }
        }
    }
    
    /// <summary>
    /// 创建图按钮点击事件
    /// </summary>
    private void OnCreateGraphPressed()
    {
        // 创建一个随机图
        _currentGraph = new DepthFirstSearch.Graph(false);
        
        int vertexCount = _random.Next(5, 10);
        int edgeCount = _random.Next(vertexCount - 1, vertexCount * 2);
        
        // 添加随机边
        for (int i = 0; i < edgeCount; i++)
        {
            int from = _random.Next(0, vertexCount);
            int to = _random.Next(0, vertexCount);
            
            if (from != to)
            {
                _currentGraph.AddEdge(from, to);
            }
        }
        
        UpdateGraphDisplay();
        _resultDisplay.Text = "[color=green]已创建新的随机图[/color]\n\n" +
                             $"顶点数: {_currentGraph.VertexCount}\n" +
                             $"边数: {_currentGraph.EdgeCount}";
        
        GD.Print($"创建了新图: {_currentGraph.VertexCount}个顶点, {_currentGraph.EdgeCount}条边");
    }
    
    /// <summary>
    /// DFS遍历按钮点击事件
    /// </summary>
    private void OnDFSTraversalPressed()
    {
        if (_currentGraph.VertexCount == 0)
        {
            _resultDisplay.Text = "[color=red]错误: 图为空，请先创建图[/color]";
            return;
        }
        
        var vertices = _currentGraph.GetVertices().ToList();
        int startVertex = vertices[0];
        
        // 递归DFS
        var recursiveResult = DepthFirstSearch.DFSRecursive(_currentGraph, startVertex);
        
        // 迭代DFS
        var iterativeResult = DepthFirstSearch.DFSIterative(_currentGraph, startVertex);
        
        // 完整DFS（处理非连通图）
        var completeResult = DepthFirstSearch.DFSComplete(_currentGraph);
        
        var display = "[b]DFS遍历结果:[/b]\n\n";
        display += $"[color=blue]递归DFS (从顶点{startVertex}开始):[/color]\n";
        display += $"访问顺序: [{string.Join(", ", recursiveResult.VisitOrder)}]\n\n";
        
        display += $"[color=blue]迭代DFS (从顶点{startVertex}开始):[/color]\n";
        display += $"访问顺序: [{string.Join(", ", iterativeResult.VisitOrder)}]\n\n";
        
        display += $"[color=blue]完整DFS (所有连通分量):[/color]\n";
        display += $"访问顺序: [{string.Join(", ", completeResult.VisitOrder)}]\n";
        display += $"连通分量数: {completeResult.ComponentCount}\n\n";
        
        // 显示发现时间和完成时间
        if (recursiveResult.DiscoveryTime.Count > 0)
        {
            display += "[color=green]时间戳信息:[/color]\n";
            foreach (var vertex in recursiveResult.DiscoveryTime.Keys.OrderBy(v => v))
            {
                display += $"顶点{vertex}: 发现时间={recursiveResult.DiscoveryTime[vertex]}, " +
                          $"完成时间={recursiveResult.FinishTime[vertex]}\n";
            }
        }
        
        _resultDisplay.Text = display;
        
        GD.Print($"DFS遍历完成: 递归访问{recursiveResult.VisitOrder.Count}个顶点, " +
                $"迭代访问{iterativeResult.VisitOrder.Count}个顶点");
    }
    
    /// <summary>
    /// 查找路径按钮点击事件
    /// </summary>
    private void OnFindPathPressed()
    {
        if (_currentGraph.VertexCount < 2)
        {
            _resultDisplay.Text = "[color=red]错误: 图中顶点数不足，无法查找路径[/color]";
            return;
        }
        
        var vertices = _currentGraph.GetVertices().ToList();
        int start = vertices[0];
        int target = vertices[vertices.Count - 1];
        
        // 查找单条路径
        var path = DepthFirstSearch.FindPath(_currentGraph, start, target);
        
        // 查找所有路径
        var allPaths = DepthFirstSearch.FindAllPaths(_currentGraph, start, target);
        
        var display = $"[b]路径查找结果 (从顶点{start}到顶点{target}):[/b]\n\n";
        
        if (path.Count > 0)
        {
            display += $"[color=green]找到路径:[/color] [{string.Join(" → ", path)}]\n";
            display += $"路径长度: {path.Count - 1}条边\n\n";
        }
        else
        {
            display += $"[color=red]未找到从{start}到{target}的路径[/color]\n\n";
        }
        
        display += $"[color=blue]所有可能路径 (共{allPaths.Count}条):[/color]\n";
        for (int i = 0; i < allPaths.Count && i < 5; i++) // 最多显示5条路径
        {
            display += $"路径{i + 1}: [{string.Join(" → ", allPaths[i])}]\n";
        }
        
        if (allPaths.Count > 5)
        {
            display += $"... 还有{allPaths.Count - 5}条路径\n";
        }
        
        _resultDisplay.Text = display;
        
        GD.Print($"路径查找完成: 找到{allPaths.Count}条从{start}到{target}的路径");
    }
    
    /// <summary>
    /// 检测环按钮点击事件
    /// </summary>
    private void OnDetectCyclePressed()
    {
        if (_currentGraph.VertexCount == 0)
        {
            _resultDisplay.Text = "[color=red]错误: 图为空，请先创建图[/color]";
            return;
        }
        
        bool hasCycle = DepthFirstSearch.HasCycleUndirected(_currentGraph);
        bool isConnected = DepthFirstSearch.IsConnected(_currentGraph);
        var components = DepthFirstSearch.GetConnectedComponents(_currentGraph);
        
        var display = "[b]图的性质分析:[/b]\n\n";
        
        display += $"[color={(hasCycle ? "red" : "green")}]是否存在环: {(hasCycle ? "是" : "否")}[/color]\n";
        display += $"[color={(isConnected ? "green" : "orange")}]是否连通: {(isConnected ? "是" : "否")}[/color]\n";
        display += $"连通分量数: {components.Count}\n\n";
        
        display += "[color=blue]连通分量详情:[/color]\n";
        for (int i = 0; i < components.Count; i++)
        {
            display += $"分量{i + 1}: [{string.Join(", ", components[i].OrderBy(v => v))}]\n";
        }
        
        _resultDisplay.Text = display;
        
        GD.Print($"环检测完成: {(hasCycle ? "发现环" : "无环")}, 连通分量数: {components.Count}");
    }
    
    /// <summary>
    /// 求解迷宫按钮点击事件
    /// </summary>
    private void OnSolveMazePressed()
    {
        var solver = new MazeSolver(_maze);
        
        // 求解从左上角到右下角的路径
        int startRow = 0, startCol = 0;
        int endRow = _maze.GetLength(0) - 1, endCol = _maze.GetLength(1) - 1;
        
        var path = solver.SolveMaze(startRow, startCol, endRow, endCol);
        var allPaths = solver.FindAllPaths(startRow, startCol, endRow, endCol);
        
        var display = $"[b]迷宫求解结果 (从({startRow},{startCol})到({endRow},{endCol})):[/b]\n\n";
        
        if (path.Count > 0)
        {
            display += $"[color=green]找到解决方案![/color]\n";
            display += $"路径长度: {path.Count}步\n";
            display += "路径: ";
            for (int i = 0; i < path.Count && i < 10; i++)
            {
                display += $"({path[i].row},{path[i].col})";
                if (i < path.Count - 1 && i < 9) display += " → ";
            }
            if (path.Count > 10) display += " ...";
            display += "\n\n";
        }
        else
        {
            display += "[color=red]迷宫无解![/color]\n\n";
        }
        
        display += $"[color=blue]总共找到{allPaths.Count}条可能路径[/color]\n";
        
        if (allPaths.Count > 0)
        {
            display += "最短路径长度: " + allPaths.Min(p => p.Count) + "步\n";
            display += "最长路径长度: " + allPaths.Max(p => p.Count) + "步\n";
        }
        
        // 更新迷宫显示，高亮路径
        UpdateMazeWithPath(path);
        
        _resultDisplay.Text = display;
        
        GD.Print($"迷宫求解完成: {(path.Count > 0 ? "有解" : "无解")}, 共找到{allPaths.Count}条路径");
    }
    
    /// <summary>
    /// 更新迷宫显示，高亮路径
    /// </summary>
    private void UpdateMazeWithPath(List<(int row, int col)> path)
    {
        var pathSet = new HashSet<(int, int)>(path);
        var children = _mazeGrid.GetChildren();
        
        int rows = _maze.GetLength(0);
        int cols = _maze.GetLength(1);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int index = i * cols + j;
                if (index < children.Count && children[index] is Button button)
                {
                    var styleBox = new StyleBoxFlat();
                    
                    if (pathSet.Contains((i, j)))
                    {
                        // 路径上的点用绿色高亮
                        styleBox.BgColor = Colors.LightGreen;
                        button.AddThemeColorOverride("font_color", Colors.DarkGreen);
                        button.Text = "●";
                    }
                    else if (_maze[i, j] == 1)
                    {
                        styleBox.BgColor = Colors.DarkGray; // 墙
                        button.AddThemeColorOverride("font_color", Colors.White);
                        button.Text = "■";
                    }
                    else
                    {
                        styleBox.BgColor = Colors.LightGray; // 通路
                        button.AddThemeColorOverride("font_color", Colors.Black);
                        button.Text = "□";
                    }
                    
                    button.AddThemeStyleboxOverride("normal", styleBox);
                }
            }
        }
    }
    
    /// <summary>
    /// 重置按钮点击事件
    /// </summary>
    private void OnResetPressed()
    {
        CreateSampleGraph();
        CreateSampleMaze();
        UpdateDisplay();
        
        GD.Print("已重置到初始状态");
    }
    
    /// <summary>
    /// 处理输入事件（键盘快捷键）
    /// </summary>
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            switch (keyEvent.Keycode)
            {
                case Key.Key1:
                    OnCreateGraphPressed();
                    break;
                case Key.Key2:
                    OnDFSTraversalPressed();
                    break;
                case Key.Key3:
                    OnFindPathPressed();
                    break;
                case Key.Key4:
                    OnDetectCyclePressed();
                    break;
                case Key.Key5:
                    OnSolveMazePressed();
                    break;
                case Key.R:
                    OnResetPressed();
                    break;
                case Key.Escape:
                    GetTree().Quit();
                    break;
            }
        }
    }
}