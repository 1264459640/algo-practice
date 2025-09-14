using System;
using System.Linq;
using FisherYates;

namespace FisherYates
{
    /// <summary>
    /// Fisher-Yates洗牌算法控制台演示程序
    /// 提供命令行界面来测试各种洗牌功能
    /// </summary>
    public class ConsoleDemo
    {
        public static void Main(string[] args)
        {
            // 设置控制台编码
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            
            Console.WriteLine("=== Fisher-Yates 洗牌算法演示 ===");
            Console.WriteLine();
            
            // 首先测试扑克牌生成
            CardTest.TestCardGeneration();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续其他演示...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示1：基本整数数组洗牌
            Demo1_BasicShuffle();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示2：扑克牌洗牌
            Demo2_CardShuffle();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示3：种子洗牌（可重现）
            Demo3_SeededShuffle();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示4：部分洗牌
            Demo4_PartialShuffle();
            
            Console.WriteLine();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            Console.Clear();
            
            // 演示5：统计特性测试
            Demo5_StatisticalTest();
            
            Console.WriteLine();
            Console.WriteLine("演示完成！按任意键退出...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// 演示1：基本整数数组洗牌
        /// </summary>
        private static void Demo1_BasicShuffle()
        {
            Console.WriteLine("【演示1：基本整数数组洗牌】");
            Console.WriteLine();
            
            // 创建原始数组
            var originalArray = FisherYatesShuffle.GenerateSequentialArray(1, 15);
            Console.WriteLine("原始数组: [" + string.Join(", ", originalArray) + "]");
            
            // 进行多次洗牌演示
            for (int i = 1; i <= 3; i++)
            {
                var testArray = new int[originalArray.Length];
                Array.Copy(originalArray, testArray, originalArray.Length);
                
                FisherYatesShuffle.Shuffle(testArray, out int swapCount);
                
                Console.WriteLine($"洗牌 {i}: [" + string.Join(", ", testArray) + $"] (交换 {swapCount} 次)");
                
                // 计算与原数组的差异
                int differences = FisherYatesShuffle.CountDifferences(originalArray, testArray);
                Console.WriteLine($"        位置变化: {differences}/{originalArray.Length} 个元素");
            }
        }
        
        /// <summary>
        /// 演示2：扑克牌洗牌
        /// </summary>
        private static void Demo2_CardShuffle()
        {
            Console.WriteLine("【演示2：扑克牌洗牌】");
            Console.WriteLine();
            
            // 生成标准52张扑克牌
            var cardDeck = FisherYatesShuffle.GenerateCardDeck();
            Console.WriteLine("标准扑克牌组 (52张):");
            DisplayCards(cardDeck, "原始顺序");
            
            Console.WriteLine();
            
            // 洗牌
            FisherYatesShuffle.Shuffle(cardDeck, out int swapCount);
            DisplayCards(cardDeck, $"洗牌后 (交换 {swapCount} 次)");
            
            Console.WriteLine();
            Console.WriteLine("发牌演示 - 前10张牌:");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{cardDeck[i].PadRight(4)} ");
                if ((i + 1) % 5 == 0) Console.WriteLine();
            }
        }
        
        /// <summary>
        /// 演示3：种子洗牌（可重现结果）
        /// </summary>
        private static void Demo3_SeededShuffle()
        {
            Console.WriteLine("【演示3：种子洗牌（可重现结果）】");
            Console.WriteLine();
            
            var originalArray = FisherYatesShuffle.GenerateSequentialArray(1, 10);
            Console.WriteLine("原始数组: [" + string.Join(", ", originalArray) + "]");
            Console.WriteLine();
            
            // 使用相同种子进行多次洗牌
            int seed = 12345;
            Console.WriteLine($"使用种子 {seed} 进行洗牌:");
            
            for (int i = 1; i <= 3; i++)
            {
                var testArray = new int[originalArray.Length];
                Array.Copy(originalArray, testArray, originalArray.Length);
                
                FisherYatesShuffle.ShuffleWithSeed(testArray, seed, out int swapCount);
                
                Console.WriteLine($"第 {i} 次: [" + string.Join(", ", testArray) + $"] (交换 {swapCount} 次)");
            }
            
            Console.WriteLine();
            Console.WriteLine("注意：使用相同种子的结果完全一致！");
            
            // 使用不同种子
            Console.WriteLine();
            Console.WriteLine("使用不同种子的结果:");
            int[] seeds = { 1, 100, 9999 };
            
            foreach (int s in seeds)
            {
                var testArray = new int[originalArray.Length];
                Array.Copy(originalArray, testArray, originalArray.Length);
                
                FisherYatesShuffle.ShuffleWithSeed(testArray, s, out int swapCount);
                
                Console.WriteLine($"种子 {s}: [" + string.Join(", ", testArray) + $"] (交换 {swapCount} 次)");
            }
        }
        
        /// <summary>
        /// 演示4：部分洗牌
        /// </summary>
        private static void Demo4_PartialShuffle()
        {
            Console.WriteLine("【演示4：部分洗牌】");
            Console.WriteLine();
            
            var originalArray = FisherYatesShuffle.GenerateSequentialArray(1, 20);
            Console.WriteLine("原始数组: [" + string.Join(", ", originalArray) + "]");
            Console.WriteLine();
            
            // 部分洗牌演示
            int[] shuffleCounts = { 5, 10, 15 };
            
            foreach (int k in shuffleCounts)
            {
                var testArray = new int[originalArray.Length];
                Array.Copy(originalArray, testArray, originalArray.Length);
                
                FisherYatesShuffle.PartialShuffle(testArray, k);
                
                Console.WriteLine($"部分洗牌 (前{k}个): [" + string.Join(", ", testArray) + "]");
                
                // 标记被洗牌的部分
                Console.Write("洗牌范围标记:     ");
                for (int i = 0; i < testArray.Length; i++)
                {
                    if (i < testArray.Length - k)
                    {
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write(" ^ ");
                    }
                    if (testArray[i] >= 10) Console.Write(" ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        
        /// <summary>
        /// 演示5：统计特性测试
        /// </summary>
        private static void Demo5_StatisticalTest()
        {
            Console.WriteLine("【演示5：统计特性测试】");
            Console.WriteLine();
            
            Console.WriteLine("正在运行统计测试，请稍候...");
            Console.WriteLine();
            
            // 不同数组大小的测试
            int[] arraySizes = { 10, 20, 50 };
            int testCount = 1000;
            
            foreach (int size in arraySizes)
            {
                Console.WriteLine($"数组大小: {size}, 测试次数: {testCount}");
                
                var stats = FisherYatesShuffle.RunStatisticalTest(size, testCount);
                
                Console.WriteLine($"  平均交换次数: {stats.AverageSwaps:F2} (理论期望: ≈{(size - 1) * 0.5:F2})");
                Console.WriteLine($"  平均位置变化: {stats.AverageDifferences:F2} (理论期望: ≈{size * (1 - 1.0 / Math.E):F2})");
                Console.WriteLine($"  平均逆序对数: {stats.AverageInversions:F2} (理论期望: ≈{size * (size - 1) / 4.0:F2})");
                Console.WriteLine();
            }
            
            Console.WriteLine("统计结果说明:");
            Console.WriteLine("- 交换次数接近理论期望值说明算法效率正常");
            Console.WriteLine("- 位置变化数接近理论期望值说明洗牌充分");
            Console.WriteLine("- 逆序对数接近理论期望值说明随机性良好");
        }
        
        /// <summary>
        /// 显示扑克牌
        /// </summary>
        private static void DisplayCards(string[] cards, string title)
        {
            Console.WriteLine($"{title}:");
            
            for (int i = 0; i < cards.Length; i++)
            {
                Console.Write($"{cards[i].PadRight(4)} ");
                
                // 每行显示13张牌
                if ((i + 1) % 13 == 0)
                {
                    Console.WriteLine();
                }
            }
            
            if (cards.Length % 13 != 0)
            {
                Console.WriteLine();
            }
        }
    }
}