using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Deck deckA = new Deck();
    public Deck deckB = new Deck();

    private Deck currentDeck;

    [Header("リフル設定")]
    public int riffleCount = 3;
    public float riffleSpread = 5f;
    public int cutSpread = 5;

    void Awake()
    {
        InitDeck(deckA);
        InitDeck(deckB);

        Shuffle(deckA);
        Shuffle(deckB);

        currentDeck = deckA;
    }

    // --------------------
    // 初期化（新品トランプ順）
    // --------------------
    void InitDeck(Deck deck)
    {
        deck.cards.Clear();
        deck.used.Clear();

        // ジョーカー2枚
        deck.cards.Add(new Card { suit = 4, number = 0 });
        deck.cards.Add(new Card { suit = 4, number = 1 });

        // スペード A-K
        for (int n = 1; n <= 13; n++)
            deck.cards.Add(new Card { suit = 0, number = n });

        // ハート A-K
        for (int n = 1; n <= 13; n++)
            deck.cards.Add(new Card { suit = 1, number = n });

        // ダイヤ K-A
        for (int n = 13; n >= 1; n--)
            deck.cards.Add(new Card { suit = 2, number = n });

        // クラブ K-A
        for (int n = 13; n >= 1; n--)
            deck.cards.Add(new Card { suit = 3, number = n });
    }

    // --------------------
    // 1枚引く
    // --------------------
    public Card DrawCard()
    {
        if (currentDeck.cards.Count == 0)
            SwitchDeck();

        Card card = currentDeck.cards[0];
        currentDeck.cards.RemoveAt(0);
        currentDeck.used.Add(card.Id);
        return card;
    }

    void SwitchDeck()
    {
        currentDeck = (currentDeck == deckA) ? deckB : deckA;
        if (currentDeck.cards.Count == 0)
            ResetAndShuffle(currentDeck);
    }

    void ResetAndShuffle(Deck deck)
    {
        InitDeck(deck);
        Shuffle(deck);
    }

    // --------------------
    // シャッフル
    // --------------------
    void Shuffle(Deck deck)
    {
        CutMiddle(deck);

        for (int i = 0; i < riffleCount; i++)
            RiffleOnce(deck);

        CutMiddle(deck);
    }

    void CutMiddle(Deck deck)
    {
        int n = deck.cards.Count;
        int cut = n / 2 + Random.Range(-cutSpread, cutSpread + 1);
        cut = Mathf.Clamp(cut, 1, n - 1);

        List<Card> top = deck.cards.GetRange(0, cut);
        List<Card> bottom = deck.cards.GetRange(cut, n - cut);

        deck.cards.Clear();
        deck.cards.AddRange(bottom);
        deck.cards.AddRange(top);
    }

    void RiffleOnce(Deck deck)
    {
        List<Card> cards = deck.cards;
        List<Card> shuffled = new List<Card>();

        int n = cards.Count;
        int cut = Mathf.Clamp(
            Mathf.RoundToInt(RandomGaussian(n / 2f, riffleSpread)),
            5, n - 5
        );

        Queue<Card> left = new Queue<Card>(cards.GetRange(0, cut));
        Queue<Card> right = new Queue<Card>(cards.GetRange(cut, n - cut));

        while (left.Count > 0 || right.Count > 0)
        {
            int takeL = Random.Range(1, Mathf.Min(3, left.Count) + 1);
            int takeR = Random.Range(1, Mathf.Min(3, right.Count) + 1);

            for (int i = 0; i < takeL && left.Count > 0; i++)
                shuffled.Add(left.Dequeue());

            for (int i = 0; i < takeR && right.Count > 0; i++)
                shuffled.Add(right.Dequeue());
        }

        deck.cards = shuffled;
    }

    float RandomGaussian(float mean, float sigma)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
        return mean + sigma * z;
    }

}
