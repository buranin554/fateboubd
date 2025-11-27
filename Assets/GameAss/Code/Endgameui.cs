using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public Text resultText;

    public void ShowResult(bool win)
    {
        if (win)
            resultText.text = "YOU WIN!";
        else
            resultText.text = "GAME OVER!";

        gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
    }
}
