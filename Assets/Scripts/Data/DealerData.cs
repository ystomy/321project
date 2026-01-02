using UnityEngine;

[CreateAssetMenu(menuName = "Game/DealerData")]
public class DealerData : ScriptableObject
{
    // ディーラー名
    // UI表示・ログ・会話演出で使用
    public string dealerName;

    [Header("初期値")]

    // 初期ラック（運の強さ）
    // 0〜100 の相対値
    // カードの巡り・演出補正・心理的圧に影響させる想定
    [Range(0, 100)]
    public int initialLuck = 50;

    // 初期ヒート（感情・ノリ）
    // 開始時は基本0
    // 勝敗やイベントで上下し、挙動や台詞に影響
    [Range(0, 100)]
    public int initialHeat = 0;

    [Header("性格メモ")]

    // ディーラーの性格・癖・演出方針メモ
    // ロジックでは直接使わず、演出・台詞・調整用
    [TextArea]
    public string description;
}
