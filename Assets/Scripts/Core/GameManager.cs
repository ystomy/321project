using System.Collections.Generic;
using UnityEngine;

// ゲーム全体の進行・判定を管理するクラス
// ・プレイヤー状態の参照
// ・山札からの追加ドロー制御
// ・UIとの橋渡し役
public class GameManager : MonoBehaviour
{
    // プレイヤーの現在状態（手札合計・運など）
    public PlayerStatus player;

    // 山札管理
    public DeckManager deck;

    // UI操作用
    public UIManager ui;

    // 追加ドローが可能かどうか
    public bool canExtraDraw;

    // --------------------
    // 追加ドロー条件チェック
    // --------------------
    public void CheckExtraDrawCondition()
    {
        // 手札合計が一定値以上なら追加ドロー解禁
        canExtraDraw = player.handTotal >= 43;

        // UI側のボタン表示／有効状態を更新
        ui.SetExtraDrawButton(canExtraDraw);
    }

    // --------------------
    // 追加ドロー処理
    // --------------------
    public void ExtraDraw()
    {
        // 条件未達なら何もしない
        if (!canExtraDraw) return;

        // 山札から1枚引く
        Card card = deck.DrawCard();

        // プレイヤーの手札に反映
        AddCardToHand(card);

        // 追加ドローの代償として運を消費
        player.luck -= 5;

        // 状態が変わったので再判定
        CheckExtraDrawCondition();
    }

    // --------------------
    // 手札へカードを加算
    // --------------------
    void AddCardToHand(Card card)
    {
        // 手札合計値を更新
        // ※ここではカードリスト管理ではなく数値管理のみ
        player.handTotal += card.value;
        
    }

    // --------------------
    // 手札合計計算（予備・汎用）
    // --------------------
    int GetHandTotal(List<Card> hand)
    {
        int total = 0;

        // 手札内のカード価値を合算
        foreach (var card in hand)
        {
            total += card.value; 
        }
        return total;
    }
}
