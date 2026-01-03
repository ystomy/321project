using System.Collections.Generic;

/// <summary>
/// 手札の合計値を計算するクラス
/// 42ルールに基づき、
/// 「一度でも表になったカード（isRevealed）」のみを対象に合計する
/// </summary>
public class HandCalculator
{
    /// <summary>
    /// 手札合計値を計算する
    /// </summary>
    /// <param name="cards">
    /// 手札のカード一覧（CardView）
    /// 表示状態ではなく isRevealed のみを判定に使用する
    /// </param>
    /// <returns>
    /// isRevealed == true のカード value の合計値
    /// </returns>
    public int Calculate(List<CardView> cards)
    {
        // 合計値は毎回ゼロから再計算する
        // 「前回いくつだったか」は一切覚えない
        int total = 0;

        foreach (var card in cards)
        {
            // 一度でも表になったカードのみを加算対象とする
            // 再び裏返っていても減算しない
            if (card.isRevealed)
            {
                total += card.card.value42;
            }
        }

        return total;
    }
}
