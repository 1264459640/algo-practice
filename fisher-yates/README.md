# Fisher-Yates 洗牌算法项目

这是一个基于 Godot 4.4 和 C# 的 Fisher-Yates 洗牌算法演示项目。

## 项目结构

```
fisher-yates/
├── docs/                           # 文档目录
│   ├── Fisher-Yates算法原理.md     # 算法原理详细说明
│   ├── README.md                   # 详细使用说明
│   └── 测试场景使用说明.md         # 测试场景使用指南
├── src/                            # 源代码目录
│   ├── FisherYatesShuffle.cs       # 核心算法实现
│   ├── TestScene.cs                # Godot 测试场景脚本
│   └── ConsoleDemo.cs              # 控制台演示程序
├── TestScene.tscn                  # Godot 测试场景文件
├── Fisher-Yates.csproj             # 项目文件
├── Fisher-Yates.sln                # 解决方案文件
├── project.godot                   # Godot 项目配置
├── icon.svg                        # 项目图标
└── README.md                       # 本文件
```

## 快速开始

### Godot 测试场景（推荐）

1. 安装 Godot 4.4（支持 C# 的版本）
2. 在 Godot 中导入项目（选择 project.godot 文件）
3. 按 F5 运行测试场景
4. 使用可视化界面测试洗牌算法

### 控制台演示

```bash
dotnet build Fisher-Yates.sln
dotnet run --project Fisher-Yates.csproj
```

## 功能特性

- ✅ 标准 Fisher-Yates 洗牌算法
- ✅ 种子洗牌（可重现结果）
- ✅ 部分洗牌
- ✅ 统计信息收集
- ✅ 可视化测试界面
- ✅ 控制台演示程序
- ✅ 详细的中文文档

## 文档

- [算法原理详解](docs/Fisher-Yates算法原理.md)
- [详细使用说明](docs/README.md)
- [测试场景使用指南](docs/测试场景使用说明.md)

## 许可证

本项目仅用于学习和演示目的。