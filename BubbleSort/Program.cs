using System;

namespace algo_practice
{
    /// <summary>
    /// 冒泡排序算法实现类
    /// 冒泡排序是一种简单的排序算法，通过重复遍历要排序的数列，
    /// 一次比较两个元素，如果它们的顺序错误就把它们交换过来。
    /// </summary>
    internal static class BubbleSort
    {
        /// <summary>
        /// 原地冒泡排序，带提前退出优化和统计信息
        /// </summary>
        /// <param name="arr">待排序的整数数组</param>
        /// <param name="ascending">是否升序排列，true为升序，false为降序</param>
        /// <returns>返回排序统计信息：(遍历轮数, 比较次数, 交换次数)</returns>
        public static (int passes, int comparisons, int swaps) Sort(int[] arr, bool ascending = true)
        {
            int n = arr.Length;          // 数组长度
            int comparisons = 0;         // 比较次数统计
            int swaps = 0;              // 交换次数统计
            int passes = 0;             // 遍历轮数统计

            // 外层循环：控制排序的轮数，每轮确定一个元素的最终位置
            // 最多需要 n-1 轮，因为最后一个元素会自动就位
            for (int i = 0; i < n - 1; i++)
            {
                bool swappedInPass = false;  // 标记本轮是否发生过交换
                
                // 内层循环：在未排序部分进行相邻元素比较和交换
                // 每轮结束后，最大（或最小）元素会"冒泡"到正确位置
                // n-1-i 是因为每轮后末尾的 i 个元素已经排好序
                for (int j = 0; j < n - 1 - i; j++)
                {
                    comparisons++;  // 每次比较都计数
                    
                    // 判断相邻两个元素是否需要交换
                    // 升序：左边 > 右边时交换；降序：左边 < 右边时交换
                    bool outOfOrder = ascending ? arr[j] > arr[j + 1] : arr[j] < arr[j + 1];
                    
                    if (outOfOrder)
                    {
                        // 使用元组交换语法，简洁地交换两个元素
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                        swaps++;              // 交换次数计数
                        swappedInPass = true; // 标记本轮发生了交换
                    }
                }
                
                passes++;  // 完成一轮遍历
                
                // 提前退出优化：如果某一轮没有发生任何交换，
                // 说明数组已经完全有序，可以提前结束排序
                if (!swappedInPass) break;
            }

            // 返回排序过程的统计信息
            return (passes, comparisons, swaps);
        }
    }

    /// <summary>
    /// 辅助工具类，提供数组生成等实用方法
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// 生成指定长度和数值范围的随机整数数组
        /// </summary>
        /// <param name="length">数组长度</param>
        /// <param name="minInclusive">最小值（包含）</param>
        /// <param name="maxInclusive">最大值（包含）</param>
        /// <returns>填充了随机整数的数组</returns>
        public static int[] GenerateRandomArray(int length, int minInclusive, int maxInclusive)
        {
            var rand = Random.Shared;    // 使用共享的随机数生成器
            var arr = new int[length];   // 创建指定长度的数组
            
            // 用随机数填充数组的每个位置
            for (int i = 0; i < length; i++)
            {
                // Random.Next(min, max+1) 生成 [min, max] 范围内的随机整数
                arr[i] = rand.Next(minInclusive, maxInclusive + 1);
            }
            
            return arr;
        }
    }

    /// <summary>
    /// 程序主入口类，演示冒泡排序算法的使用
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// 程序主入口方法
        /// 生成随机数组，分别进行升序和降序冒泡排序，并输出结果和统计信息
        /// </summary>
        static void Main()
        {
            // 设置控制台编码为UTF-8，解决中文显示乱码问题
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            
            Console.WriteLine("=== 冒泡排序练习 ===");

            // 生成一个包含10个随机整数的数组，数值范围为0-99
            int[] original = Helpers.GenerateRandomArray(length: 10, minInclusive: 0, maxInclusive: 99);
            Console.WriteLine("原始数组   : " + string.Join(", ", original));

            // 克隆原数组进行升序排序
            var asc = (int[])original.Clone();
            var ascStats = BubbleSort.Sort(asc, ascending: true);
            Console.WriteLine("升序排序   : " + string.Join(", ", asc));
            Console.WriteLine($"升序统计   -> 轮数: {ascStats.passes}, 比较: {ascStats.comparisons}, 交换: {ascStats.swaps}");

            // 克隆原数组进行降序排序
            var desc = (int[])original.Clone();
            var descStats = BubbleSort.Sort(desc, ascending: false);
            Console.WriteLine("降序排序   : " + string.Join(", ", desc));
            Console.WriteLine($"降序统计   -> 轮数: {descStats.passes}, 比较: {descStats.comparisons}, 交换: {descStats.swaps}");
        }
    }
}
