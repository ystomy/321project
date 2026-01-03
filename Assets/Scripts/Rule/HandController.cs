using System;
using System.Collections.Generic;
using UnityEngine;

// 手札全体を管理するコントローラ
// ・CardView からの「初めて表になった」通知を受け取る
// ・42ルールに基づいて手札合計値を再計算
// ・結果を UI（HandTotalView）に反映する
public class HandController : MonoBehaviour
{
    // 合計値表示用UI（Total : の下の数値）
    // Inspector で紐付ける
    [SerializeField]
    HandTotalView handTotalView;

    [SerializeField]
    GameObject cardPrefab;


    // この手札に属するカード一覧
    // PlayerHand / DealerHand ごとに別インスタンスを持つ想定
    private readonly List<CardView> cards = new List<CardView>();

    HandCalculator calculator = new HandCalculator();

    // 手札合計を計算する純ロジッククラス
    // UIや演出に依存しない

    public event Action<int> OnHandTotalChanged;


    public void Register(CardView card)
    {
        cards.Add(card);
        card.OnRevealed += OnCardRevealed;

        Debug.Log($"[HandController] Register {card.card} / total={cards.Count}");
    }

    // カードが初めて表になったときに呼ばれる
    private void OnCardRevealed(CardView revealed)
    {
        int sum = calculator.Calculate(cards);
        handTotalView.SetValue(sum);
        OnHandTotalChanged?.Invoke(sum);
    }

    public void AddCard(Card card)
    {
        // ここで表示用カード生成
        GameObject go = Instantiate(cardPrefab, transform);
        CardView view = go.GetComponent<CardView>();

        view.Init(card);          // ← ここで Register & 計算が走る
        view.SetFace(true);       // 表にするなら

        // レイアウト更新などがあればここ
    }

}
