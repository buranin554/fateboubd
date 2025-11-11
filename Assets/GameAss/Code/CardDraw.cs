using System.Collections.Generic;
using UnityEngine;

public class CardDrawSystem : MonoBehaviour
{
    [Header("Prefab ไพ่ (SpriteRenderer)")]
    public GameObject cardPrefab;

    [Header("ตำแหน่งวางไพ่ (Slots)")]
    public Transform[] handSlots;

    [Header("Deck (สำรับไพ่ทั้งหมด)")] public GameObject cardPrefab2;
    public List<GameObject> deck = new List<GameObject>();

    [Header("ตั้งค่าการจั่ว")]
    public int startHand = 7;
    public int maxHandSize = 12;

    public GameManager gameManager; 
    private List<CardSelectable> hand = new List<CardSelectable>();
    public List<CardSelectable> handCards = new List<CardSelectable>();

    public void Initialize()
    {
        ShuffleDeck();
        DrawCards(startHand);   // จั่วเปิดมือ 7 ใบ
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rand = Random.Range(i, deck.Count);
            (deck[i], deck[rand]) = (deck[rand], deck[i]);
        }
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (deck.Count == 0 || hand.Count >= maxHandSize) break;

            GameObject cardToDraw = deck[0];
            deck.RemoveAt(0);

            GameObject newCard = Instantiate(cardToDraw, handSlots[hand.Count].position, handSlots[hand.Count].rotation);
            newCard.transform.SetParent(handSlots[hand.Count], false);

            CardSelectable selectable = newCard.GetComponent<CardSelectable>();
            if (selectable == null) selectable = newCard.AddComponent<CardSelectable>();

            selectable.owner = this;
            selectable.UpdateOriginalPosition(handSlots[hand.Count].position);

            hand.Add(selectable);
        }
       
        ReorderHand();
    }

    public void ReorderHand()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            CardSelectable card = hand[i];
            Transform slot = handSlots[i];

            card.transform.position = slot.position;
            card.transform.rotation = slot.rotation;
            card.transform.SetParent(slot);

            card.UpdateOriginalPosition(slot.position);
            card.SetSelected(false);
        }
    }

    public void RemoveFromHand(CardSelectable card)
    {
        if (card != null && hand.Contains(card))
        {
            hand.Remove(card);
            Destroy(card.gameObject);
        }
        ReorderHand();
    }
    // ✅ ฟังก์ชันเผาไพ่ทั้งหมดในมือ
    public int BurnAllHand()
    {
        int burnedCount = hand.Count;

        // ลบการ์ดทั้งหมดจากมือ
        foreach (var card in hand)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }
        }

        // เคลียร์รายชื่อในระบบ
        hand.Clear();

        // เรียงมือใหม่ (จะว่างเปล่า)
        ReorderHand();

        // อัปเดต UI
        if (gameManager != null)
            gameManager.UpdateUI();

        Debug.Log($"[CardDrawSystem] Burned {burnedCount} cards.");
        return burnedCount; // คืนค่าจำนวนที่ถูกเผา
    }

}
