[System.Serializable]
public class PlayerStatus
{
    // 現在の手札合計値
    // isRevealed == true のカードのみを対象に
    // 毎回再計算して代入される想定
    // 「足し引きの履歴」は持たない
    public int handTotal;

    // 現在表示したカードの数
    public int handCount;

    // プレイヤーの運（補正値）
    // 基本値は100を基準とした相対値
    // カード配分・イベント・スキル効果の
    // 補正に使う想定
    public int luck = 100;

    // 将来的に追加候補：
    // ・betPoint（賭けポイント）
    // ・heat（プレイヤー側のノリ）
    // ・skillCoolTime
}
