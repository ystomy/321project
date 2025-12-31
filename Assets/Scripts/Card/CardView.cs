using UnityEngine;
using DG.Tweening;

// カード1枚の表示と演出を管理するクラス
// ・クリックで表裏を反転
// ・マウスホバーで少し持ち上がる演出
// ・DOTween使用

public class CardView : MonoBehaviour
{
    // 表面・裏面のスプライト
    public Sprite frontSprite;
    public Sprite backSprite;

    // 実際に表示する SpriteRenderer（Visual 配下）
    SpriteRenderer sr;

    // 現在カードが表向きかどうか
    bool isFaceUp;

    // フリップ演出中かどうか（多重操作防止用）
    bool isFlipping;

    // ホバー中かどうか（Tween多重発火防止用）
    bool isHovering;

    // 外部からフリップ可否を制御するためのフラグ
    public bool canFlip = true;

    // ホバー前の元のローカル座標
    Vector3 defaultPos;

    // ホバー演出用 Tween（途中キャンセル用に保持）
    Tween hoverTween;

    // ホバー時に持ち上げる高さ
    [SerializeField]
    float hoverOffsetY = 20f;

    // ホバー演出の速度
    [SerializeField]
    float hoverTime = 0.1f;

    // 見た目専用オブジェクト
    // 回転・位置演出を親と分離するための子Transform
    [SerializeField]
    Transform visual;

    void Awake()
    {
        // visual が未指定なら子オブジェクトから自動取得
        if (visual == null)
        {
            visual = transform.Find("Visual");
        }

        // 表示用 SpriteRenderer を取得
        sr = visual.GetComponent<SpriteRenderer>();

        // 初期状態は裏向き
        sr.sprite = backSprite;

        // ホバー解除時に戻すため初期位置を保存
        defaultPos = visual.localPosition;
    }

    // カードの表スプライトを設定し、状態をリセット
    public void SetCard(Sprite front)
    {
        frontSprite = front;
        sr.sprite = backSprite;
        isFaceUp = false;
    }

    // 強制的に表・裏を指定する（演出なし）
    // 山札生成時やリセット用
    public void SetFace(bool faceUp)
    {
        isFaceUp = faceUp;
        sr.sprite = isFaceUp ? frontSprite : backSprite;

        // 回転状態を初期化
        transform.localRotation = Quaternion.identity;
    }


    // 描画順を外部から制御する用
    public void SetSortOrder(int order)
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = order;
    }


    void OnMouseDown()
    {
        // フリップ不可・演出中・表スプライト未設定なら無視
        if (!canFlip || isFlipping || frontSprite == null) return;

        Flip();
    }

    // カードを回転させて表裏を切り替える
    void Flip()
    {
        isFlipping = true;

        // いったん90度まで回転してスプライトを切り替える
        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.15f)
            .OnComplete(() =>
            {
                // 回転途中で表裏を反転
                isFaceUp = !isFaceUp;
                sr.sprite = isFaceUp ? frontSprite : backSprite;

                // 元の角度に戻す
                transform.DOLocalRotate(Vector3.zero, 0.15f)
                    .OnComplete(() => isFlipping = false);
            });
    }

    void OnMouseEnter()
    {
        // フリップ中・すでにホバー中なら何もしない
        if (isFlipping || isHovering) return;

        isHovering = true;

        // 既存Tweenを停止してから新しく動かす
        hoverTween?.Kill();
        hoverTween = visual.DOLocalMoveY(
            defaultPos.y + hoverOffsetY,
            hoverTime
        );
    }

    void OnMouseExit()
    {
        // ホバーしていなければ無視
        if (!isHovering) return;

        isHovering = false;

        // 元の位置に戻す
        hoverTween?.Kill();
        hoverTween = visual.DOLocalMoveY(
            defaultPos.y,
            hoverTime
        );
    }

}
