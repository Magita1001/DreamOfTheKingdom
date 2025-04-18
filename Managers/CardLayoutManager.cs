using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌位置管理
/// </summary>
public class CardLayoutManager : MonoBehaviour
{
    public static CardLayoutManager Instance { get; private set; }

    public bool isHorizontal;
    public float maxWifth = 7;
    public float cardsSpacing = 2f;

    [Header("弧形参数")]
    public float angleBetweenCards = 7f;
    public float radius = 17f;

    public Vector3 centerPoint; //中心点

    private List<Vector3> cardPosition = new List<Vector3>();       //记录卡牌位置
    private List<Quaternion> cardQRotations = new List<Quaternion>();//记录卡牌角度

    private void Awake()
    {
        Instance = this;
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    public CardTransform GetCardTransform(int index, int totalCards)
    {
        CalculatePosition(totalCards, isHorizontal);

        return new CardTransform(cardPosition[index], cardQRotations[index]);
    }

    private void CalculatePosition(int numberOfCards, bool horizontal)
    {
        cardPosition.Clear();
        cardQRotations.Clear();
        if (horizontal)
        {
            float currentWidth = cardsSpacing * (numberOfCards - 1);   //总宽度
            float totalWidth = Mathf.Min(currentWidth, maxWifth);//保证总宽度不超过最大值
            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0; //每张牌的间隙

            for (int i = 0; i < numberOfCards; i++)
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing); //0 - (totalWidth / 2)决定最左侧的位置

                var pos = new Vector3(xPos, centerPoint.y, 0);
                var rotation = Quaternion.identity;

                cardPosition.Add(pos);
                cardQRotations.Add(rotation);
            }
        }
        else
        {
            float currentAngle = angleBetweenCards * (numberOfCards - 1); // 总角度
            float totalAngle = Mathf.Min(currentAngle, 45f); // 保证总角度不超过180度
            float currentSpacing = totalAngle > 0 ? totalAngle / (numberOfCards - 1) : 0; // 每张牌的角度间隙

            float cardAngle = (numberOfCards - 1) * currentSpacing / 2;

            for (int i = 0; i < numberOfCards; i++)
            {
                var pos = FanCardPosition(cardAngle - i * currentSpacing);

                var rotation = Quaternion.Euler(0, 0, cardAngle - i * currentSpacing);

                cardPosition.Add(pos);
                cardQRotations.Add(rotation);
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
