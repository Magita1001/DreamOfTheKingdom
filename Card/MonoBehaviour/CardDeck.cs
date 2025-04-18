using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using System.Collections;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager layoutManager;
    public Vector3 deckPositon;

    private List<CardDataSO> drawDeck = new List<CardDataSO>();   //抽牌堆
    private List<CardDataSO> disCardDeck = new List<CardDataSO>();//弃牌堆
    private List<Card> handCardObjectList=new List<Card>();       //手牌(每回合)

    [Header("事件广播")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;


    //测试用 记得删
    private void Start()
    {
        InitializaDeck();
        
    }

    public void InitializaDeck()
    {
        foreach (var entry in cardManager.currentLibrary.cardLibraryList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }

        //TODU：洗牌/更新抽牌堆、弃牌堆显示的数字
        ShuffleDeck();

    }

    [ContextMenu("测试抽牌")]
    private void TestDrawCard()
    {
        DrawCard(1);
    }

    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void NewTurnDrawCards()
    {
        DrawCard(4);
    }

    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                //TODU：洗牌/更新抽牌堆、弃牌堆显示的数字
                foreach (var item in disCardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }
            CardDataSO currentCardData = drawDeck[0]; //得到牌堆顶的卡牌类型
            drawDeck.RemoveAt(0);                     //得到后从抽牌堆移除
            //移除牌后更新UI
            drawCountEvent.RaisEvent(drawDeck.Count, this);

            var card = cardManager.GetCardObject().GetComponent<Card>(); //从对象池中拿出一个卡牌对象
            //初始化
            card.Init(currentCardData);
            
            card.transform.position = deckPositon;            
            

            handCardObjectList.Add(card); //最后添加到手牌列表中

            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }

    /// <summary>
    /// 设置卡牌动画、位置、层级
    /// </summary>
    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform = layoutManager.GetCardTransform(i, handCardObjectList.Count);

            //currentCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rotation);

            //卡牌能量判断
            currentCard.UpdateCardState();

            currentCard.isAnimating = true;

            //先执行缩放动画  添加延迟 并添加在缩放结束后移动的事件
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

    public void UpdateCardState()
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];                       
            //卡牌能量判断
            currentCard.UpdateCardState();
        }
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        disCardDeck.Clear();
        //TODO：更新UI显示的数量
        drawCountEvent.RaisEvent(drawDeck.Count, this);
        discardCountEvent.RaisEvent(disCardDeck.Count, this);

        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 弃牌,从手牌移除，然后加入到弃牌堆逻辑、事件函数
    /// </summary>
    /// <param name="card"></param>
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;
        disCardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);        

        cardManager.DiscardCard(card.gameObject);

        discardCountEvent.RaisEvent(disCardDeck.Count, this);//弃牌后也更新UI

        SetCardLayout(0f);
    }

    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            disCardDeck.Add(handCardObjectList[i].cardData);
            cardManager.DiscardCard(handCardObjectList[i].gameObject);
        }
        handCardObjectList.Clear();
        discardCountEvent.RaisEvent(disCardDeck.Count, this);//弃牌后也更新UI
    }

    public void ReliseAllCards(object obj)
    {
        foreach (var card in handCardObjectList)
        {
            cardManager.DiscardCard(card.gameObject);
        }
        handCardObjectList.Clear();
        InitializaDeck();
    } 

    
}
