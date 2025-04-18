 using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayPanel : MonoBehaviour
{
    [Header("�¼��㲥")]
    public ObjectEventSO playerTurnEndEvent;


    private VisualElement rootElement;
    private Label energyAmountLabel, drawAmountLabel, discardAmountLabel, turnLabel;
    private Button endTurnButton;



    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;

        //���������UIԪ�غ��¼������߼�
        energyAmountLabel = rootElement.Q<Label>("EneergyAmount");
        drawAmountLabel = rootElement.Q<Label>("DrawAmount");
        discardAmountLabel = rootElement.Q<Label>("DiscardAmount");
        turnLabel = rootElement.Q<Label>("TurnLabel");
        endTurnButton = rootElement.Q<Button>("EndTurn");

        endTurnButton.clicked += EndTurnButton_clicked;

        energyAmountLabel.text = "0";
        drawAmountLabel.text = "0";
        discardAmountLabel.text = "0";
        turnLabel.text = "��Ϸ��ʼ";


    }

    private void EndTurnButton_clicked()
    {
        playerTurnEndEvent.RaisEvent(null, this);
    }

    public void UpdateDrawDeckAmount(int amount)
    {
        drawAmountLabel.text = amount.ToString();
    }

    public void UpdateDiscardDeckAmount(int amount)
    {
        discardAmountLabel.text = amount.ToString();
    }

    public void UpdateEnergyAmount(int amount)
    {
        energyAmountLabel.text = amount.ToString();
    }


    public void OnEnemyTurnBegin()
    {
        endTurnButton.SetEnabled(false);
        turnLabel.text = "���˻غ�";
        turnLabel.style.color = new StyleColor(Color.red);
    }

    public void OnPlayerTurnBegin()
    {
        endTurnButton.SetEnabled(true);
        turnLabel.text = "��һغ�";
        turnLabel.style.color = new StyleColor(Color.white);
    }


}

