using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class CardSelectable : MonoBehaviour
{
    public enum CardType
    {
        Normal,
        Special
    }

    [Header("Card Config")]
    public CardType cardType = CardType.Normal;
    public int cardValue;
    public CardSuit cardSuit;
    public bool IsSpecial => cardType == CardType.Special;
    public bool isComboCard = true;
    public GameObject glowEffect;

    private Renderer render;
    private Color originalColor;
    public Color selectedColor = Color.yellow;

    [Header("Card Score")]
    [Tooltip("คะแนนที่จะได้เมื่อใช้การ์ดใบนี้")]
    public int cardScore = 100;

    [Header("Card Info")]
    public Sprite cardSprite; // สำหรับดูรายละเอียด
    public string cardName = "Unknown Card";
    [TextArea] public string cardDescription = "รายละเอียดของการ์ดนี้";

    [HideInInspector] public CardDrawSystem owner;  // อ้างอิงเจ้าของ
    private Vector3 originalPosition;
    public float selectOffset = 0.5f;
    public bool isSelected = false;

    private void Start()
    {
        render = GetComponent<Renderer>();
        if (render != null)
            originalColor = render.material.color;

        originalPosition = transform.position;

        // เพิ่ม Collider อัตโนมัติถ้ายังไม่มี
        if (!TryGetComponent(out Collider _))
        {
            gameObject.AddComponent<BoxCollider>();
        }

        // สร้างเอฟเฟกต์ Glow เริ่มต้นถ้ายังไม่มี
        if (glowEffect == null)
            CreateDefaultGlow();
    }

    private void Update()
    {
        // ✅ ถ้าไพ่ถูกเลือก และผู้เล่นกด Q หรือ X → แสดงรายละเอียดการ์ด
        if (isSelected && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.X)))
        {
            if (CardPreviewUI.Instance != null)
                CardPreviewUI.Instance.Show(cardSprite, cardName, cardDescription);
            else
                Debug.LogWarning("CardPreviewUI.Instance ยังไม่ได้เซ็ตใน Scene!");
        }
    }

    private void OnMouseDown()
    {
        // คลิกแล้วส่งไปให้ CardSelectManager จัดการเลือก
        CardSelectManager.Instance.SelectCard(this);
    }

    public void SetSelected(bool select)
    {
        isSelected = select;

        // เปลี่ยนสี
        if (render != null)
            render.material.color = isSelected ? selectedColor : originalColor;

        // ยกตำแหน่งขึ้น
        transform.position = isSelected
            ? originalPosition + Vector3.up * selectOffset
            : originalPosition;

        // เปิด/ปิดเอฟเฟกต์ Glow
        SetGlow(isSelected);
    }

    public void UpdateOriginalPosition(Vector3 newPos)
    {
        originalPosition = newPos;
        if (!isSelected)
            transform.position = newPos;
    }

    // ✅ ใช้ไพ่: เพิ่มคะแนน แล้วนำไพ่ออกจากมือ
    public virtual void UseCard()
    {
        Debug.Log($"ใช้การ์ด: {gameObject.name} | ได้คะแนน: {cardScore}");

        // เพิ่มคะแนนให้เกม
        if (owner != null && owner.gameManager != null && cardScore != 0)
        {
            owner.gameManager.AddScore(cardScore);
        }

        // เอาไพ่ออกจากมือ
        if (owner != null)
        {
            owner.RemoveFromHand(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetGlow(bool active)
    {
        if (glowEffect != null)
            glowEffect.SetActive(active);
    }

    private void CreateDefaultGlow()
    {
        glowEffect = GameObject.CreatePrimitive(PrimitiveType.Quad);
        glowEffect.name = "GlowEffect";
        glowEffect.transform.SetParent(this.transform);
        glowEffect.transform.localPosition = new Vector3(0, 0, 0.1f);
        glowEffect.transform.localScale = new Vector3(1.2f, 1.2f, 1f);

        var renderer = glowEffect.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        renderer.material.color = new Color(1f, 0.85f, 0.2f, 0.5f); // สีทองโปร่ง

        glowEffect.SetActive(false);
    }

    public enum CardSuit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }
}
