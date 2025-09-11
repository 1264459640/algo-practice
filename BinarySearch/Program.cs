using System;

namespace BinarySearch
{
    /// <summary>
    /// 二分查找算法实现类
    /// 二分查找是一种在有序数组中查找特定元素的搜索算法。
    /// 它通过重复将搜索区间分成两半来工作，每次比较都能排除一半的元素。
    /// </summary>
    internal static class BinarySearch
    {
        /// <summary>
        /// 标准二分查找：在有序数组中查找目标值的索引
        /// </summary>
        /// <param name="arr">已排序的整数数组（升序）</param>
        /// <param name="target">要查找的目标值</param>
        /// <returns>目标值的索引，如果未找到返回-1</returns>
        public static (int index, int comparisons) Search(int[] arr, int target)
        {
            int left = 0;                    // 左边界（包含）
            int right = arr.Length - 1;      // 右边界（包含）
            int comparisons = 0;             // 比较次数统计

            // 当搜索区间有效时继续查找
            while (left <= right)
            {
                // 计算中间位置，使用 (left + right) / 2 的安全版本
                // 避免整数溢出：left + (right - left) / 2
                int mid = left + (right - left) / 2;
                comparisons++;

                // 找到目标值
                if (arr[mid] == target)
                {
                    return (mid, comparisons);
                }
                // 目标值在左半部分
                else if (arr[mid] > target)
                {
                    right = mid - 1;  // 缩小搜索范围到左半部分
                }
                // 目标值在右半部分
                else
                {
                    left = mid + 1;   // 缩小搜索范围到右半部分
                }
            }

            // 未找到目标值
            return (-1, comparisons);
        }

        /// <summary>
        /// 查找第一个出现的位置（处理重复元素）
        /// </summary>
        /// <param name="arr">已排序的整数数组</param>
        /// <param name="target">要查找的目标值</param>
        /// <returns>第一个出现位置的索引，如果未找到返回-1</returns>
        public static (int index, int comparisons) FindFirst(int[] arr, int target)
        {
            int left = 0;
            int right = arr.Length - 1;
            int comparisons = 0;
            int result = -1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                comparisons++;

                if (arr[mid] == target)
                {
                    result = mid;      // 记录找到的位置
                    right = mid - 1;   // 继续在左半部分查找更早的出现
                }
                else if (arr[mid] > target)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return (result, comparisons);
        }

        /// <summary>
        /// 查找最后一个出现的位置（处理重复元素）
        /// </summary>
        /// <param name="arr">已排序的整数数组</param>
        /// <param name="target">要查找的目标值</param>
        /// <returns>最后一个出现位置的索引，如果未找到返回-1</returns>
        public static (int index, int comparisons) FindLast(int[] arr, int target)
        {
            int left = 0;
            int right = arr.Length - 1;
            int comparisons = 0;
            int result = -1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                comparisons++;

                if (arr[mid] == target)
                {
                    result = mid;      // 记录找到的位置
                    left = mid + 1;    // 继续在右半部分查找更晚的出现
                }
                else if (arr[mid] > target)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return (result, comparisons);
        }
    }

    /// <summary>
    /// 辅助工具类，提供数组生成和显示功能
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// 生成指定长度的有序随机数组
        /// </summary>
        /// <param name="length">数组长度</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>升序排列的随机整数数组</returns>
        public static int[] GenerateSortedArray(int length, int minValue, int maxValue)
        {
            var rand = Random.Shared;
            var arr = new int[length];
            
            // 生成随机数
            for (int i = 0; i < length; i++)
            {
                arr[i] = rand.Next(minValue, maxValue + 1);
            }
            
            // 排序以满足二分查找的前提条件
            Array.Sort(arr);
            return arr;
        }

        /// <summary>
        /// 生成包含重复元素的有序数组（用于测试查找第一个/最后一个位置）
        /// </summary>
        /// <returns>包含重复元素的有序数组</returns>
        public static int[] GenerateArrayWithDuplicates()
        {
            return new int[] { 1, 2, 2, 2, 3, 4, 4, 5, 6, 6, 6, 6, 7, 8, 9 };
        }

