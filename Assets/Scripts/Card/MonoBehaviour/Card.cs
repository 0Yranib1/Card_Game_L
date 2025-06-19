using System;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("组件")] 
    public SpriteRenderer cardSprite;

    public TextMeshPro costText, descriptionText, typeText;
    public CardDataSO cardData;

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
}
