using System.Collections.Generic;
using UnityEngine;

// トランプ1組分の状態を保持するクラス
// ・cards : 現在山に残っているカード
// ・used  : すでに使用されたカードIDの履歴（統計・演出・不正防止用）
[System.Serializable]
public class Deck
{
    // 山札に残っているカード一覧
    // Draw系処理でここから減っていく
    public List<Card> cards = new List<Card>();

    // 使用済みカードのID履歴
    // 「何が出たか」を後から参照するためのログ用途
    // シャッフルやリセット時にクリアされる想定
    public List<string> used = new List<string>();

    // --------------------
    // ランダムに1枚引く（完全ランダム）
    // --------------------
    // ・山札の順序を考慮しない
    // ・演出用、特殊効果用、テスト用を想定
    public Card DrawRandom()
    {
        // 山札が空なら引けない
        if (cards.Count == 0) return null;

        // 残っているカードからランダムに1枚選ぶ
        int index = Random.Range(0, cards.Count);
        Card card = cards[index];

        // 山札から除外
        cards.RemoveAt(index);

        return card;
    }
}
