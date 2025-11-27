using UnityEngine;

public class ComboCardToggle : MonoBehaviour
{
    public CanvasGroup comboCardCanvas; // ลาก ComboCardPanel ที่มี CanvasGroup มาใส่ตรงนี้
    public float fadeDuration = 0.25f;
    private bool isVisible = false;

    private void Start()
    {
        // เริ่มต้นซ่อน
        comboCardCanvas.alpha = 0;
        comboCardCanvas.interactable = false;
        comboCardCanvas.blocksRaycasts = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // คีย์ลัด
            TogglePanel();
    }

    public void TogglePanel()
    {
        StopAllCoroutines();
        StartCoroutine(FadePanel(!isVisible));
    }

    private System.Collections.IEnumerator FadePanel(bool show)
    {
        isVisible = show;
        float start = comboCardCanvas.alpha;
        float end = show ? 1 : 0;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            comboCardCanvas.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }

        comboCardCanvas.alpha = end;
        comboCardCanvas.interactable = show;
        comboCardCanvas.blocksRaycasts = show;
    }
}
