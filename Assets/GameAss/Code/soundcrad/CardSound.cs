using UnityEngine;

public class CardSound : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip popupSound;   // เสียงตอนเด้ง Popup
    public AudioClip useSound;     // เสียงตอนกดใช้

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // เรียกใช้เวลาต้องการให้เล่นเสียง Popup
    public void PlayPopupSound()
    {
        if (popupSound != null)
            audioSource.PlayOneShot(popupSound);
    }

    // เรียกใช้เวลาต้องการให้เล่นเสียงตอนกดใช้
    public void PlayUseSound()
    {
        if (useSound != null)
            audioSource.PlayOneShot(useSound);
    }
}
