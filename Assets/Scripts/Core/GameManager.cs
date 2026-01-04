using UnityEngine;

// ゲーム全体の進行・判定を管理するクラス
// ・プレイヤー状態の参照
// ・山札からの追加ドロー制御
// ・UIとの橋渡し役
public class GameManager : MonoBehaviour
{
    // プレイヤーの現在状態（手札合計・運など）
    public PlayerStatus player;

    // 山札管理
    public DeckManager deck;

    // UI操作用
    public UIManager ui;

    // 追加ドローが可能かどうか
    public bool canExtraDraw;

    //ハンコンから出た計算を受け取る
    public HandController playerHand;


    // --------------------
    // 追加ドロー条件チェック
    // --------------------
    void CheckExtraDraw()
    {
        bool can =
            player.handCount == 6 &&

            // 手札合計が一定値以上なら追加ドロー解禁
            player.handTotal <= 42 &&

            player.luck >= 5;

        playerHand.AreAllCardsRevealed();
        // UI側のボタン表示／有効状態を更新
        ui.SetExtraDrawButton(can);
    }

    // --------------------
    // 追加ドロー処理
    // --------------------
    public void ExtraDraw()
    {
        // 条件未達なら何もしない
        if (player.luck < 5) return;

        // 追加ドローの代償として運を消費
        player.luck -= 5;

        // 山札から1枚引く
        Card card = deck.DrawCard();

        // プレイヤーの手札に反映
        playerHand.AddCard(card);

        ui.SetExtraDrawButton(false); // 一旦閉じる
    }


    
    // --------------------
    // 手札へカードを加算
    // --------------------
    void AddCardToHand(Card card)
    {
        playerHand.AddCard(card); // HandController側で生成＆計算

    }

    void Start()
    {
        playerHand.OnHandTotalChanged += OnPlayerHandTotalChanged;
        playerHand.OnAllCardsRevealed += CheckExtraDraw;
    }

    void OnPlayerHandTotalChanged(int total)
    {
        player.handTotal = total;
        CheckExtraDraw();
    }


}
