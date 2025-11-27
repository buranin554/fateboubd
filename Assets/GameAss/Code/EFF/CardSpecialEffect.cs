using UnityEngine;
using System.Collections;

public class CardSpecialEffect : MonoBehaviour
{
    public enum SpecialType
    {
        Morvhal,
        VoidArbiter,
        BoundFiend,
        Soulflame
    }

    [Header("Card Setting")]
    public SpecialType type;
    public Sprite cardImage;
    [TextArea] public string description;

    private CardEffectManager effectManager;
    private GameManager gameManager;
    private CardDrawSystem drawSystem;
    private CardSelectable selfCard;

    void Start()
    {
        effectManager = FindObjectOfType<CardEffectManager>();
        gameManager = FindObjectOfType<GameManager>();
        drawSystem = FindObjectOfType<CardDrawSystem>();
        selfCard = GetComponent<CardSelectable>();
    }

    public void ActivateEffect()
    {
        StartCoroutine(ExecuteEffect());
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(0.1f);

        // ลบออกจากมือก่อน (กันช่องว่าง)
        if (selfCard != null)
            drawSystem.RemoveFromHand(selfCard);

        switch (type)
        {
            case SpecialType.Morvhal:
                drawSystem.BurnAllHand();
                yield return new WaitForSeconds(0.1f);
                drawSystem.DrawCards(7);

                effectManager.ShowEffect(cardImage,
                    "MORVHAL. LORD OF MISFORTUNE</b></size>\n\n" +
                    "Immediately replace all cards in your hand.\nCOMBOS cannot be used on your next turn.");
                break;

            case SpecialType.VoidArbiter:
                gameManager.SkipNextTurn = true;

                effectManager.ShowEffect(cardImage,
                    "VOID ARBITER</b></size>\n\n" +
                    "You cannot draw any cards on your next turn.");
                break;

            case SpecialType.BoundFiend:
                gameManager.limitedHandTurns = 2;
                gameManager.handLimit = 5;

                effectManager.ShowEffect(cardImage,
                    "THE BOUND FIEND</b></size>\n\n" +
                    "Your hand size is limited to 5 cards for the next 2 turns.");
                break;

            case SpecialType.Soulflame:
                drawSystem.BurnAllHand();
                gameManager.ReduceTurn(1);
                yield return new WaitForSeconds(0.1f);
                drawSystem.DrawCards(7);

                effectManager.ShowEffect(cardImage,
                    "THE SOULFLAME REVENANT</b></size>\n\n" +
                    "Discard all cards in your hand and skip your next turn.\nNext turn: draw a new full hand.");
                break;
        }

        Destroy(gameObject);
    }

    

}
