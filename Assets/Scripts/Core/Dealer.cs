using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    [SerializeField]
    DeckManager deckManager;
    [SerializeField]
    CardSpriteDB spriteDB;
    [SerializeField]
    GameObject cardPrefab;

    [Header("”z’u")]
    public Transform playerRoot;
    public Transform dealerRoot;

    public int initialDealCount = 6;

    public List<Card> playerHand = new List<Card>();
    public List<Card> dealerHand = new List<Card>();
    [SerializeField]
    private float cardSpacing = 100f;
    public DealerData dealerData;
    [SerializeField]
    private LuckHeatManager luckHeat;

    void Start()
    {
        luckHeat.InitializeFromDealer(dealerData);
        DealInitialCards();
    }

    public void DealInitialCards()
    {
        playerHand.Clear();
        dealerHand.Clear();

        for (int i = 0; i < initialDealCount; i++)
        {
            DealOne(playerHand, playerRoot, false);

            bool dealerFaceUp = (i % 2 == 0);
            DealOne(dealerHand, dealerRoot, dealerFaceUp);
        }
    }

    void DealOne(List<Card> hand, Transform root, bool faceUp)
    {
        Card card = deckManager.DrawCard();
        hand.Add(card);

        GameObject go = Instantiate(cardPrefab, root);
        CardView view = go.GetComponent<CardView>();

        view.SetCard(spriteDB.GetSprite(card));
        view.SetFace(faceUp);

        int baseOrder = 100;
        int index = hand.Count - 1;

        float offset = (hand.Count - 1) * cardSpacing * 0.5f;
        go.transform.localPosition = new Vector3(
            index * cardSpacing - offset,
            0f,
            0f
        );

        view.SetSortOrder(baseOrder + index);

    }
}
