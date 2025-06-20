using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;

    public CardLayoutManager LayoutManager;

    public Vector3 deckPosition;
    
    //抽牌堆
    private List<CardDataSO> drawDeck = new List<CardDataSO>();
    //弃牌堆
    private List<CardDataSO> discardDeck = new List<CardDataSO>();

    //当前回合手牌
    private List<Card> handCardObjectList = new List<Card>();


    private void Start()
    {
        //测试用
        InitializeDeck();
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
        //洗牌
        //更新抽牌堆弃牌堆相关显示
    }

    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
    
    private void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                //洗牌
            }

            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            var card = cardManager.GetCardObject().GetComponent<Card>();
            //初始化卡牌
            card.Init(currentCardData);
            card.transform.position = deckPosition;
            
            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout( delay);
        }

    }

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard=handCardObjectList[i];
            CardTransform cardTransform = LayoutManager.GetCardTransform(i, handCardObjectList.Count);

            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, 0.4f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransform.pos, 0.5f).onComplete=()=>currentCard.isAnimating = false;
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
            };
            
            //设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }
    
}
