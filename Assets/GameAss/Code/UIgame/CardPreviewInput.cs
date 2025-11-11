using UnityEngine;

public class CardPreviewInput : MonoBehaviour
{
    [Header("Key Settings")]
    public KeyCode showKey = KeyCode.Q;
    public KeyCode hideKey = KeyCode.X;

    [Header("Debug")]
    public CardSelectable selectedCard;

    void Update()
    {
        // ค้นหาการ์ดที่ถูกเลือก (เลือกใบเดียวในตอนนี้)
        if (selectedCard == null)
        {
            var allCards = FindObjectsOfType<CardSelectable>();
            foreach (var card in allCards)
            {
                if (card.isSelected)
                {
                    selectedCard = card;
                    break;
                }
            }
        }

        // กด Q เพื่อดูไพ่
        if (Input.GetKeyDown(showKey) && selectedCard != null)
        {
            if (CardPreviewUI.Instance != null)
            {
                CardPreviewUI.Instance.Show(
                    GetCardSprite(selectedCard),
                    selectedCard.cardName,
                    selectedCard.cardDescription
                );
                Debug.Log($"🃏 เปิดดูการ์ด: {selectedCard.cardName}");
            }
            else
            {
                Debug.LogError("❌ CardPreviewUI.Instance ยังไม่มีใน Scene!");
            }
        }

        // กด X เพื่อปิด
        if (Input.GetKeyDown(hideKey))
        {
            if (CardPreviewUI.Instance != null)
                CardPreviewUI.Instance.Hide();

            selectedCard = null;
        }
    }

    private Sprite GetCardSprite(CardSelectable card)
    {
        if (card.TryGetComponent(out SpriteRenderer sr))
            return sr.sprite;
        return null;
    }
}
