using UnityEngine;

public class Card_Pegasus : CardSelectable
{
    public override void UseCard()
    {
        Debug.Log("🪽 ใช้ Pegasus: ลดเทิร์นลง 1");

        // ✅ ตรวจสอบว่ามี owner และ GameManager หรือไม่
        if (owner == null || owner.gameManager == null)
        {
            Debug.LogError("Pegasus: owner หรือ GameManager ยังไม่ได้เซ็ตใน Inspector!");
            Destroy(gameObject);
            return;
        }

        // ✅ ลบการ์ดนี้ออกจากมือ (ถือว่าใช้แล้ว)
        owner.RemoveFromHand(this);

        // ✅ ลดเทิร์นลง 1 (แต่ไม่ให้ติดลบ)
        owner.gameManager.currentTurn -= 3;
        if (owner.gameManager.currentTurn < 0)
            owner.gameManager.currentTurn = 0;

        // ✅ อัปเดต UI
        owner.gameManager.UpdateUI();

        Debug.Log($" เทิร์นถูกลดลงเหลือ: {owner.gameManager.currentTurn} / {owner.gameManager.maxTurns}");
    }
}
