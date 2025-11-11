using UnityEngine;

public abstract class SpecialCard : CardSelectable
{
    // ฟังก์ชันพิเศษที่การ์ดลูกต้อง override
    public abstract void ActivateEffect();

    // เมื่อกดใช้ (หรือกด E แล้วเลือกการ์ดพิเศษ)
    public override void UseCard()
    {
        base.UseCard(); // เรียก behavior ปกติ เช่น ลบออกจากมือ
        ActivateEffect(); // เรียก effect เฉพาะของการ์ดนั้น
    }
}