        /// <summary>
        /// 显示数组内容，突出显示指定索引
        /// </summary>
        /// <param name="arr">要显示的数组</param>
        /// <param name="highlightIndex">要突出显示的索引，-1表示不突出显示</param>
        /// <returns>格式化的数组字符串</returns>
        public static string DisplayArray(int[] arr, int highlightIndex = -1)
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == highlightIndex)
                {
                    result.Append($"[{arr[i]}]");
                }
                else
                {
                    result.Append(arr[i].ToString());
                }
                
                if (i < arr.Length - 1)
                {
                    result.Append(", ");
                }
            }
            return result.ToString();
        }
    }

    /// <summary>
    /// 程序主入口类，演示二分查找算法的各种用法
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// 程序主入口方法
        /// 演示标准二分查找、查找第一个位置、查找最后一个位置等功能
        /// </summary>
        static void Main()
        {
            // 设置控制台编码为UTF-8，解决中文显示乱码问题
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            
            Console.WriteLine("=== 二分查找算法练习 ===");
            Console.WriteLine();

            // 演示1：标准二分查找
            Console.WriteLine("【演示1：标准二分查找】");
            int[] sortedArray = Helpers.GenerateSortedArray(15, 1, 50);
            Console.WriteLine("有序数组: " + string.Join(", ", sortedArray));
            
            // 查找存在的元素
            int target1 = sortedArray[7];  // 选择数组中间的一个元素
            var result1 = BinarySearch.Search(sortedArray, target1);
            Console.WriteLine($"查找 {target1}: 索引 = {result1.index}, 比较次数 = {result1.comparisons}");
            Console.WriteLine("结果数组: " + Helpers.DisplayArray(sortedArray, result1.index));
            
            // 查找不存在的元素
            int target2 = 100;
            var result2 = BinarySearch.Search(sortedArray, target2);
            Console.WriteLine($"查找 {target2}: 索引 = {result2.index}, 比较次数 = {result2.comparisons}");
            Console.WriteLine();

            // 演示2：处理重复元素
            Console.WriteLine("【演示2：处理重复元素】");
            int[] arrayWithDuplicates = Helpers.GenerateArrayWithDuplicates();
            Console.WriteLine("有重复元素的数组: " + string.Join(", ", arrayWithDuplicates));
            
            int target3 = 6;  // 选择一个重复的元素
            var firstResult = BinarySearch.FindFirst(arrayWithDuplicates, target3);
            var lastResult = BinarySearch.FindLast(arrayWithDuplicates, target3);
            var standardResult = BinarySearch.Search(arrayWithDuplicates, target3);
            
            Console.WriteLine($"查找 {target3}:");
            Console.WriteLine($"  标准查找: 索引 = {standardResult.index}, 比较次数 = {standardResult.comparisons}");
            Console.WriteLine($"  第一个位置: 索引 = {firstResult.index}, 比较次数 = {firstResult.comparisons}");
            Console.WriteLine($"  最后位置: 索引 = {lastResult.index}, 比较次数 = {lastResult.comparisons}");
            Console.WriteLine("第一个位置: " + Helpers.DisplayArray(arrayWithDuplicates, firstResult.index));
            Console.WriteLine("最后位置: " + Helpers.DisplayArray(arrayWithDuplicates, lastResult.index));
            Console.WriteLine();

            // 演示3：时间复杂度对比
            Console.WriteLine("【演示3：时间复杂度分析】");
            int[] largeArray = Helpers.GenerateSortedArray(1000, 1, 10000);
            int targetInLarge = largeArray[500];  // 选择中间位置的元素
            var largeResult = BinarySearch.Search(largeArray, targetInLarge);
            
            Console.WriteLine($"在1000个元素的数组中查找:");
            Console.WriteLine($"目标值: {targetInLarge}, 找到位置: {largeResult.index}, 比较次数: {largeResult.comparisons}");
            Console.WriteLine($"理论最大比较次数: {Math.Ceiling(Math.Log2(largeArray.Length))} (log₂{largeArray.Length})");
            Console.WriteLine($"线性查找需要比较次数: {largeResult.index + 1} (最坏情况: {largeArray.Length})");
            Console.WriteLine($"效率提升: {(double)(largeResult.index + 1) / largeResult.comparisons:F1}倍");
        }
    }
}