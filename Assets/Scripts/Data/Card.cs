// このクラスは「カードの事実」だけを持つ
// 表裏・公開状態・演出は CardView 側で管理する

[System.Serializable]
public class Card
{
    // スート種別
    // 0:♠ 1:♥ 2:♦ 3:♣ / 4:Joker
    public int suit;

    // カード番号
    // 通常カード：1〜13（A〜K）
    // Joker：0 または 1（識別用）
    public int number;

    // 数値計算用の値
    // 表示用の number とは別
    // 例：ブラックジャックでは A=11 / 絵札=10 など
    public int value
    {
        get
        {
            if (number > 10) return 10;
            return number;
        }
    }

    public int value42
    {
        get
        {
            // ジョーカー
            if (suit == 4) return 11;

            // A
            if (number == 1) return 11;

            // 通常カード
            return value;
        }
    }


    // 一意識別子
    // デバッグ・ログ・Dictionaryキー用
    // 「suit-number」の組み合わせでカードを特定する
    public string Id => $"{suit}-{number}";
}
