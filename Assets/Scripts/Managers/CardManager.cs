using System;
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


    private void Awake()
    {
        InitializeCardDataList();
        //根据策划游戏的内容自行调整
        foreach (var item in newGameCardLibrary.cardLibraryList)
        {
            currentLibrary.cardLibraryList.Add(item);
        }
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

    public GameObject GetCardObject()
    {
        return poolTool.GetObjectFromPool();
    }

    public void DiscardCard(GameObject obj)
    {
        poolTool.ReleaseObjectToPool(obj);
    }

}
