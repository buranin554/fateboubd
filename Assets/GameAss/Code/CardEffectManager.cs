using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CardEffectManager : MonoBehaviour
{
    [Header("Effect UI")]
    public CanvasGroup effectPanel;        // พาเนลแสดงข้อมูลการ์ด
    public Image effectImage;              // ภาพการ์ด
    public TMP_Text effectDescription;     // คำอธิบาย
    public Button closeButton;             // ปุ่ม X ปิด
    public float fadeDuration = 0.25f;

    private bool showingEffect = false;

    private void Start()
    {
        HideEffectInstant();
        closeButton.onClick.AddListener(HideEffect);
    }

    // ฟังก์ชันเรียกตอนจั่วการ์ดพิเศษ
    public void ShowEffect(Sprite cardSprite, string description)
    {
        effectImage.sprite = cardSprite;
        effectDescription.text = description;
        StopAllCoroutines();
        StartCoroutine(FadeInPanel());
    }

    IEnumerator FadeInPanel()
    {
        showingEffect = true;
        effectPanel.interactable = true;
        effectPanel.blocksRaycasts = true;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            effectPanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        effectPanel.alpha = 1;
    }

    public void HideEffect()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutPanel());
    }

    IEnumerator FadeOutPanel()
    {
        float t = 0;
        float start = effectPanel.alpha;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            effectPanel.alpha = Mathf.Lerp(start, 0, t / fadeDuration);
            yield return null;
        }
        HideEffectInstant();
        showingEffect = false;
    }

    void HideEffectInstant()
    {
        effectPanel.alpha = 0;
        effectPanel.interactable = false;
        effectPanel.blocksRaycasts = false;   // ❗ สำคัญมาก ต้องปิด block
    }


    private void Update()
    {
        if (effectPanel.alpha > 0 && Input.GetKeyDown(KeyCode.X))
        {
            HideEffect();
        }
    }


}
