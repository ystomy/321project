using System.Collections.Generic;
using UnityEngine;

// カードの配布役（ディーラー）を担当するクラス
// ・山札からカードを引く
// ・プレイヤー／ディーラーの手札を管理
// ・カードの生成と配置を行う
public class Dealer : MonoBehaviour
{
    // 山札管理クラス（カードを引く）
    [SerializeField]
    DeckManager deckManager;

    // カードID → スプライト変換用DB
    [SerializeField]
    CardSpriteDB spriteDB;

    // 表示用カードPrefab（CardViewを持つ）
    [SerializeField]
    GameObject cardPrefab;

    [Header("配置")]
    // プレイヤー側カードの親Transform
    public Transform playerRoot;

    // ディーラー側カードの親Transform
    public Transform dealerRoot;

    // 初期配布枚数
    public int initialDealCount = 6;

    // プレイヤーの手札データ
    public List<Card> playerHand = new List<Card>();

    // ディーラーの手札データ
    public List<Card> dealerHand = new List<Card>();

    // カード同士の横方向の間隔
    [SerializeField]
    private float cardSpacing = 100f;

    // ディーラー固有データ（性格・運勢など）
    public DealerData dealerData;

    // 運・ヒート管理（ディーラーに紐づく）
    [SerializeField]
    private LuckHeatManager luckHeat;

    void Start()
    {
        // ディーラーデータを元に運勢システムを初期化
        luckHeat.InitializeFromDealer(dealerData);

        // ゲーム開始時に初期配布を行う
        DealInitialCards();
    }

    // 初期状態のカード配布
    public void DealInitialCards()
    {
        // 手札データをリセット
        playerHand.Clear();
        dealerHand.Clear();

        for (int i = 0; i < initialDealCount; i++)
        {
            // プレイヤーはすべて伏せて配る
            DealOne(playerHand, playerRoot, false);

            // ディーラーは交互に表・裏を切り替える
            // （ブラックジャック的な見せ方）
            bool dealerFaceUp = (i % 2 == 0);
            DealOne(dealerHand, dealerRoot, dealerFaceUp);
        }
    }

    // 指定した手札にカードを1枚配る共通処理
    void DealOne(List<Card> hand, Transform root, bool faceUp)
    {
        // 山札からカードデータを1枚引く
        Card card = deckManager.DrawCard();
        hand.Add(card);

        // 表示用カードオブジェクトを生成
        GameObject go = Instantiate(cardPrefab, root);
        CardView view = go.GetComponent<CardView>();

        // カードの見た目を設定
        view.SetCard(spriteDB.GetSprite(card));
        view.SetFace(faceUp);

        // 描画順の基準値（他UIより前に出す想定）
        int baseOrder = 100;

        // 手札内でのインデックス
        int index = hand.Count - 1;

        // 中央揃えになるようにオフセットを計算
        float offset = (hand.Count - 1) * cardSpacing * 0.5f;

        // 横一列に並べる
        go.transform.localPosition = new Vector3(
            index * cardSpacing - offset,
            0f,
            0f
        );

        // 手前に来るカードほど描画順を大きくする
        view.SetSortOrder(baseOrder + index);

    }
}
