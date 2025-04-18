using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList; //游戏中所有可能出现的卡牌

    [Header("卡牌库")]
    public CardLibrarySO newGameCardLibrary; //新游戏时初始化的卡牌库
    public CardLibrarySO currentLibrary;     //当前玩家开牌库

    private int previousIndex;
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


    #region 获得项目卡牌
    /// <summary>
    /// 初始化获得所有项目卡牌资源
    /// </summary>
    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += CardManager_Completed;
    }

    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="obj"></param>
    private void CardManager_Completed(AsyncOperationHandle<IList<CardDataSO>> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(obj.Result);
        }
        else
        {
            Debug.Log("No CardData Found");
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
        poolTool.ReturnObjectToPool(obj);
    }
    
    public CardDataSO GetNewCardData()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = Random.Range(randomIndex, cardDataList.Count);
        } while (previousIndex == randomIndex);
        
        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }


    /// <summary>
    /// 解锁、添加新卡牌
    /// </summary>
    /// <param name="newCardData"></param>
    public void UnLockCard(CardDataSO newCardData)
    {
        var newCard = new CardLibrarySOEntry
        {
            cardData = newCardData,
            amount = 1
        };

        if (currentLibrary.cardLibraryList.Contains(newCard))
        {
            var target = currentLibrary.cardLibraryList.Find(t => t.cardData == newCardData);
            target.amount++;
        }
        else
        {
            currentLibrary.cardLibraryList.Add(newCard);
        }
    }

}
