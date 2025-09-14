using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FisherYates
{
    /// <summary>
    /// Fisher-Yates洗牌算法实现类
    /// 提供多种洗牌方法和演示功能
    /// </summary>
    public static class FisherYatesShuffle
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// 标准Fisher-Yates洗牌算法（原地洗牌）
        /// </summary>
        /// <typeparam name="T">数组元素类型</typeparam>
        /// <param name="array">要洗牌的数组</param>
        /// <param name="swapCount">输出参数：交换次数统计</param>
        public static void Shuffle<T>(T[] array, out int swapCount)
        {
            swapCount = 0;
            
            // 从最后一个元素开始，向前遍历到第二个元素
            for (int i = array.Length - 1; i > 0; i--)
            {
                // 在 [0, i] 范围内随机选择一个索引
                int j = _random.Next(0, i + 1);
                
                // 交换 array[i] 和 array[j]
                if (i != j)
                {
                    (array[i], array[j]) = (array[j], array[i]);
                    swapCount++;
                }
            }
        }

        /// <summary>
        /// 标准Fisher-Yates洗牌算法（简化版本，不统计交换次数）
        /// </summary>
        /// <typeparam name="T">数组元素类型</typeparam>
        /// <param name="array">要洗牌的数组</param>
        public static void Shuffle<T>(T[] array)
        {
            Shuffle(array, out _);
        }

        /// <summary>
        /// 创建洗牌后的新数组（不修改原数组）
        /// </summary>
        /// <typeparam name="T">数组元素类型</typeparam>
        /// <param name="source">源数组</param>
        /// <returns>洗牌后的新数组</returns>
        public static T[] CreateShuffled<T>(T[] source)
        {
            T[] result = new T[source.Length];
            Array.Copy(source, result, source.Length);
            Shuffle(result);
            return result;
        }

        /// <summary>
        /// 部分洗牌：只洗牌数组的前k个元素
        /// </summary>
        /// <typeparam name="T">数组元素类型</typeparam>
        /// <param name="array">要洗牌的数组</param>
        /// <param name="k">要洗牌的元素个数</param>
        public static void PartialShuffle<T>(T[] array, int k)
        {
            if (k > array.Length) k = array.Length;
            if (k <= 1) return;

            // 只处理后k个位置
            for (int i = array.Length - 1; i >= array.Length - k; i--)
            {
                int j = _random.Next(0, i + 1);
                if (i != j)
                {
                    (array[i], array[j]) = (array[j], array[i]);
                }
            }
        }

        /// <summary>
        /// 使用指定种子的洗牌（可重现结果）
        /// </summary>
        /// <typeparam name="T">数组元素类型</typeparam>
        /// <param name="array">要洗牌的数组</param>
        /// <param name="seed">随机数种子</param>
        /// <param name="swapCount">输出参数：交换次数统计</param>
        public static void ShuffleWithSeed<T>(T[] array, int seed, out int swapCount)
        {
            var seededRandom = new Random(seed);
            swapCount = 0;

            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = seededRandom.Next(0, i + 1);
                if (i != j)
                {
                    (array[i], array[j]) = (array[j], array[i]);
                    swapCount++;
                }
            }
        }

        /// <summary>
        /// 生成指定范围的连续整数数组
        /// </summary>
        /// <param name="start">起始值</param>
        /// <param name="count">元素个数</param>
        /// <returns>连续整数数组</returns>
        public static int[] GenerateSequentialArray(int start, int count)
        {
            return Enumerable.Range(start, count).ToArray();
        }

        /// <summary>
        /// 生成指定长度的字符串数组（用于演示）
        /// </summary>
        /// <param name="length">数组长度</param>
        /// <returns>字符串数组</returns>
        public static string[] GenerateStringArray(int length)
        {
            var result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = $"Item_{i + 1}";
            }
            return result;
        }

        /// <summary>
        /// 生成扑克牌数组（用于演示）
        /// </summary>
        /// <returns>包含52张牌的数组</returns>
        public static string[] GenerateCardDeck()
        {
            var suits = new[] { "♠", "♥", "♦", "♣" };
            var ranks = new[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            var deck = new List<string>();

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add($"{rank}{suit}");
                }
            }

            return deck.ToArray();
        }

        /// <summary>
        /// 验证洗牌结果的随机性（简单统计测试）
        /// </summary>
        /// <param name="originalArray">原始数组</param>
        /// <param name="shuffledArray">洗牌后的数组</param>
        /// <returns>不同位置的元素个数</returns>
        public static int CountDifferences<T>(T[] originalArray, T[] shuffledArray)
        {
            if (originalArray.Length != shuffledArray.Length)
                throw new ArgumentException("数组长度不匹配");

            int differences = 0;
            for (int i = 0; i < originalArray.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(originalArray[i], shuffledArray[i]))
                {
                    differences++;
                }
            }
            return differences;
        }

        /// <summary>
        /// 计算数组的"逆序对"数量（用于评估随机性）
        /// </summary>
        /// <param name="array">整数数组</param>
        /// <returns>逆序对数量</returns>
        public static int CountInversions(int[] array)
        {
            int inversions = 0;
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[i] > array[j])
                    {
                        inversions++;
                    }
                }
            }
            return inversions;
        }

        /// <summary>
        /// 批量测试洗牌算法的统计特性
        /// </summary>
        /// <param name="arraySize">数组大小</param>
        /// <param name="testCount">测试次数</param>
        /// <returns>统计结果</returns>
        public static ShuffleStatistics RunStatisticalTest(int arraySize, int testCount)
        {
            var stats = new ShuffleStatistics();
            var originalArray = GenerateSequentialArray(1, arraySize);
            
            int totalSwaps = 0;
            int totalDifferences = 0;
            int totalInversions = 0;

            for (int test = 0; test < testCount; test++)
            {
                var testArray = new int[arraySize];
                Array.Copy(originalArray, testArray, arraySize);
                
                Shuffle(testArray, out int swaps);
                
                totalSwaps += swaps;
                totalDifferences += CountDifferences(originalArray, testArray);
                totalInversions += CountInversions(testArray);
            }

            stats.AverageSwaps = (double)totalSwaps / testCount;
            stats.AverageDifferences = (double)totalDifferences / testCount;
            stats.AverageInversions = (double)totalInversions / testCount;
            stats.TestCount = testCount;
            stats.ArraySize = arraySize;

            return stats;
        }
    }

    /// <summary>
    /// 洗牌算法统计结果
    /// </summary>
    public class ShuffleStatistics
    {
        public double AverageSwaps { get; set; }
        public double AverageDifferences { get; set; }
        public double AverageInversions { get; set; }
        public int TestCount { get; set; }
        public int ArraySize { get; set; }

        public override string ToString()
        {
            return $"统计结果 (数组大小: {ArraySize}, 测试次数: {TestCount}):\n" +
                   $"  平均交换次数: {AverageSwaps:F2}\n" +
                   $"  平均位置变化: {AverageDifferences:F2}\n" +
                   $"  平均逆序对数: {AverageInversions:F2}";
        }
    }
}