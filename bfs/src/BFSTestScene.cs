using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BFS
{
    /// <summary>
    /// BFS算法的Godot测试场景
    /// 提供可视化的BFS演示
    /// </summary>
    public partial class BFSTestScene : Control
    {
        private Graph testGraph;
        private RichTextLabel resultText;
        private Button runBfsButton;
        private Button runShortestPathButton;
        private Button runBipartiteButton;
        private Button mazeButton;
        private SpinBox startVertexSpinBox;
        private SpinBox targetVertexSpinBox;
        
        public override void _Ready()
        {
            SetupUI();
            CreateTestGraph();
        }
        
        private void SetupUI()
        {
            // 获取UI节点引用
            startVertexSpinBox = GetNode<SpinBox>("VBoxContainer/ControlPanel/StartSpinBox");
            targetVertexSpinBox = GetNode<SpinBox>("VBoxContainer/ControlPanel/TargetSpinBox");
            
            runBfsButton = GetNode<Button>("VBoxContainer/ButtonPanel/BFSButton");
            runShortestPathButton = GetNode<Button>("VBoxContainer/ButtonPanel/ShortestPathButton");
            runBipartiteButton = GetNode<Button>("VBoxContainer/ButtonPanel/BipartiteButton");
            mazeButton = GetNode<Button>("VBoxContainer/ButtonPanel/MazeButton");
            
            resultText = GetNode<RichTextLabel>("VBoxContainer/ResultScrollContainer/ResultText");
            
            // 连接按钮信号
            runBfsButton.Pressed += OnRunBfsPressed;
            runShortestPathButton.Pressed += OnRunShortestPathPressed;
            runBipartiteButton.Pressed += OnRunBipartitePressed;
            mazeButton.Pressed += OnRunMazePressed;
        }
        
        private void CreateTestGraph()
        {
            testGraph = new Graph(false);
            
            // 创建示例图
            testGraph.AddEdge(0, 1);
            testGraph.AddEdge(0, 2);
            testGraph.AddEdge(1, 3);
            testGraph.AddEdge(1, 4);
            testGraph.AddEdge(2, 5);
            testGraph.AddEdge(3, 6);
            testGraph.AddEdge(4, 6);
            testGraph.AddEdge(5, 6);
            testGraph.AddEdge(7, 8); // 添加一个独立的连通分量
        }
        
        private void OnRunBfsPressed()
        {
            int startVertex = (int)startVertexSpinBox.Value;
            
            try
            {
                var result = BreadthFirstSearch.BFS(testGraph, startVertex);
                var levelResult = BreadthFirstSearch.BFSLevelOrder(testGraph, startVertex);
                
                var output = $"=== BFS遍历结果 ===\n";
                output += $"起始顶点: {startVertex}\n";
                output += $"访问顺序: [{string.Join(", ", result.VisitOrder)}]\n\n";
                
                output += "距离信息:\n";
                foreach (var kvp in result.Distance.OrderBy(x => x.Key))
                {
                    output += $"顶点{kvp.Key}: 距离={kvp.Value}\n";
                }
                
                output += "\n按层次分组:\n";
                for (int i = 0; i < levelResult.Levels.Count; i++)
                {
                    output += $"第{i}层: [{string.Join(", ", levelResult.Levels[i])}]\n";
                }
                
                // 连通性分析
                var completeResult = BreadthFirstSearch.BFSComplete(testGraph);
                output += $"\n连通分量数: {completeResult.ComponentCount}\n";
                output += $"图是否连通: {(BreadthFirstSearch.IsConnected(testGraph) ? "是" : "否")}";
                
                resultText.Text = FormatOutput(output);
            }
            catch (Exception ex)
            {
                resultText.Text = $"[color=red]错误: {ex.Message}[/color]";
            }
        }
        
        private void OnRunShortestPathPressed()
        {
            int startVertex = (int)startVertexSpinBox.Value;
            int targetVertex = (int)targetVertexSpinBox.Value;
            
            try
            {
                var path = BreadthFirstSearch.FindShortestPath(testGraph, startVertex, targetVertex);
                var allPaths = BreadthFirstSearch.FindAllShortestPaths(testGraph, startVertex, targetVertex);
                
                var output = $"=== 最短路径查找结果 ===\n";
                output += $"从顶点{startVertex}到顶点{targetVertex}:\n\n";
                
                if (path.Count > 0)
                {
                    output += $"最短路径: [{string.Join(" → ", path)}]\n";
                    output += $"路径长度: {path.Count - 1}条边\n\n";
                    
                    output += $"所有最短路径 (共{allPaths.Count}条):\n";
                    for (int i = 0; i < Math.Min(allPaths.Count, 5); i++)
                    {
                        output += $"路径{i + 1}: [{string.Join(" → ", allPaths[i])}]\n";
                    }
                    
                    if (allPaths.Count > 5)
                    {
                        output += $"... 还有{allPaths.Count - 5}条路径\n";
                    }
                }
                else
                {
                    output += "未找到路径（顶点不连通）\n";
                }
                
                // 显示距离信息
                var distances = BreadthFirstSearch.FindShortestDistances(testGraph, startVertex);
                output += $"\n从顶点{startVertex}到各顶点的最短距离:\n";
                foreach (var kvp in distances.OrderBy(x => x.Key))
                {
                    output += $"到顶点{kvp.Key}: {kvp.Value}步\n";
                }
                
                resultText.Text = FormatOutput(output);
            }
            catch (Exception ex)
            {
                resultText.Text = $"[color=red]错误: {ex.Message}[/color]";
            }
        }
        
        private void OnRunBipartitePressed()
        {
            try
            {
                var result = BreadthFirstSearch.CheckBipartite(testGraph);
                
                var output = $"=== 二分图检测结果 ===\n";
                output += $"是否为二分图: {(result.IsBipartite ? "是" : "否")}\n\n";
                
                if (result.IsBipartite)
                {
                    output += "着色结果:\n";
                    var group0 = result.Coloring.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key).OrderBy(x => x);
                    var group1 = result.Coloring.Where(kvp => kvp.Value == 1).Select(kvp => kvp.Key).OrderBy(x => x);
                    
                    output += $"组1 (颜色0): [{string.Join(", ", group0)}]\n";
                    output += $"组2 (颜色1): [{string.Join(", ", group1)}]\n\n";
                    
                    output += "二分图的应用场景:\n";
                    output += "- 任务分配问题\n";
                    output += "- 匹配问题\n";
                    output += "- 调度问题\n";
                    output += "- 图着色问题";
                }
                else
                {
                    output += "该图包含奇数长度的环，不是二分图。\n\n";
                    output += "非二分图的特征:\n";
                    output += "- 包含奇数长度的环\n";
                    output += "- 无法用两种颜色着色\n";
                    output += "- 不能进行完美二分匹配";
                }
                
                resultText.Text = FormatOutput(output);
            }
            catch (Exception ex)
            {
                resultText.Text = $"[color=red]错误: {ex.Message}[/color]";
            }
        }
        
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey keyEvent && keyEvent.Pressed)
            {
                switch (keyEvent.Keycode)
                {
                    case Key.Key1:
                        OnRunBfsPressed();
                        break;
                    case Key.Key2:
                        OnRunShortestPathPressed();
                        break;
                    case Key.Key3:
                        OnRunBipartitePressed();
                        break;
                    case Key.Key4:
                        OnRunMazePressed();
                        break;
                    case Key.Escape:
                        GetTree().Quit();
                        break;
                }
            }
        }
        
        private void OnRunMazePressed()
        {
            try
            {
                // 创建测试迷宫
                int[,] maze = {
                    {0, 1, 0, 0, 0, 0},
                    {0, 1, 0, 1, 1, 0},
                    {0, 0, 0, 1, 0, 0},
                    {1, 1, 0, 0, 0, 1},
                    {0, 0, 0, 1, 0, 0},
                    {0, 1, 0, 0, 0, 0}
                };
                
                var solver = new MazeSolver(maze);
                int startRow = 0, startCol = 0;
                int endRow = 5, endCol = 5;
                
                var output = "=== 迷宫求解演示 ===\n";
                output += "迷宫结构 (S=起点, E=终点, ■=墙, □=通路):\n";
                output += DisplayMaze(maze, startRow, startCol, endRow, endCol);
                output += "\n";
                
                // 求解最短路径
                var shortestPath = solver.SolveMaze(startRow, startCol, endRow, endCol);
                
                if (shortestPath.Count > 0)
                {
                    output += $"找到最短路径! 路径长度: {shortestPath.Count - 1}步\n";
                    output += "路径坐标: ";
                    for (int i = 0; i < Math.Min(shortestPath.Count, 10); i++)
                    {
                        output += $"({shortestPath[i].row},{shortestPath[i].col})";
                        if (i < shortestPath.Count - 1 && i < 9) output += " → ";
                    }
                    if (shortestPath.Count > 10) output += " ...";
                    output += "\n\n";
                    
                    output += "带路径的迷宫显示 (●=路径):\n";
                    output += DisplayMazeWithPath(maze, shortestPath, startRow, startCol, endRow, endCol);
                }
                else
                {
                    output += "迷宫无解!\n";
                }
                
                // 查找所有最短路径
                var allShortestPaths = solver.FindAllShortestPaths(startRow, startCol, endRow, endCol);
                output += $"\n所有最短路径: 共找到{allShortestPaths.Count}条等长最短路径";
                
                if (allShortestPaths.Count > 0)
                {
                    output += $"\n最短路径长度: {allShortestPaths[0].Count - 1}步";
                }
                
                output += "\n\nBFS在迷宫求解中的优势:";
                output += "\n• 保证找到最短路径";
                output += "\n• 可以找到所有最短路径";
                output += "\n• 适合无权图的最短路径问题";
                
                resultText.Text = FormatOutput(output);
            }
            catch (Exception ex)
            {
                resultText.Text = $"[color=red]错误: {ex.Message}[/color]";
            }
        }
        
        private string DisplayMaze(int[,] maze, int startRow, int startCol, int endRow, int endCol)
        {
            var output = "";
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == startRow && j == startCol)
                        output += "S ";
                    else if (i == endRow && j == endCol)
                        output += "E ";
                    else if (maze[i, j] == 1)
                        output += "■ ";
                    else
                        output += "□ ";
                }
                output += "\n";
            }
            
            return output;
        }
        
        private string DisplayMazeWithPath(int[,] maze, List<(int row, int col)> path, 
            int startRow, int startCol, int endRow, int endCol)
        {
            var pathSet = new HashSet<(int, int)>(path);
            var output = "";
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == startRow && j == startCol)
                        output += "S ";
                    else if (i == endRow && j == endCol)
                        output += "E ";
                    else if (pathSet.Contains((i, j)))
                        output += "● ";
                    else if (maze[i, j] == 1)
                        output += "■ ";
                    else
                        output += "□ ";
                }
                output += "\n";
            }
            
            return output;
        }
        
        private string FormatOutput(string text)
        {
            // 将普通文本转换为BBCode格式
            text = text.Replace("===", "[b]").Replace("===", "[/b]");
            text = text.Replace("【", "[color=blue][b]").Replace("】", "[/b][/color]");
            
            // 高亮数字和路径
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\[(\d+(?:,\s*\d+)*)\]", "[color=green][$1][/color]");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"(\d+)条", "[color=orange]$1[/color]条");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"(\d+)步", "[color=orange]$1[/color]步");
            
            return text;
        }
    }
}