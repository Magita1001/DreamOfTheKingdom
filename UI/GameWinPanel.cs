using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button pickCardButton;
    private Button backToMapButton;

    [Header("Ê±¼ä¹ã²¥")]
    public ObjectEventSO pickCardEvent;
    public ObjectEventSO loadMapEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = rootElement.Q<Button>("PickCardButton");
        backToMapButton = rootElement.Q<Button>("BackToMapButton");

        pickCardButton.clicked += PickCardButton_clicked;
        backToMapButton.clicked += BackToMapButton_clicked;
    }

    private void PickCardButton_clicked()
    {
        pickCardEvent.RaisEvent(null, this);
    }

    private void BackToMapButton_clicked()
    {
        loadMapEvent.RaisEvent(null, this);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardButton.style.display = DisplayStyle.None;
    }


}
