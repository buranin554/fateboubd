using UnityEngine;

public class Card_Athena : CardSelectable
{
    public ParticleSystem GlowEffect; // เอฟเฟกต์แสง

    public override void UseCard()
    {
        Debug.Log("🦉 ใช้ Athena: คะแนนคูณ 2!");

        // ตรวจสอบ owner และ GameManager
        if (owner == null || owner.gameManager == null)
        {
            Debug.LogError("❌ Athena ไม่มี owner หรือ GameManager");
            return;
        }

        // ลบการ์ดออกจากมือ (ถ้าใช้แล้ว)
        owner.RemoveFromHand(this);

        // ✨ คูณคะแนน x2
        owner.gameManager.currentScore *= 2;

        // ✅ จำกัดคะแนนไม่ให้ติดลบ (กันไว้)
        if (owner.gameManager.currentScore < 0)
            owner.gameManager.currentScore = 0;

        // อัปเดต UI
        owner.gameManager.UpdateUI();

        // เอฟเฟกต์ Glow
        if (GlowEffect != null)
            GlowEffect.Play();

        Debug.Log($"Athena ใช้พลังแห่งสติปัญญา! (คะแนนตอนนี้: {owner.gameManager.currentScore})");
    }
}
