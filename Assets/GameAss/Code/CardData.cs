using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Fatebound/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;    // ชื่อไพ่
    public Sprite cardSprite;  // รูปไพ่
    // เพิ่มข้อมูลอื่นๆได้ในอนาคต เช่น แต้ม, เอฟเฟกต์
}
