using System.Collections.Generic;
using UnityEngine;
using static CardComboChecker;  // ใช้ Enum ComboType
using static CardSelectable;

public class CardSelectManager : MonoBehaviour
{
    public static CardSelectManager Instance;

    public List<CardSelectable> selectedCards = new List<CardSelectable>();
    public CardDrawSystem cardDrawSystem;
    public GameManager gameManager; // ต้องเชื่อมจาก Inspector

    private void Awake()
    {
        Instance = this;
    }

    public void SelectCard(CardSelectable card)
    {
        // ถ้าเป็นการ์ดพิเศษและมีไพ่ปกติถูกเลือกอยู่แล้ว (หรือกลับกัน) จะไม่ให้เลือกปนกัน
        bool hasNormal = selectedCards.Exists(c => !c.IsSpecial);
        bool hasSpecial = selectedCards.Exists(c => c.IsSpecial);

        if (card.IsSpecial && hasNormal) { Debug.Log("⚠️ เลือกการ์ดพิเศษพร้อมไพ่ปกติไม่ได้"); return; }
        if (!card.IsSpecial && hasSpecial) { Debug.Log("⚠️ เลือกไพ่ปกติพร้อมการ์ดพิเศษไม่ได้"); return; }

        // toggle เลือก/ยกเลิก ตามเดิม
        if (selectedCards.Contains(card))
        {
            card.SetSelected(false);
            selectedCards.Remove(card);
        }
        else
        {
            card.SetSelected(true);
            selectedCards.Add(card);
        }
    }

    void EvaluateCombo(List<CardSelectable> comboCards)
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager ยังไม่ได้เชื่อม!");
            return;
        }

        // ✅ ถ้ามีการ์ดพิเศษใบเดียว ให้ใช้ได้ทันที
        if (comboCards.Count == 1 && comboCards[0].cardType == CardType.Special)
        {
            var specialCard = comboCards[0];
            Debug.Log($"🔥 ใช้การ์ดพิเศษ: {specialCard.name}");
            specialCard.UseCard();

            selectedCards.Clear();
            cardDrawSystem?.ReorderHand();
            gameManager.UpdateUI();
            return;
        }

        // ✅ ตรวจคอมโบ
        ComboType combo = CardComboChecker.CheckCombo(comboCards);
        int score = CardComboChecker.GetScore(combo);

        // ✅ ตรวจจำนวนขั้นต่ำของคอมโบ
        int minCount = GetMinimumCardCountForCombo(combo);

        if (combo == ComboType.None || score <= 0)
        {
            Debug.Log("❌ ไม่มีคอมโบ");
            return;
        }

        if (comboCards.Count < minCount)
        {
            Debug.Log($"❌ ต้องใช้ไพ่ {minCount} ใบขึ้นไป สำหรับคอมโบ {combo}");
            return;
        }

        // ✅ ผ่านเงื่อนไข → ใช้คอมโบ
        Debug.Log($"✅ พบคอมโบ: {combo} +{score}");
        gameManager.AddScore(score);

        foreach (var c in comboCards)
            c.UseCard();

        selectedCards.RemoveAll(c => comboCards.Contains(c));
        cardDrawSystem?.ReorderHand();
        gameManager.UpdateUI();
    }

    private int GetMinimumCardCountForCombo(CardComboChecker.ComboType combo)
    {
        switch (combo)
        {
            case CardComboChecker.ComboType.RoyalFlush:
            case CardComboChecker.ComboType.StraightFlush:
            case CardComboChecker.ComboType.Straight:
            case CardComboChecker.ComboType.Flush:
            case CardComboChecker.ComboType.FullHouse:
                return 5;

            case CardComboChecker.ComboType.FourOfKind:
            case CardComboChecker.ComboType.TwoPair:
                return 4;

            case CardComboChecker.ComboType.ThreeOfKind:
                return 3;

            case CardComboChecker.ComboType.SinglePair:
                return 2;

            default:
                return 0;
        }
    }

    // ✅ สำหรับกรณีผู้เล่นกดใช้งานเอง (เช่น E)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ConfirmSelection();
        }
    }

    public void UseSelectedCards()
    {
        if (selectedCards.Count == 0)
        {
            Debug.Log("❌ ยังไม่ได้เลือกไพ่");
            return;
        }

        // ✅ เรียก EvaluateCombo เพื่อให้ระบบคอมโบ + การ์ดพิเศษทำงานในที่เดียว
        EvaluateCombo(new List<CardSelectable>(selectedCards));
    }

    public void ConfirmSelection()
    {
        // ✅ ถ้ามีการ์ดพิเศษใบเดียว → ใช้ได้ทันที
        if (selectedCards.Count == 1 && selectedCards[0].IsSpecial)
        {
            selectedCards[0].UseCard();
            selectedCards.Clear();
            Debug.Log("✨ ใช้การ์ดพิเศษใบเดียวเรียบร้อย");
            return;
        }

        // ✅ กรองเฉพาะไพ่ปกติไปเช็คคอมโบ
        List<CardSelectable> normals = selectedCards.FindAll(c => !c.IsSpecial);

        if (normals.Count < 2)
        {
            Debug.Log("❌ ต้องเลือกอย่างน้อย 2 ใบที่เป็นไพ่ปกติ (ไม่ใช่การ์ดพิเศษ)");
            return;
        }

        EvaluateCombo(normals);
    }
}
