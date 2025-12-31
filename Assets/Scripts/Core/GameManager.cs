using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerStatus player;
    public DeckManager deck;
    public UIManager ui;

    public bool canExtraDraw;

    public void CheckExtraDrawCondition()
    {
        canExtraDraw = player.handTotal >= 43;
        ui.SetExtraDrawButton(canExtraDraw);
    }

    public void ExtraDraw()
    {
        if (!canExtraDraw) return;

        Card card = deck.DrawCard();
        AddCardToHand(card);

        player.luck -= 5;
        CheckExtraDrawCondition();
    }

    void AddCardToHand(Card card)
    {
        player.handTotal += card.value;
        // CardView¶¬‚Æ‚©‚ÍŠù‘¶ˆ—‚ÅOK
    }

    int GetHandTotal(List<Card> hand)
    {
        int total = 0;
        foreach (var card in hand)
        {
            total += card.value; // © ‚±‚±‚Í Card ‚Ì’è‹`‚É‡‚í‚¹‚Ä
        }
        return total;
    }
}
