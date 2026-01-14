using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;

    private List<CardDataSO> drawDeck = new(); //≥È≈∆∂—

    private List<CardDataSO> discardDeck = new(); //∆˙≈∆∂—

    private List<Card> handCardObjectList = new(); //µ±«∞ ÷≈∆£®√øªÿ∫œ£©

    private void Start()
    {
        InitializeDeck();//≤‚ ‘
    }
    public void InitializeDeck()
    {
        drawDeck.Clear();
        foreach (var entry in cardManager.currentLibrary.cardLibraryList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }

        //TODO:œ¥≈∆/∏¸–¬≥È≈∆∂—or∆˙≈∆∂—µƒ ˝◊÷
    }
    [ContextMenu("TestDrawCard")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(drawDeck.Count == 0)
            {

            }
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            var card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            handCardObjectList.Add(card);
        }
    }
}
