using System;
using System.Collections.Generic;
using UnityEngine;

// ================================
// HandController
// ----------------
// 1つの手札（プレイヤー / ディーラー）を管理する中枢クラス
//
// 役割：
// ・配下の CardView を登録・管理する
// ・CardView から「初めて表になった」通知を受け取る
// ・42ルールに基づいて手札合計値を計算する
// ・計算結果を UI（HandTotalView）や他システムへ通知する
//
// ※ カードの生成・配置は Dealer 側、
//    数値計算と通知は HandController 側、という責務分離
// ================================
public class HandController : MonoBehaviour
{
    // ----------------------------
    // Inspector 設定
    // ----------------------------

    // 手札合計表示用UI
    // 「Total :」の下にある数値表示
    [SerializeField]
    HandTotalView handTotalView;

    // カード表示用プレハブ（CardView を持つ）
    // AddCard() で使用
    [SerializeField]
    GameObject cardPrefab;

    // ----------------------------
    // 内部状態
    // ----------------------------

    // この手札に属するカード一覧
    // PlayerHand / DealerHand ごとに HandController を分けて持つ想定
    private readonly List<CardView> cards = new List<CardView>();

    // 42ルール専用の純ロジック計算クラス
    // MonoBehaviour 非依存・テスト可能
    HandCalculator calculator = new HandCalculator();

    // 手札合計が変化したときに通知するイベント
    // UI以外（演出・SE・ゲーム進行）からも購読可能
    public event Action<int> OnHandTotalChanged;

    // 手札が全てオープンされたかチェック
    public event Action OnAllCardsRevealed;
    
    // 手札がオープンされた数のチェック
    public event Action<int> OnHandCountChanged;

    // ----------------------------
    // CardView 登録
    // ----------------------------

    // CardView 側から呼ばれる登録処理
    // Init() 時に HandController を探して自動登録される想定
    public void Register(CardView card)
    {
        cards.Add(card);

        // 「初めて表になった瞬間」を監視
        card.OnRevealed += OnCardRevealed;
        
        // すでに表になった枚数を監視
        OnHandCountChanged?.Invoke(cards.Count);

    }

    // ----------------------------
    // カード公開時の処理
    // ----------------------------

    // カードが初めて表になった瞬間に呼ばれる
    private void OnCardRevealed(CardView revealed)
    {
        // 現在の手札から合計値を再計算
        int sum = calculator.Calculate(cards);

        // UIへ反映
        handTotalView.SetValue(sum);

        // 外部システムへ通知
        OnHandTotalChanged?.Invoke(sum);

        // 全カードが表か？
        if (cards.Count == 6 && cards.TrueForAll(c => c.isFaceUp))
        {
            OnAllCardsRevealed?.Invoke();
        }

        if (AreAllCardsRevealed())
        {
            Debug.Log("42ルール：全カードオープン！");
        }
    }

    // ----------------------------
    // カード追加（予備・拡張用）
    // ----------------------------

    // 追加ドローなどでカードを直接手札に加える場合に使用
    public void AddCard(Card card)
    {
        // 表示用カードを生成
        GameObject go = Instantiate(cardPrefab, transform);
        CardView view = go.GetComponent<CardView>();

        // CardView 初期化
        // この中で Register() が呼ばれる
        view.Init(card);

        // 追加ドロー時は即表にする想定
        view.SetFace(true);

        // レイアウト更新や演出があればここに追加
    }

    // ----------------------------
    // 全てのカードが表示済みか確認
    // ----------------------------
    public bool AreAllCardsRevealed()
    {
        if (cards.Count < 6) return false;

        foreach (var card in cards)
        {
            if (!card.isRevealed) return false;
        }
        return true;
    }


}
