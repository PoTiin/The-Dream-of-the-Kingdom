using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList;

    [Header("卡牌库")]

    public CardLibrarySO newGameCardLibrary;
    public CardLibrarySO currentLibrary;

    private int previousIndex;


    private void Awake()
    {
        InitializeCardDataList();
        //根据策划游戏的内容自行调整
        foreach (var item in newGameCardLibrary.cardLibraryList)
        {
            currentLibrary.cardLibraryList.Add(item);
        }
    }
    private void OnDisable()
    {
        currentLibrary.cardLibraryList.Clear();
    }
    #region 获取项目卡牌
    /// <summary>
    /// 初始化获得所有项目卡牌资源
    /// </summary>
    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("No CardData Found!");
        }
    }
    #endregion
    /// <summary>
    /// 抽卡时调用的函数获得卡牌GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetCardObject()
    {
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
    }

    public void DiscardCard(GameObject obj)
    {
        poolTool.ReleaseObjectToPool(obj);
    }

    public CardDataSO GetNewCardData()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = Random.Range(0, cardDataList.Count);
        } while (previousIndex == randomIndex);

        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    /// <summary>
    /// 解锁新卡牌
    /// </summary>
    /// <param name="newCardData"></param>
    public void UnlockCard(CardDataSO newCardData)
    {
        var newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1,
        };
        if (currentLibrary.cardLibraryList.Contains(newCard))
        {
            int targetIndex = currentLibrary.cardLibraryList.FindIndex(t => t.cardData == newCardData);
            CardLibraryEntry ce = new CardLibraryEntry();
            ce.amount = currentLibrary.cardLibraryList[targetIndex].amount + 1;
            ce.cardData = currentLibrary.cardLibraryList[targetIndex].cardData;
            currentLibrary.cardLibraryList[targetIndex] = ce;
            //target.amount++;
        }
        else
        {
            currentLibrary.cardLibraryList.Add(newCard);
        }
    }
}
