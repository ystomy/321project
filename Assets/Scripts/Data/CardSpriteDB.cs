using UnityEngine;

[CreateAssetMenu(menuName = "Card/CardSpriteDB")]
public class CardSpriteDB : ScriptableObject
{
    public Sprite[] spade;
    public Sprite[] heart;
    public Sprite[] diamond;
    public Sprite[] club;

    public Sprite[] joker;

    public Sprite GetSprite(Card card)
    {
        // ジョーカー
        if (card.suit == 4)
        {
            int index = Mathf.Clamp(card.number, 0, joker.Length - 1);
            return joker[index];
        }

        int i = card.number - 1;

        switch (card.suit)
        {
            case 0:
                return spade[i];
            case 1:
                return heart[i];
            case 2:
                return diamond[i];
            case 3:
                return club[i];
            default:
                return null;
        }
    }

}
