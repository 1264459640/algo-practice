# 算法练习项目

这是一个用于练习各种排序算法的 C# 项目集合。

## 项目结构

```
algo-practice/
├── algo-practice.sln          # 解决方案文件
├── BubbleSort/                 # 冒泡排序项目
│   ├── BubbleSort.csproj
│   └── Program.cs
├── .vscode/                    # VS Code 配置
│   ├── launch.json            # 调试配置
│   └── tasks.json             # 任务配置
└── README.md                   # 项目说明
```

## 运行项目

### 命令行运行
```bash
# 构建整个解决方案
dotnet build algo-practice.sln

# 运行冒泡排序项目
dotnet run --project .\BubbleSort\BubbleSort.csproj
```

### VS Code 调试
1. 打开项目文件夹
2. 按 `F5` 或使用调试面板
3. 选择"调试冒泡排序"配置

## 中文乱码解决方案

如果在某些环境下遇到中文显示乱码，可以尝试以下方法：

### 方法1：设置 PowerShell 编码
```powershell
# 临时设置当前会话
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
chcp 65001

# 或者设置环境变量
$env:DOTNET_SYSTEM_CONSOLE_ALLOW_ANSI_COLOR_REDIRECTION = "true"
```

### 方法2：使用 Windows Terminal
- 推荐使用 Windows Terminal 而不是传统的 cmd 或 PowerShell
- Windows Terminal 对 UTF-8 支持更好

### 方法3：VS Code 终端设置
在 VS Code 设置中添加：
```json
{
    "terminal.integrated.shellArgs.windows": ["-NoExit", "-Command", "chcp 65001"]
}
```

## 冒泡排序功能

- ✅ 支持升序和降序排序
- ✅ 提前退出优化（已排序时停止）
- ✅ 统计信息（轮数、比较次数、交换次数）
- ✅ 详细的中文注释
- ✅ 随机数组生成

## 后续计划

- [ ] 选择排序 (SelectionSort)
- [ ] 插入排序 (InsertionSort)
- [ ] 快速排序 (QuickSort)
- [ ] 归并排序 (MergeSort)
- [ ] 堆排序 (HeapSort)
- [ ] 单元测试项目
- [ ] 性能基准测试