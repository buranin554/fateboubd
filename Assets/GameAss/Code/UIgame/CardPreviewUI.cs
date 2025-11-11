using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPreviewUI : MonoBehaviour
{
    public static CardPreviewUI Instance;

    [Header("UI References")]
    public GameObject panel;
    public Image cardImage;
    public TMP_Text cardName;
    public TMP_Text cardDescription;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // ป้องกันกรณีมีหลายตัวใน Scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("✅ CardPreviewUI Instance created");

        if (panel != null)
        {
            canvasGroup = panel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = panel.AddComponent<CanvasGroup>();

            panel.SetActive(false); // ซ่อนตอนเริ่มเกม
        }
        else
        {
            Debug.LogError("❌ CardPreviewUI: panel ยังไม่ได้ assign");
        }
    }

    public void Show(Sprite image, string name, string desc)
    {
        if (panel == null)
        {
            Debug.LogError("❌ ไม่มี Panel ให้แสดงผล!");
            return;
        }

        if (cardImage != null) cardImage.sprite = image;
        if (cardName != null) cardName.text = name;
        if (cardDescription != null) cardDescription.text = desc;

        Debug.Log($"🃏 แสดงการ์ด: {name} | {desc}");

        panel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(true));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(false));
    }

    private System.Collections.IEnumerator FadeCanvas(bool show)
    {
        float duration = 0.2f;
        float start = show ? 0 : 1;
        float end = show ? 1 : 0;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, end, t / duration);
            if (canvasGroup != null)
                canvasGroup.alpha = a;
            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = end;

        if (!show)
            panel.SetActive(false);
    }
}
