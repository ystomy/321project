using UnityEngine;

public class HandTotalView : MonoBehaviour
{
    // 手札の合計値をUIに表示する役割
    // 計算は行わない（表示専用）
    // 表示する値は PlayerStatus.handTotal を参照する

    // カードの表裏や isRevealed の判定は
    // ここでは一切関知しない

    void Start()
    {
        // 初期表示用
        // ゲーム開始時の handTotal をUIに反映する想定
    }

    void Update()
    {
        // 毎フレーム更新はしない予定
        // 値が変わったタイミングで外部から呼ばれる設計にする
    }
}
