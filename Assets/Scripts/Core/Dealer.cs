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

    [SerializeField]
    float tableSpacing = 220f;   // 3卓の間隔
    [SerializeField]
    float stackOffsetY = 10f;    // 重ね用Yオフセット

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
            DealPlayerCard();

            // ディーラーは交互に表・裏を切り替える
            // （ブラックジャック的な見せ方）
            DealDealerCard();
        }
    }

    // --------------------
    // 共通：カード生成
    // --------------------
    CardView CreateCardView(Card card, Transform root)
    {
        GameObject go = Instantiate(cardPrefab, root);
        CardView view = go.GetComponent<CardView>();

        // カードのデータをカードビューに渡す
        view.Init(card);
        view.SetCard(spriteDB.GetSprite(card));

        return view;
    }

    // --------------------
    // プレイヤー用配布（横並び）
    // --------------------
    void DealPlayerCard()
    {
        // 山札からカードデータを1枚引く
        Card card = deckManager.DrawCard();
        playerHand.Add(card);

        // 表示用カードオブジェクトを生成
        CardView view = CreateCardView(card, playerRoot);
        view.SetFace(false); // 初期は伏せ

        

        // 手札内でのインデックス
        int index = playerHand.Count - 1;
        int count = playerHand.Count;
        // 中央揃えになるようにオフセットを計算
        float offset = (count - 1) * cardSpacing * 0.5f;

        // 横一列に並べる
        view.transform.localPosition = new Vector3(
            index * cardSpacing - offset,
            0f,
            0f
        );

        // 手前に来るカードほど描画順を大きくする
        view.SetSortOrder(100 + index);
    }

    // --------------------
    // ディーラー用配布（3卓×重ね）
    // --------------------
    void DealDealerCard()
    {
        // 山札からカードデータを1枚引く
        Card card = deckManager.DrawCard();
        dealerHand.Add(card);

        // 表示用カードオブジェクトを生成
        CardView view = CreateCardView(card, dealerRoot);

        // 手札内でのインデックス
        int index = dealerHand.Count - 1;

        int tableIndex = index / 2;   // 0,1,2
        int stackIndex = index % 2;   // 下0 / 上1

        bool faceUp = (stackIndex == 1); // 上だけ表
        view.SetFace(faceUp);

        float x = (tableIndex - 1) * tableSpacing;
        float y = stackIndex * stackOffsetY;

        view.transform.localPosition = new Vector3(x, y, 0f);
        // 手前に来るカードほど描画順を大きくする
        view.SetSortOrder(200 + tableIndex * 10 + stackIndex);
    }


    //カード追加時に再ソート
    void RelayoutHand(List<Transform> cards)
    {
        int count = cards.Count;
        float offset = (count - 1) * cardSpacing * 0.5f;

        for (int i = 0; i < count; i++)
        {
            cards[i].localPosition = new Vector3(
                i * cardSpacing - offset,
                0f,
                0f
            );
        }
    }


}
