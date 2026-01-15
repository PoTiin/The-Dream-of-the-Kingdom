using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;
    [Header("»¡ÐÎ²ÎÊý")]
    public float angleBetweenCards = 7f;
    public float radius = 17f;
    public float maxAngle = 35f;

    public Vector3 centerPoint;

    [SerializeField]private List<Vector3> cardPosition = new();
    private List<Quaternion> cardRotations = new();

    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }
    public CardTransform GetCardTransform(int index,int totalCards)
    {
        CalculatePosition(totalCards, isHorizontal);
        return new CardTransform(cardPosition[index], cardRotations[index]);
    }

    private void CalculatePosition(int numberOfCards,bool horizontal)
    {
        cardPosition.Clear();
        cardRotations.Clear();
        if (horizontal)
        {
            float currentWidth = cardSpacing * (numberOfCards - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);
            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

            for (int i = 0; i < numberOfCards; i++)
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);

                var pos = new Vector3(xPos, centerPoint.y, 0f);
                var rotation = Quaternion.identity;
                cardPosition.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        else
        {
            float currentCardAngle = (numberOfCards - 1) * angleBetweenCards;
            float totalAngle = Mathf.Min(currentCardAngle, maxAngle);
            float leftAngle = totalAngle / 2;
            float spacingAngle = totalAngle > 0 ? totalAngle / (numberOfCards - 1) : 0;
            for (int i = 0; i < numberOfCards; i++)
            {
                var pos = FanCardPosition(leftAngle - i * spacingAngle);

                var rotation = Quaternion.Euler(0, 0, leftAngle - i * spacingAngle);
                cardPosition.Add(pos);
                cardRotations.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
            ); 
    }
}
