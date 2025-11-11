using UnityEngine; // ✅ ต้องมี

public class Phoenix : CardSelectable
{
    public override void UseCard()
    {
        Debug.Log("🔥 ใช้ Phoenix: เผาไพ่ทั้งหมดแล้วจั่วใหม่ 2 เท่า");

        int cardsInHand = owner.BurnAllHand();
        int drawAmount = Mathf.Min(cardsInHand * 2, owner.maxHandSize);

        owner.DrawCards(drawAmount);
        owner.gameManager.NextTurn();
        owner.gameManager.UpdateUI();
    }
}
