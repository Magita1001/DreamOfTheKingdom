using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    public CardManager CardManager;

    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;    
    private VisualElement cardContainer;

    private CardDataSO currentCardData;

    private List<Button> cardButtons = new List<Button>();

    private Button confirmButton;

    [Header("广播时间")]
    public ObjectEventSO finishPickcard;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmBUtton");

        confirmButton.clicked += ConfirmButton_clicked;

        for (int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            card.style.height = 320;
            var data = CardManager.GetNewCardData();
            //初始化
            InitCard(card, data);
            var cardButton = card.Q<Button>("Card");

            cardContainer.Add(card);
            cardButtons.Add(cardButton);

            cardButton.clicked += () => OnCardClicked(cardButton, data);
        }

    }

    private void ConfirmButton_clicked()
    {
        CardManager.UnLockCard(currentCardData);
        finishPickcard.RaisEvent(null, this);
    }

    private void OnCardClicked(Button cardButton, CardDataSO data)
    {
        currentCardData = data;
        Debug.Log("Card Click" + currentCardData.cardName);

        for (int i = 0; i < cardButtons.Count; i++)
        {
            if (cardButtons[i] == cardButton)
            {
                cardButtons[i].SetEnabled(false);
            }
            else
            {
                cardButtons[i].SetEnabled(true);
            }
        }

    }

    public void InitCard(VisualElement card, CardDataSO cardData)
    {
        var cardSpriteElement = card.Q<VisualElement>("CardSprite");
        var cardName = card.Q<Label>("CardName");
        var cardCost = card.Q<Label>("EnergyCost");
        var cardDescription = card.Q<Label>("CardDescription");
        var cardType = card.Q<Label>("CardType");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);

        cardName.text = cardData.cardName;
        cardCost.text = cardData.cost.ToString();
        cardDescription.text = cardData.description;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "能力",
            CardType.Abilities => "技能",
            _ => throw new System.NotImplementedException(),
        };
    }
}
