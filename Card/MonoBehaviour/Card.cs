using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [Header("组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText, descriptionText, typeText, cardName;

    public CardDataSO cardData;

    [Header("原始数据")]
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;

    public bool isAnimating;
    public bool isAvailiable;

    public Player player;

    [Header("广播事件")]
    public ObjectEventSO discardCardEvent;
    public IntEventSO costEvent;
    public ObjectEventSO UpDateCardState;


    private void Start()
    {
        Init(cardData);
    }

    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardImage;
        costText.text = data.cost.ToString();
        descriptionText.text = data.description;
        cardName.text = data.cardName;
        typeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException(),
        };

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void UpdatePositionRotation(Vector3 postion, Quaternion rotation)
    {
        originalPosition = postion;
        originalRotation = rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAnimating) return;
        transform.position = CardLayoutManager.Instance.isHorizontal ? originalPosition + Vector3.up : new Vector3(originalPosition.x, -3.5f, 0);
        transform.rotation = Quaternion.identity;

        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating) return;
        RsetCardTransform();
    }

    public void RsetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    /// <summary>
    /// 打出卡牌 执行卡牌效果 随后回收卡牌
    /// </summary>
    /// <param name="form"></param>
    /// <param name="target"></param>
    public void ExecuteCardEffects(CharacterBase form, CharacterBase target)
    {        
        //TODO:打出卡牌后减少能量 通知回收卡牌 
        costEvent.RaisEvent(cardData.cost, this);
        discardCardEvent.RaisEvent(this, this);
        

        foreach (var effect in cardData.effects)
        {
            effect.Execute(form, target);
        }

        UpDateCardState.RaisEvent(null, this);
    }

    public void UpdateCardState()
    {
        isAvailiable = cardData.cost <= player.CurrentMana;
        costText.color = isAvailiable ? Color.green : Color.red;
    }
}
