using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
    public List<Card> cards = new List<Card>();
    public List<string> used = new List<string>();

    public Card DrawRandom()
    {
        if (cards.Count == 0) return null;

        int index = Random.Range(0, cards.Count);
        Card card = cards[index];
        cards.RemoveAt(index);
        return card;
    }
}
