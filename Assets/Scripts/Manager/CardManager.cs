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
    //初始化卡牌库
    public CardLibrarySO newGameCardLibrary;
    //玩家卡牌库
    public CardLibrarySO currentLibrary;
    
    private void Awake()
    {
        InitializeCardDataList();
        
        foreach (var item in newGameCardLibrary.cardLibraryList)
        {
            currentLibrary.cardLibraryList.Add(item);
        }
    }

    private void OnDisable()
    {
        currentLibrary.cardLibraryList.Clear();
    }

    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>(key: "CardData").Completed += OnCardDataLoaded;
    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("卡牌资源加载失败");
        }
    }

    /// <summary>
    /// 抽卡时获得卡牌gameobject
    /// </summary>
    /// <returns></returns>
    public GameObject GetCardObject()
    {
        var cardObj= poolTool.GetObjectFromPool();
        cardObj.transform.localScale=Vector3.zero;
        return cardObj;
    }

    public void DiscardCard(GameObject cardObj)
    {
        poolTool.ReturnObjectToPool(cardObj);
    }
}
