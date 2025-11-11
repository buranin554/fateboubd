using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CardSelectable;

public static class CardComboChecker
{
    public enum ComboType
    {
        None,
        SinglePair,
        TwoPair,
        ThreeOfKind,
        Straight,
        Flush,
        FullHouse,
        FourOfKind,
        StraightFlush,
        RoyalFlush
    }

    public static ComboType CheckCombo(List<CardSelectable> selectedCards)
    {
        if (selectedCards == null || selectedCards.Count == 0)
            return ComboType.None;

        // ✅ ถ้ามีการ์ดพิเศษเพียงใบเดียว — ถือว่า valid
        if (selectedCards.Count == 1 && selectedCards[0].cardType == CardType.Special)
        {
            Debug.Log("✨ การ์ดพิเศษใบเดียว ใช้งานได้");
            return ComboType.None; // คืน None แต่อย่า block
        }

        if (selectedCards.Count < 2)
            return ComboType.None;

        // ต่อด้วยโค้ดคอมโบเดิม...
        List<int> values = selectedCards.Select(c => c.cardValue).OrderBy(v => v).ToList();
        List<CardSuit> suits = selectedCards.Select(c => c.cardSuit).ToList();

        bool isFlush = suits.All(s => s == suits[0]);
        bool isStraight = IsStraight(values);
        Dictionary<int, int> groups = values.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());

        if (isFlush && IsRoyal(values)) return ComboType.RoyalFlush;
        if (isFlush && isStraight) return ComboType.StraightFlush;
        if (groups.ContainsValue(4)) return ComboType.FourOfKind;
        if (groups.ContainsValue(3) && groups.ContainsValue(2)) return ComboType.FullHouse;
        if (isFlush) return ComboType.Flush;
        if (isStraight) return ComboType.Straight;
        if (groups.ContainsValue(3)) return ComboType.ThreeOfKind;
        if (groups.Values.Count(v => v == 2) == 2) return ComboType.TwoPair;
        if (groups.ContainsValue(2)) return ComboType.SinglePair;

        return ComboType.None;
    }


    private static bool IsStraight(List<int> values)
    {
        for (int i = 0; i < values.Count - 1; i++)
        {
            if (values[i + 1] != values[i] + 1)
                return false;
        }
        return true;
    }

    private static bool IsRoyal(List<int> values)
    {
        int[] royal = { 10, 11, 12, 13, 14 };
        return values.SequenceEqual(royal);
    }

    public static int GetScore(ComboType combo)
    {
        switch (combo)
        {
            case ComboType.RoyalFlush: return 2000;
            case ComboType.StraightFlush: return 1000;
            case ComboType.FourOfKind: return 850;
            case ComboType.FullHouse: return 800;
            case ComboType.Flush: return 700;
            case ComboType.Straight: return 650;
            case ComboType.ThreeOfKind: return 500;
            case ComboType.TwoPair: return 450 ;
            case ComboType.SinglePair: return 200;
            default: return 0;
        }
    }


}
