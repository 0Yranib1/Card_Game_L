using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;

    public Vector3 centerPoint;
    
    private List<Vector3> cardPositions=new List<Vector3>();
    private List<Quaternion> cardRotations=new List<Quaternion>();

    public CardTransform GetCardTransform(int index, int totalCards)
    {
        CalculatePosition(totalCards,isHorizontal);

        return new CardTransform(cardPositions[index], cardRotations[index]);
    }
    
    private void CalculatePosition(int numberOfCards, bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();
        if (horizontal)
        {
            float currentWidth = cardSpacing * (numberOfCards - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

            for (int i = 0; i < numberOfCards; i++)
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);
                var pos = new Vector3(xPos, centerPoint.y, 0);
                var rotation = Quaternion.identity;
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
    }
}
