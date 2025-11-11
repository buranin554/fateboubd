using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public enum ButtonAction
    {
        LoadScene,
        QuitGame
    }

    [Header("เลือก Action ของปุ่มนี้")]
    public ButtonAction action;

    [Header("ใส่ชื่อ Scene ที่ต้องการไป (ใช้เฉพาะ Action = LoadScene)")]
    public string sceneName;

    private void OnMouseDown()
    {
        switch (action)
        {
            case ButtonAction.LoadScene:
                if (!string.IsNullOrEmpty(sceneName))
                {
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    Debug.LogWarning($"ปุ่ม {gameObject.name} ยังไม่ได้ใส่ชื่อ Scene ใน Inspector");
                }
                break;

            case ButtonAction.QuitGame:
                Debug.Log("Quit Game");
                Application.Quit();
                break;
        }
    }
}
 