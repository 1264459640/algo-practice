using Godot;
using System;
using System.Collections.Generic;
using FisherYates;

/// <summary>
/// Fisher-Yates算法简单测试场景
/// 提供可视化的洗牌演示
/// </summary>
public partial class TestScene : Control
{
    // UI控件引用
    private VBoxContainer _mainContainer;
    private Label _titleLabel;
    private HBoxContainer _buttonContainer;
    private Button _shuffleNumbersButton;
    private Button _shuffleCardsButton;
    private Button _resetButton;
    
    // 显示区域
    private GridContainer _numbersGrid;
    private GridContainer _cardsGrid;
    private Label _statsLabel;
    
    // 数据
    private int[] _numbers;
    private string[] _cards;
    private readonly Color _originalColor = Colors.LightBlue;
    private readonly Color _shuffledColor = Colors.LightCoral;
    
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
        _shuffleNumbersButton = GetNode<Button>("MainContainer/ButtonContainer/ShuffleNumbersButton");
        _shuffleCardsButton = GetNode<Button>("MainContainer/ButtonContainer/ShuffleCardsButton");
        _resetButton = GetNode<Button>("MainContainer/ButtonContainer/ResetButton");
        _numbersGrid = GetNode<GridContainer>("MainContainer/NumbersGrid");
        _cardsGrid = GetNode<GridContainer>("MainContainer/CardsGrid");
        _statsLabel = GetNode<Label>("MainContainer/StatsLabel");
    }
    
    /// <summary>
    /// 连接信号
    /// </summary>
    private void ConnectSignals()
    {
        _shuffleNumbersButton.Pressed += OnShuffleNumbersPressed;
        _shuffleCardsButton.Pressed += OnShuffleCardsPressed;
        _resetButton.Pressed += OnResetPressed;
    }
    
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitializeData()
    {
        // 初始化数字数组
        _numbers = FisherYatesShuffle.GenerateSequentialArray(1, 20);
        
        // 初始化扑克牌（只取前20张用于显示）
        var fullDeck = FisherYatesShuffle.GenerateCardDeck();
        _cards = new string[20];
        Array.Copy(fullDeck, _cards, 20);
    }
    
    /// <summary>
    /// 更新显示
    /// </summary>
    private void UpdateDisplay()
    {
        UpdateNumbersGrid();
        UpdateCardsGrid();
    }
    
    /// <summary>
    /// 更新数字网格显示
    /// </summary>
    private void UpdateNumbersGrid()
    {
        // 清除现有子节点
        foreach (Node child in _numbersGrid.GetChildren())
        {
            child.QueueFree();
        }
        
        // 添加数字按钮
        for (int i = 0; i < _numbers.Length; i++)
        {
            var button = new Button();
            button.Text = _numbers[i].ToString();
            button.CustomMinimumSize = new Vector2(40, 40);
            
            // 设置颜色（原位置为蓝色，移动位置为红色）
            var styleBox = new StyleBoxFlat();
            if (_numbers[i] == i + 1)
            {
                styleBox.BgColor = _originalColor;
            }
            else
            {
                styleBox.BgColor = _shuffledColor;
            }
            button.AddThemeStyleboxOverride("normal", styleBox);
            
            // 设置字体颜色为黑色以提高对比度
            button.AddThemeColorOverride("font_color", Colors.Black);
            button.AddThemeColorOverride("font_pressed_color", Colors.Black);
            button.AddThemeColorOverride("font_hover_color", Colors.Black);
            button.AddThemeColorOverride("font_focus_color", Colors.Black);
            button.AddThemeColorOverride("font_disabled_color", Colors.DarkGray);
            
            _numbersGrid.AddChild(button);
        }
    }
    
    /// <summary>
    /// 更新扑克牌网格显示
    /// </summary>
    private void UpdateCardsGrid()
    {
        // 清除现有子节点
        foreach (Node child in _cardsGrid.GetChildren())
        {
            child.QueueFree();
        }
        
        // 添加扑克牌按钮
        for (int i = 0; i < _cards.Length; i++)
        {
            var button = new Button();
            button.Text = _cards[i];
            button.CustomMinimumSize = new Vector2(50, 40);
            
            // 根据花色设置颜色
            var styleBox = new StyleBoxFlat();
            if (_cards[i].Contains("♥") || _cards[i].Contains("♦"))
            {
                styleBox.BgColor = Colors.LightPink;
                // 红色花色使用深红色字体
                button.AddThemeColorOverride("font_color", Colors.DarkRed);
            }
            else
            {
                styleBox.BgColor = Colors.LightGray;
                // 黑色花色使用黑色字体
                button.AddThemeColorOverride("font_color", Colors.Black);
            }
            button.AddThemeStyleboxOverride("normal", styleBox);
            
            // 设置其他状态的字体颜色
            button.AddThemeColorOverride("font_pressed_color", Colors.Black);
            button.AddThemeColorOverride("font_hover_color", Colors.Black);
            button.AddThemeColorOverride("font_focus_color", Colors.Black);
            button.AddThemeColorOverride("font_disabled_color", Colors.DarkGray);
            
            _cardsGrid.AddChild(button);
        }
    }
    
    /// <summary>
    /// 洗牌数字按钮点击事件
    /// </summary>
    private void OnShuffleNumbersPressed()
    {
        // 记录原始状态
        var originalNumbers = new int[_numbers.Length];
        Array.Copy(_numbers, originalNumbers, _numbers.Length);
        
        // 执行洗牌
        FisherYatesShuffle.Shuffle(_numbers, out int swapCount);
        
        // 计算统计信息
        int differences = FisherYatesShuffle.CountDifferences(originalNumbers, _numbers);
        int inversions = FisherYatesShuffle.CountInversions(_numbers);
        
        // 更新显示
        UpdateNumbersGrid();
        
        // 更新统计信息
        _statsLabel.Text = $"数字洗牌统计: 交换 {swapCount} 次, 位置变化 {differences} 个, 逆序对 {inversions} 个";
        
        // 输出到控制台
        GD.Print($"数字洗牌结果: [{string.Join(", ", _numbers)}]");
        GD.Print($"统计: 交换{swapCount}次, 位置变化{differences}个, 逆序对{inversions}个");
    }
    
    /// <summary>
    /// 洗牌扑克牌按钮点击事件
    /// </summary>
    private void OnShuffleCardsPressed()
    {
        // 记录原始状态
        var originalCards = new string[_cards.Length];
        Array.Copy(_cards, originalCards, _cards.Length);
        
        // 执行洗牌
        FisherYatesShuffle.Shuffle(_cards, out int swapCount);
        
        // 计算统计信息
        int differences = FisherYatesShuffle.CountDifferences(originalCards, _cards);
        
        // 更新显示
        UpdateCardsGrid();
        
        // 更新统计信息
        _statsLabel.Text = $"扑克牌洗牌统计: 交换 {swapCount} 次, 位置变化 {differences} 个";
        
        // 输出到控制台
        GD.Print($"扑克牌洗牌结果: [{string.Join(", ", _cards)}]");
        GD.Print($"统计: 交换{swapCount}次, 位置变化{differences}个");
    }
    
    /// <summary>
    /// 重置按钮点击事件
    /// </summary>
    private void OnResetPressed()
    {
        // 重新初始化数据
        InitializeData();
        
        // 更新显示
        UpdateDisplay();
        
        // 重置统计信息
        _statsLabel.Text = "统计信息: 已重置";
        
        GD.Print("已重置到初始状态");
    }
    
    /// <summary>
    /// 处理输入事件（添加键盘快捷键）
    /// </summary>
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            switch (keyEvent.Keycode)
            {
                case Key.Key1:
                    OnShuffleNumbersPressed();
                    break;
                case Key.Key2:
                    OnShuffleCardsPressed();
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