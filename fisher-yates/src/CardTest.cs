using System;
using FisherYates;

/// <summary>
/// 扑克牌测试程序，用于验证所有花色是否正确生成
/// </summary>
public class CardTest
{
    public static void TestCardGeneration()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        Console.WriteLine("=== 扑克牌生成测试 ===");
        
        var cards = FisherYatesShuffle.GenerateCardDeck();
        
        Console.WriteLine($"总共生成了 {cards.Length} 张牌");
        Console.WriteLine();
        
        // 按花色分组显示
        var spades = new System.Collections.Generic.List<string>();
        var hearts = new System.Collections.Generic.List<string>();
        var diamonds = new System.Collections.Generic.List<string>();
        var clubs = new System.Collections.Generic.List<string>();
        
        foreach (var card in cards)
        {
            if (card.Contains("♠"))
                spades.Add(card);
            else if (card.Contains("♥"))
                hearts.Add(card);
            else if (card.Contains("♦"))
                diamonds.Add(card);
            else if (card.Contains("♣"))
                clubs.Add(card);
        }
        
        Console.WriteLine($"黑桃 ♠ ({spades.Count} 张): {string.Join(", ", spades)}");
        Console.WriteLine();
        Console.WriteLine($"红桃 ♥ ({hearts.Count} 张): {string.Join(", ", hearts)}");
        Console.WriteLine();
        Console.WriteLine($"方块 ♦ ({diamonds.Count} 张): {string.Join(", ", diamonds)}");
        Console.WriteLine();
        Console.WriteLine($"梅花 ♣ ({clubs.Count} 张): {string.Join(", ", clubs)}");
        Console.WriteLine();
        
        // 验证每种花色是否都有13张牌
        Console.WriteLine("=== 验证结果 ===");
        Console.WriteLine($"黑桃数量: {spades.Count} (应该是13)");
        Console.WriteLine($"红桃数量: {hearts.Count} (应该是13)");
        Console.WriteLine($"方块数量: {diamonds.Count} (应该是13)");
        Console.WriteLine($"梅花数量: {clubs.Count} (应该是13)");
        
        bool allCorrect = spades.Count == 13 && hearts.Count == 13 && 
                         diamonds.Count == 13 && clubs.Count == 13;
        
        Console.WriteLine($"\n所有花色都正确: {(allCorrect ? "是" : "否")}");
        
        if (!allCorrect)
        {
            Console.WriteLine("\n可能的问题:");
            Console.WriteLine("1. Unicode 字符显示问题");
            Console.WriteLine("2. 字体不支持某些符号");
            Console.WriteLine("3. 控制台编码设置问题");
        }
    }
}