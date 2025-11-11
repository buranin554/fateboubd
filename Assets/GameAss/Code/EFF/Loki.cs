using UnityEngine;
using System.Collections.Generic;

public class Card_Loki : SpecialCard
{
    public override void ActivateEffect()
    {
        Debug.Log("🃏 Loki Effect: สลับตำแหน่งไพ่ในมือแบบสุ่ม!");

        if (CardSelectManager.Instance == null ||
            CardSelectManager.Instance.cardDrawSystem == null)
            return;

        var hand = CardSelectManager.Instance.cardDrawSystem.handCards;
        if (hand == null || hand.Count <= 1) return;

        // สุ่มสลับตำแหน่งไพ่
        for (int i = 0; i < hand.Count; i++)
        {
            int rand = Random.Range(0, hand.Count);
            var temp = hand[i];
            hand[i] = hand[rand];
            hand[rand] = temp;
        }

        CardSelectManager.Instance.cardDrawSystem.ReorderHand();
        Debug.Log("🌀 Loki ทำให้ไพ่ในมือสับใหม่เรียบร้อย!");
    }
}
