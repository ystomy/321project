using UnityEngine;
using DG.Tweening;

public class CardView : MonoBehaviour
{
    public Sprite frontSprite;
    public Sprite backSprite;


    SpriteRenderer sr;
    bool isFaceUp;
    bool isFlipping;
    bool isHovering;

    public bool canFlip = true;

    Vector3 defaultPos;
    Tween hoverTween;

    [SerializeField]
    float hoverOffsetY = 20f;
    [SerializeField]
    float hoverTime = 0.1f;
    [SerializeField]
    Transform visual;   // ★追加（SpriteRendererが付いてる子）

    void Awake()
    {
        if (visual == null)
        {
            visual = transform.Find("Visual");
        }

        sr = visual.GetComponent<SpriteRenderer>();
        sr.sprite = backSprite;

        defaultPos = visual.localPosition;
    }

    public void SetCard(Sprite front)
    {
        frontSprite = front;
        sr.sprite = backSprite;
        isFaceUp = false;
    }

    public void SetFace(bool faceUp)
    {
        isFaceUp = faceUp;
        sr.sprite = isFaceUp ? frontSprite : backSprite;
        transform.localRotation = Quaternion.identity;
    }

    public void SetSortOrder(int order)
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = order;
    }


    void OnMouseDown()
    {
        if (!canFlip || isFlipping || frontSprite == null) return;
        Flip();
    }

    void Flip()
    {
        isFlipping = true;

        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.15f)
            .OnComplete(() =>
            {
                isFaceUp = !isFaceUp;
                sr.sprite = isFaceUp ? frontSprite : backSprite;

                transform.DOLocalRotate(Vector3.zero, 0.15f)
                    .OnComplete(() => isFlipping = false);
            });
    }

    void OnMouseEnter()
    {
        if (isFlipping || isHovering) return;

        isHovering = true;

        hoverTween?.Kill();
        hoverTween = visual.DOLocalMoveY(
            defaultPos.y + hoverOffsetY,
            hoverTime
        );
    }

    void OnMouseExit()
    {
        if (!isHovering) return;

        isHovering = false;

        hoverTween?.Kill();
        hoverTween = visual.DOLocalMoveY(
            defaultPos.y,
            hoverTime
        );
    }

}
