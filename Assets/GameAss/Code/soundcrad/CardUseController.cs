using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;

public class CardUseController : MonoBehaviour, IPointerClickHandler
{
    private CardSound cardSound;
    private object effectScript;
    private MethodInfo doEffectMethod;

    void Awake()
    {
        cardSound = GetComponent<CardSound>();

        // ค้นหา Script ที่มีฟังก์ชันชื่อ DoEffect
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (var s in scripts)
        {
            var m = s.GetType().GetMethod("DoEffect");
            if (m != null)
            {
                effectScript = s;
                doEffectMethod = m;
                break;
            }
        }

        if (doEffectMethod == null)
            Debug.LogWarning($"{name} ไม่มีฟังก์ชัน DoEffect() ในสคริปต์ใด ๆ");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // เล่นเสียงตอนใช้การ์ด
        cardSound.PlayUseSound();

        // เรียก DoEffect() ถ้ามี
        if (doEffectMethod != null)
        {
            doEffectMethod.Invoke(effectScript, null);
        }
    }
}
