using UnityEngine;

// カードとスプライトの対応表を管理するデータベース
// ScriptableObjectとして作成し、
// 絵柄差し替え・テーマ変更をコード修正なしで行えるようにする
[CreateAssetMenu(menuName = "Card/CardSpriteDB")]
public class CardSpriteDB : ScriptableObject
{
    // 各スートごとのカードスプライト配列
    // index = card.number - 1（A=0, K=12）
    public Sprite[] spade;
    public Sprite[] heart;
    public Sprite[] diamond;
    public Sprite[] club;

    // ジョーカー用スプライト
    // number をインデックスとして使用（0,1想定）
    public Sprite[] joker;

    // --------------------
    // Cardデータから対応するスプライトを取得
    // --------------------
    // Card.suit
    // 0 = Spade
    // 1 = Heart
    // 2 = Diamond
    // 3 = Club
    // 4 = Joker
    public Sprite GetSprite(Card card)
    {
        // ---- ジョーカー処理 ----
        if (card.suit == 4)
        {
            // number が範囲外でも落ちないように保険
            int index = Mathf.Clamp(card.number, 0, joker.Length - 1);
            return joker[index];
        }

        // 通常カードは number - 1 を配列インデックスとして使用
        int i = card.number - 1;

        // ---- スートごとの分岐 ----
        switch (card.suit)
        {
            case 0: // スペード
                return spade[i];
            case 1: // ハート
                return heart[i];
            case 2: // ダイヤ
                return diamond[i];
            case 3: // クラブ
                return club[i];
            default:
                // 不正な suit が来た場合
                return null;
        }
    }

}
