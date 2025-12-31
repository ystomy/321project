using UnityEngine;

// UI全体の表示制御を担当するマネージャ
// ゲームロジック（GameManager等）から状態だけを受け取り、
// 「何を表示するか」だけを判断する役割
public class UIManager : MonoBehaviour
{
    // 追加ドロー（42ルール）用のボタン
    // handTotal が条件を満たした時のみ表示される
    public GameObject extraDrawButton;

    // --------------------
    // 追加ドローボタンの表示切り替え
    // --------------------
    // isActive == true  : 42ルールが成立し、プレイヤーが選択可能
    // isActive == false : 条件未達、または使用不可状態
    public void SetExtraDrawButton(bool isActive)
    {
        // UI側では条件判定は行わない
        // 「表示していいかどうか」だけを受け取って反映する
        extraDrawButton.SetActive(isActive);
    }
}
