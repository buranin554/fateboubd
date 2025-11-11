using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI deckCountText;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI scoreText;

    [Header("Card Button")]
    public Button cardButton;

    [Header("Game Variables")]
    public int maxTurns = 10;
    public int currentTurn = 0;
    public int currentScore = 0;
    public int targetScore = 10000;

    [Tooltip("จำนวนไพ่ที่จั่วต่อเทิร์นปกติ")]
    public int drawPerTurn = 2;          // ค่าพื้นฐาน (base)

    [Header("Reference ระบบจั่วจริง")]
    public CardDrawSystem cardDrawSystem;

    // ---------- Buff / Debuff ----------
    // Loki: บัฟจั่วต่อเทิร์น (+2 = จาก 2 เป็น 4)
    private int drawBonus = 0;           // จำนวนที่บวกเพิ่มจาก base
    private int drawBonusTurns = 0;      // ระยะเวลาที่เหลือ (เทิร์น)

    // Athena: ตัวคูณคะแนน
    private int scoreMultiplier = 1;     // x2 เมื่อมีบัฟ
    private int scoreMultTurns = 0;      // ระยะเวลาที่เหลือ (เทิร์น)
    // -----------------------------------

    void Start()
    {
        if (cardDrawSystem == null)
        {
            Debug.LogError(" GameManager: ยังไม่ได้ลาก CardDrawSystem มาใส่ใน Inspector");
            return;
        }
        if (cardButton == null)
        {
            Debug.LogError(" GameManager: ยังไม่ได้ลากปุ่ม (Button) มาใส่ใน Inspector");
            return;
        }
        if (turnText == null || deckCountText == null)
        {
            Debug.LogError(" GameManager: ยังไม่ได้ลาก Turn/Deck Text มาใส่ใน Inspector");
            return;
        }

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnCardClick);

        cardDrawSystem.Initialize();

        currentTurn = 0;
        UpdateUI();
        Debug.Log("Game start: Deck=" + cardDrawSystem.deck.Count + " Turn=" + currentTurn);
    }

    // กดปุ่มจั่ว = จั่วไพ่ (ตามบัฟ) แล้วจบเทิร์น
    void OnCardClick()
    {
        if (currentTurn >= maxTurns) return;

        Debug.Log("Click Draw: ก่อนจั่ว Deck=" + cardDrawSystem.deck.Count + " Turn=" + currentTurn);

        int cardsToDraw = GetCurrentDrawPerTurn(); // รวมบัฟแล้ว
        cardDrawSystem.DrawCards(cardsToDraw);

        NextTurn();    // จบเทิร์น
        OnTurnEnd();   // นับถอยหลังบัฟต่าง ๆ
        UpdateUI();    // อัปเดตตัวเลขทันที

        Debug.Log("หลังจั่ว: Deck=" + cardDrawSystem.deck.Count + " Turn=" + currentTurn);
    }

    // ---------- คะแนน (รองรับ Athena x2) ----------
    public void AddScore(int amount)
    {
        int finalScore = amount * scoreMultiplier;
        currentScore += finalScore;
        if (currentScore > targetScore) currentScore = targetScore;

        Debug.Log($"AddScore: {amount} x{scoreMultiplier} = {finalScore} (total {currentScore})");
        UpdateUI();
    }
    // ------------------------------------------------

    public void UpdateUI()
    {
        if (deckCountText != null && cardDrawSystem != null)
            deckCountText.text = cardDrawSystem.deck.Count.ToString();

        if (turnText != null)
            turnText.text = currentTurn + " / " + maxTurns;

        if (scoreText != null)
            scoreText.text = currentScore.ToString("0000") + " / " + targetScore;
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn > maxTurns) currentTurn = maxTurns;
        Debug.Log("Turn: " + currentTurn);
    }

    // ============== Buff / Debuff API (เรียกจากการ์ด) ==============

    // Athena: เปิดตัวคูณคะแนน
    public void ActivateScoreMultiplier(int multiplier, int turns)
    {
        scoreMultiplier = multiplier;     // เช่น 2
        scoreMultTurns = turns;           // เช่น 2 เทิร์น
        Debug.Log($"[BUFF] Score x{multiplier} for {turns} turn(s).");
    }

    // Loki: เพิ่มจำนวนจั่วต่อเทิร์น (เช่น +2 = จาก 2 เป็น 4) นาน N เทิร์น
    public void AddDrawBuff(int extraDraw, int turns)
    {
        drawBonus = extraDraw;           // +2
        drawBonusTurns = turns;          // 2 เทิร์น
        Debug.Log($"[BUFF] Draw +{extraDraw} for {turns} turn(s).");
    }

    // Pegasus: ปรับเทิร์น (ใส่ -1 เพื่อลด 1 เทิร์น)
    public void ChangeTurn(int delta)
    {
        currentTurn = Mathf.Clamp(currentTurn + delta, 0, maxTurns);
        Debug.Log($"Turn changed: {currentTurn}/{maxTurns}");
        UpdateUI();
    }

    // เรียกทุกครั้งที่จบเทิร์น (หลัง NextTurn)
    public void OnTurnEnd()
    {
        // นับถอยหลังบัฟจั่ว
        if (drawBonusTurns > 0)
        {
            drawBonusTurns--;
            if (drawBonusTurns == 0)
            {
                drawBonus = 0;
                Debug.Log("[BUFF] Draw bonus expired.");
            }
        }

        // นับถอยหลังตัวคูณคะแนน
        if (scoreMultTurns > 0)
        {
            scoreMultTurns--;
            if (scoreMultTurns == 0)
            {
                scoreMultiplier = 1;
                Debug.Log("[BUFF] Score multiplier expired.");
            }
        }
    }

    // จำนวนจั่วต่อเทิร์น (รวมบัฟ)
    private int GetCurrentDrawPerTurn()
    {
        return drawPerTurn + drawBonus; // ปกติ 2, ถ้ามีบัฟ +2 = 4
    }
    public void ReduceTurn(int amount)
    {
        currentTurn -= amount;
        if (currentTurn < 0) currentTurn = 0; // ป้องกันติดลบ
        UpdateUI();
    }

}
