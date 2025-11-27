using UnityEngine;

public class PopupCardController : MonoBehaviour
{
    public CardSound cardSound;      // ลาก Card มาใส่
    public GameObject popupPanel;

    public void ShowPopup(CardSound card)
    {
        cardSound = card;
        popupPanel.SetActive(true);

        // เล่นเสียง popup
        cardSound.PlayPopupSound();
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}
