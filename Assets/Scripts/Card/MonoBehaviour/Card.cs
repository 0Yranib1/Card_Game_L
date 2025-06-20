using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [Header("组件")] 
    public SpriteRenderer cardSprite;

    public TextMeshPro costText, descriptionText, typeText;
    public CardDataSO cardData;

    [Header("原始数据")] 
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;

    public bool isAnimating;
    private void Start()
    {
        Init(cardData);
    }

    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardImage;
        costText.text = data.cost.ToString();
        typeText.text = data.cardType switch
        {
            CardType.Attack=>"攻击",
            CardType.Defense=>"技能",
            CardType.Abilities=>"能力",
            _=>"无"
        };
        descriptionText.text = data.description;
    }


    public void UpdatePositionRotation(Vector3 position, Quaternion rotation)
    {
        originalPosition=position;
        originalRotation=rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAnimating)
        {
            return;
        }
        transform.position = originalPosition + Vector3.up;
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder + 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isAnimating)
            return;
        ResetCardTransform();
    }

    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }
}
