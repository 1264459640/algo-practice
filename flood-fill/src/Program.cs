using System;
using FloodFillAlgorithm;

public class Program
{
    public static void Main(string[] args)
    {
        // 检查是否在Godot环境中运行
        if (args.Length > 0 && args[0] == "--console")
        {
            // 控制台模式
            Console.WriteLine("=== 泛洪算法 (Flood Fill) 控制台演示 ===");
            Console.WriteLine();
            
            var demo = new FloodFill.ConsoleDemo();
            demo.RunAllDemos();
        }
        else
        {
            // Godot模式 - 不需要特殊处理，Godot会自动处理
            Console.WriteLine("泛洪算法项目已启动");
            Console.WriteLine("请在Godot编辑器中运行项目以查看可视化界面");
            Console.WriteLine("或使用 --console 参数运行控制台演示");
        }
    }
}