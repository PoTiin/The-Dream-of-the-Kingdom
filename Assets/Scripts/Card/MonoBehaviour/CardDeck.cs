using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager layoutManager;

    public Vector3 deckPosition;

    private List<CardDataSO> drawDeck = new(); //抽牌堆

    private List<CardDataSO> discardDeck = new(); //弃牌堆

    private List<Card> handCardObjectList = new(); //当前手牌（每回合）

    [Header("事件广播")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;

    private void Start()
    {
        InitializeDeck();//测试
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

        //TODO:洗牌/更新抽牌堆or弃牌堆的数字
        ShuffleDeck();
    }
    [ContextMenu("TestDrawCard")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
    /// <summary>
    /// 事件监听事件
    /// </summary>
    public void NewTurnDrawCards()
    {
        DrawCard(4);
    }
    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(drawDeck.Count == 0)
            {

                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            drawCountEvent.RaiseEvent(drawDeck.Count, this);
            var card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            card.transform.position = deckPosition;
            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
        
    }
    /// <summary>
    /// 设置卡牌布局
    /// </summary>
    /// <param name="delay"></param>
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
            //设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }
    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        discardDeck.Clear();
        //TODO:更新UI显示数量
        drawCountEvent.RaiseEvent(drawDeck.Count, this);
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }
    /// <summary>
    /// 弃牌逻辑
    /// </summary>
    /// <param name="card"></param>
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        cardManager.DiscardCard(card.gameObject);
        SetCardLayout(0f);
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
    }
}
