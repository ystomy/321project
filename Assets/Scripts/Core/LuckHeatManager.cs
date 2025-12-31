using UnityEngine;

// 運（Luck）と熱（Heat）による数値補正を管理するクラス
// ・Luck：平均値を押し上げる「追い風」
// ・Heat：ブレ幅を拡大する「荒れ要素」
public class LuckHeatManager : MonoBehaviour
{
    // 運の良さ（期待値に影響）
    // 高いほど結果が全体的に有利になる
    [Range(0, 100)]
    public int Luck = 50;

    // 熱量（ブレ幅に影響）
    // 高いほど結果が安定せず、極端になりやすい
    [Range(0, 100)]
    public int Heat = 0;

    // --------------------
    // ディーラー設定から初期値を反映
    // --------------------
    public void InitializeFromDealer(DealerData dealer)
    {
        // 対戦相手（ディーラー）ごとの
        // 初期Luck / Heatを読み込む
        Luck = dealer.initialLuck;
        Heat = dealer.initialHeat;
    }

    // --------------------
    // Luck / Heat を加味した数値補正
    // --------------------
    public float ApplyLuckHeat(float baseValue)
    {
        // Luckは「常に上乗せされる期待値補正」
        // 50なら +0.5、100なら +1.0 相当
        float luckBonus = Luck * 0.01f;

        // Heatは「結果の振れ幅」を拡張する
        // Heatが高いほど上下のブレが激しくなる
        float heatRange = 1f + Heat * 0.02f;

        // 基本値 ＋ ランダムなブレ ＋ 運補正
        return baseValue
             + Random.Range(-heatRange, heatRange)
             + luckBonus;
    }
}