using UnityEngine;

public class Card_Valkyrie : CardSelectable
{
    public ParticleSystem GlowEffect; // 🌟 เพิ่มตัวนี้

    public override void UseCard()
    {
        Debug.Log("⚔️ ใช้ Valkyrie: เพิ่มพลัง +1000 แต้ม");

        if (owner == null || owner.gameManager == null)
        {
            Debug.LogError("❌ Valkyrie ไม่มี owner หรือ GameManager");
            return;
        }

        owner.gameManager.currentScore += 1000;
        owner.gameManager.UpdateUI();

        if (GlowEffect != null)
            GlowEffect.Play();

        Debug.Log("✨ Valkyrie ปลดปล่อยพลังศักดิ์สิทธิ์ (+1000 คะแนน)");
    }
}
