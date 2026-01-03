using UnityEngine;
using TMPro;

public class HandTotalView : MonoBehaviour
{
    // 手札の合計値表示用テキスト
    // 変動する数値表示用
    [SerializeField]
    private TMP_Text valueText;

    // 表示専用クラス
    // 計算・カード管理・ルール判断は一切行わない

    /// <summary>
    /// 手札合計値をUIに反映する
    /// PlayerStatus.handTotal の再計算後に呼ばれる想定
    /// </summary>
    public void UpdateTotal(int handTotal)
    {
        valueText.text = handTotal.ToString();
    }

    public void SetValue(int total)
    {
        valueText.text = total.ToString();
    }

    /// <summary>
    /// 初期化用（任意）
    /// ゲーム開始時に一度だけ呼ぶ
    /// </summary>
    public void Init(PlayerStatus status)
    {
        UpdateTotal(status.handTotal);
    }
}
