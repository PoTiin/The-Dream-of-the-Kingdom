using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager layoutManager;

    public Vector3 deckPosition;

    private List<CardDataSO> drawDeck = new(); //≥È≈∆∂—

    private List<CardDataSO> discardDeck = new(); //∆˙≈∆∂—

    private List<Card> handCardObjectList = new(); //µ±«∞ ÷≈∆£®√øªÿ∫œ£©

    private void Start()
    {
        InitializeDeck();//≤‚ ‘
        DrawCard(3);
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
                //TODO:œ¥≈∆/∏¸–¬≥È≈∆∂—or∆˙≈∆∂—µƒ ˝◊÷
            }
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            var card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            card.transform.position = deckPosition;
            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
        
    }

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform = layoutManager.GetCardTransform(i, handCardObjectList.Count);
            //currentCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rotation);

            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransform.pos, 0.5f).onComplete = () => currentCard.isAnimating = false;
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
            };
            //…Ë÷√ø®≈∆≈≈–Ú
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }
}
