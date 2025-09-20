using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FloodFillVisualizer : Control
{
    private FloodFillAlgorithm.Grid _grid;
    private FloodFillAlgorithm.FloodFill _floodFill;
    private GridContainer _gridContainer;
    private VBoxContainer _controlPanel;
    private OptionButton _algorithmSelector;
    private OptionButton _connectivitySelector;
    private SpinBox _widthSpinBox;
    private SpinBox _heightSpinBox;
    private Button _generateButton;
    private Button _clearButton;
    private Button _randomButton;
    private Label _statusLabel;
    private Label _performanceLabel;
    
    private const int CELL_SIZE = 20;
    private Color[] _colors = {
        Colors.White,      // 0 - 空白
        Colors.Black,      // 1 - 障碍物
        Colors.Red,        // 2 - 填充颜色1
        Colors.Blue,       // 3 - 填充颜色2
        Colors.Green,      // 4 - 填充颜色3
        Colors.Yellow,     // 5 - 填充颜色4
        Colors.Purple,     // 6 - 填充颜色5
        Colors.Orange,     // 7 - 填充颜色6
        Colors.Cyan,       // 8 - 填充颜色7
        Colors.Pink        // 9 - 填充颜色8
    };
    
    private int _currentFillValue = 2;
    private bool _isDrawing = false;
    private bool _isErasing = false;

    public override void _Ready()
    {
        SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        
        _floodFill = new FloodFillAlgorithm.FloodFill();
        
        CreateUI();
        GenerateGrid(20, 15);
    }

    private void CreateUI()
    {
        var hbox = new HBoxContainer();
        AddChild(hbox);
        
        // 创建控制面板
        CreateControlPanel(hbox);
        
        // 创建网格容器
        var scrollContainer = new ScrollContainer();
        scrollContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        scrollContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        hbox.AddChild(scrollContainer);
        
        _gridContainer = new GridContainer();
        scrollContainer.AddChild(_gridContainer);
    }

    private void CreateControlPanel(HBoxContainer parent)
    {
        _controlPanel = new VBoxContainer();
        _controlPanel.CustomMinimumSize = new Vector2(250, 0);
        parent.AddChild(_controlPanel);
        
        // 网格尺寸控制
        var sizeGroup = new VBoxContainer();
        sizeGroup.AddThemeStyleboxOverride("panel", new StyleBoxFlat());
        _controlPanel.AddChild(sizeGroup);
        
        var sizeLabel = new Label();
        sizeLabel.Text = "网格尺寸";
        sizeGroup.AddChild(sizeLabel);
        
        var widthContainer = new HBoxContainer();
        sizeGroup.AddChild(widthContainer);
        widthContainer.AddChild(new Label { Text = "宽度:" });
        _widthSpinBox = new SpinBox();
        _widthSpinBox.MinValue = 5;
        _widthSpinBox.MaxValue = 50;
        _widthSpinBox.Value = 20;
        widthContainer.AddChild(_widthSpinBox);
        
        var heightContainer = new HBoxContainer();
        sizeGroup.AddChild(heightContainer);
        heightContainer.AddChild(new Label { Text = "高度:" });
        _heightSpinBox = new SpinBox();
        _heightSpinBox.MinValue = 5;
        _heightSpinBox.MaxValue = 50;
        _heightSpinBox.Value = 15;
        heightContainer.AddChild(_heightSpinBox);
        
        // 算法选择
        var algorithmGroup = new VBoxContainer();
        _controlPanel.AddChild(algorithmGroup);
        
        var algorithmLabel = new Label();
        algorithmLabel.Text = "算法类型";
        algorithmGroup.AddChild(algorithmLabel);
        
        _algorithmSelector = new OptionButton();
        _algorithmSelector.AddItem("递归算法");
        _algorithmSelector.AddItem("栈算法 (DFS)");
        _algorithmSelector.AddItem("队列算法 (BFS)");
        algorithmGroup.AddChild(_algorithmSelector);
        
        // 连通性选择
        var connectivityGroup = new VBoxContainer();
        _controlPanel.AddChild(connectivityGroup);
        
        var connectivityLabel = new Label();
        connectivityLabel.Text = "连通性";
        connectivityGroup.AddChild(connectivityLabel);
        
        _connectivitySelector = new OptionButton();
        _connectivitySelector.AddItem("4-连通");
        _connectivitySelector.AddItem("8-连通");
        connectivityGroup.AddChild(_connectivitySelector);
        
        // 控制按钮
        var buttonGroup = new VBoxContainer();
        _controlPanel.AddChild(buttonGroup);
        
        _generateButton = new Button();
        _generateButton.Text = "生成新网格";
        _generateButton.Pressed += OnGeneratePressed;
        buttonGroup.AddChild(_generateButton);
        
        _clearButton = new Button();
        _clearButton.Text = "清空网格";
        _clearButton.Pressed += OnClearPressed;
        buttonGroup.AddChild(_clearButton);
        
        _randomButton = new Button();
        _randomButton.Text = "随机障碍物";
        _randomButton.Pressed += OnRandomPressed;
        buttonGroup.AddChild(_randomButton);
        
        // 状态显示
        var statusGroup = new VBoxContainer();
        _controlPanel.AddChild(statusGroup);
        
        var statusTitle = new Label();
        statusTitle.Text = "状态信息";
        statusGroup.AddChild(statusTitle);
        
        _statusLabel = new Label();
        _statusLabel.Text = "点击网格开始填充";
        _statusLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        statusGroup.AddChild(_statusLabel);
        
        _performanceLabel = new Label();
        _performanceLabel.Text = "";
        _performanceLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        statusGroup.AddChild(_performanceLabel);
        
        // 说明文本
        var instructionGroup = new VBoxContainer();
        _controlPanel.AddChild(instructionGroup);
        
        var instructionTitle = new Label();
        instructionTitle.Text = "操作说明";
        instructionGroup.AddChild(instructionTitle);
        
        var instructionText = new Label();
        instructionText.Text = "• 左键点击：泛洪填充\n• 右键拖拽：绘制障碍物\n• 中键拖拽：擦除";
        instructionText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        instructionGroup.AddChild(instructionText);
    }

    private void GenerateGrid(int width, int height)
    {
        _grid = new FloodFillAlgorithm.Grid(width, height);
        UpdateGridDisplay();
        _statusLabel.Text = $"生成了 {width}x{height} 的网格";
    }

    private void UpdateGridDisplay()
    {
        // 清除现有的网格显示
        foreach (Node child in _gridContainer.GetChildren())
        {
            child.QueueFree();
        }
        
        _gridContainer.Columns = _grid.Width;
        
        // 创建网格按钮
        for (int y = 0; y < _grid.Height; y++)
        {
            for (int x = 0; x < _grid.Width; x++)
            {
                var button = new Button();
                button.CustomMinimumSize = new Vector2(CELL_SIZE, CELL_SIZE);
                button.Flat = true;
                
                int value = _grid[x, y];
                button.Modulate = _colors[Math.Min(value, _colors.Length - 1)];
                
                // 存储坐标信息
                button.SetMeta("x", x);
                button.SetMeta("y", y);
                
                // 连接信号
                button.Pressed += () => OnCellPressed(x, y);
                button.GuiInput += (inputEvent) => OnCellInput(inputEvent, x, y);
                
                _gridContainer.AddChild(button);
            }
        }
    }

    private void OnCellPressed(int x, int y)
    {
        if (Input.IsActionPressed("ui_accept") || Input.IsMouseButtonPressed(MouseButton.Left))
        {
            PerformFloodFill(x, y);
        }
    }

    private void OnCellInput(InputEvent @event, int x, int y)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Right && mouseButton.Pressed)
            {
                _isDrawing = true;
                _grid[x, y] = 1; // 设置为障碍物
                UpdateCellDisplay(x, y);
            }
            else if (mouseButton.ButtonIndex == MouseButton.Middle && mouseButton.Pressed)
            {
                _isErasing = true;
                _grid[x, y] = 0; // 清除
                UpdateCellDisplay(x, y);
            }
            else if (!mouseButton.Pressed)
            {
                _isDrawing = false;
                _isErasing = false;
            }
        }
        else if (@event is InputEventMouseMotion && (_isDrawing || _isErasing))
        {
            if (_isDrawing)
            {
                _grid[x, y] = 1;
            }
            else if (_isErasing)
            {
                _grid[x, y] = 0;
            }
            UpdateCellDisplay(x, y);
        }
    }

    private void UpdateCellDisplay(int x, int y)
    {
        int index = y * _grid.Width + x;
        if (index < _gridContainer.GetChildCount())
        {
            var button = _gridContainer.GetChild(index) as Button;
            if (button != null)
            {
                int value = _grid[x, y];
                button.Modulate = _colors[Math.Min(value, _colors.Length - 1)];
            }
        }
    }

    private void PerformFloodFill(int x, int y)
    {
        var originalValue = _grid[x, y];
        if (originalValue == 1) // 不能填充障碍物
        {
            _statusLabel.Text = "无法填充障碍物！";
            return;
        }
        
        var algorithmType = (FloodFillAlgorithm.FloodFillAlgorithm)_algorithmSelector.Selected;
        var connectivity = (FloodFillAlgorithm.ConnectivityType)_connectivitySelector.Selected;
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        FloodFillAlgorithm.FloodFillResult result = algorithmType switch
        {
            FloodFillAlgorithm.FloodFillAlgorithm.Recursive => _floodFill.FillRecursive(_grid, x, y, _currentFillValue, connectivity),
            FloodFillAlgorithm.FloodFillAlgorithm.Stack => _floodFill.FillWithStack(_grid, x, y, _currentFillValue, connectivity),
            FloodFillAlgorithm.FloodFillAlgorithm.Queue => _floodFill.FillWithQueue(_grid, x, y, _currentFillValue, connectivity),
            _ => throw new ArgumentException("未知的算法类型")
        };
        
        stopwatch.Stop();
        
        // 更新显示
        _grid = result.FilledGrid;
        UpdateGridDisplay();
        
        // 更新状态信息
        _statusLabel.Text = $"填充完成！填充了 {result.FilledPositions.Count} 个单元格";
        _performanceLabel.Text = $"算法: {GetAlgorithmName(algorithmType)}\n" +
                                $"连通性: {GetConnectivityName(connectivity)}\n" +
                                $"耗时: {stopwatch.ElapsedMilliseconds} ms\n" +
                                $"边界框: {result.BoundingBox.Width}x{result.BoundingBox.Height}\n" +
                                $"周长: {result.GetPerimeter()}";
        
        // 切换到下一个填充颜色
        _currentFillValue = (_currentFillValue % (_colors.Length - 2)) + 2;
    }

    private string GetAlgorithmName(FloodFillAlgorithm.FloodFillAlgorithm type)
    {
        return type switch
        {
            FloodFillAlgorithm.FloodFillAlgorithm.Recursive => "递归算法",
            FloodFillAlgorithm.FloodFillAlgorithm.Stack => "栈算法 (DFS)",
            FloodFillAlgorithm.FloodFillAlgorithm.Queue => "队列算法 (BFS)",
            _ => "未知"
        };
    }

    private string GetConnectivityName(FloodFillAlgorithm.ConnectivityType type)
    {
        return type switch
        {
            FloodFillAlgorithm.ConnectivityType.FourConnected => "4-连通",
            FloodFillAlgorithm.ConnectivityType.EightConnected => "8-连通",
            _ => "未知"
        };
    }

    private void OnGeneratePressed()
    {
        int width = (int)_widthSpinBox.Value;
        int height = (int)_heightSpinBox.Value;
        GenerateGrid(width, height);
    }

    private void OnClearPressed()
    {
        for (int y = 0; y < _grid.Height; y++)
        {
            for (int x = 0; x < _grid.Width; x++)
            {
                _grid[x, y] = 0;
            }
        }
        UpdateGridDisplay();
        _statusLabel.Text = "网格已清空";
        _performanceLabel.Text = "";
    }

    private void OnRandomPressed()
    {
        var random = new Random();
        for (int y = 0; y < _grid.Height; y++)
        {
            for (int x = 0; x < _grid.Width; x++)
            {
                if (random.NextDouble() < 0.3) // 30% 概率生成障碍物
                {
                    _grid[x, y] = 1;
                }
                else
                {
                    _grid[x, y] = 0;
                }
            }
        }
        UpdateGridDisplay();
        _statusLabel.Text = "已生成随机障碍物";
        _performanceLabel.Text = "";
    }
}