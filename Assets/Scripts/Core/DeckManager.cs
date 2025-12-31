using System.Collections.Generic;
using UnityEngine;

// 山札全体を管理するクラス
// ・2つのデッキを持ち回りで使用
// ・リアル寄りのシャッフル（カット＋リフル）を再現
// ・カードを1枚ずつ供給する
public class DeckManager : MonoBehaviour
{
    // 使用する2つのデッキ（使い切り対策）
    public Deck deckA = new Deck();
    public Deck deckB = new Deck();

    // 現在使用中のデッキ
    private Deck currentDeck;

    [Header("リフル設定")]
    // リフルシャッフルを何回行うか
    public int riffleCount = 3;
    // リフル時のばらつき（中央からのズレ）
    public float riffleSpread = 5f;
    // カット位置のランダム幅
    public int cutSpread = 5;

    void Awake()
    {
        // 両方のデッキを新品状態に初期化
        InitDeck(deckA);
        InitDeck(deckB);

        // それぞれシャッフル
        Shuffle(deckA);
        Shuffle(deckB);

        // 最初はデッキAから使用
        currentDeck = deckA;
    }

    // --------------------
    // 初期化（新品トランプ順）
    // --------------------
    void InitDeck(Deck deck)
    {
        // 山札・使用済み履歴をクリア
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

        // ダイヤ K-A（新品トランプの並びに準じて意図的に逆順）
        for (int n = 13; n >= 1; n--)
            deck.cards.Add(new Card { suit = 2, number = n });

        // クラブ K-A（新品トランプの並びに準じて意図的に逆順）
        for (int n = 13; n >= 1; n--)
            deck.cards.Add(new Card { suit = 3, number = n });
    }

    // --------------------
    // 1枚引く
    // --------------------
    public Card DrawCard()
    {
        // 現在のデッキが空なら切り替え
        if (currentDeck.cards.Count == 0)
            SwitchDeck();

        // 先頭のカードを引く
        Card card = currentDeck.cards[0];
        currentDeck.cards.RemoveAt(0);

        // 使用済みカードとして記録（イカサマ・解析用）
        currentDeck.used.Add(card.Id);

        return card;
    }

    // デッキ切り替え処理
    void SwitchDeck()
    {
        // A ↔ B を切り替える
        currentDeck = (currentDeck == deckA) ? deckB : deckA;

        // 切り替え先も空なら再初期化＆シャッフル
        if (currentDeck.cards.Count == 0)
            ResetAndShuffle(currentDeck);
    }

    // デッキを新品状態に戻してシャッフル
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
        // 中央カット
        CutMiddle(deck);

        // リフルシャッフルを複数回
        for (int i = 0; i < riffleCount; i++)
            RiffleOnce(deck);

        // 仕上げにもう一度カット
        CutMiddle(deck);
    }

    // 山札を中央付近でカットする
    void CutMiddle(Deck deck)
    {
        int n = deck.cards.Count;

        // 中央 ± ランダム幅でカット位置を決定
        int cut = n / 2 + Random.Range(-cutSpread, cutSpread + 1);
        cut = Mathf.Clamp(cut, 1, n - 1);

        // 上下に分割
        List<Card> top = deck.cards.GetRange(0, cut);
        List<Card> bottom = deck.cards.GetRange(cut, n - cut);

        // 下を先にして合体（カット再現）
        deck.cards.Clear();
        deck.cards.AddRange(bottom);
        deck.cards.AddRange(top);
    }


    // リフルシャッフルを1回行う
    void RiffleOnce(Deck deck)
    {
        List<Card> cards = deck.cards;
        List<Card> shuffled = new List<Card>();

        int n = cards.Count;

        // 正規分布を使って「人間っぽい」カット位置を決定
        int cut = Mathf.Clamp(
            Mathf.RoundToInt(RandomGaussian(n / 2f, riffleSpread)),
            5, n - 5
        );

        // 左右に分割
        Queue<Card> left = new Queue<Card>(cards.GetRange(0, cut));
        Queue<Card> right = new Queue<Card>(cards.GetRange(cut, n - cut));

        // 左右から1〜2枚ずつ交互に落とす
        while (left.Count > 0 || right.Count > 0)
        {
            int takeL = Random.Range(1, Mathf.Min(3, left.Count) + 1);
            int takeR = Random.Range(1, Mathf.Min(3, right.Count) + 1);

            for (int i = 0; i < takeL && left.Count > 0; i++)
                shuffled.Add(left.Dequeue());

            for (int i = 0; i < takeR && right.Count > 0; i++)
                shuffled.Add(right.Dequeue());
        }

        // シャッフル結果を反映
        deck.cards = shuffled;
    }

    // 正規分布（ガウス分布）乱数生成
    // mean = 平均値, sigma = ばらつき
    float RandomGaussian(float mean, float sigma)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
        return mean + sigma * z;
    }

}
