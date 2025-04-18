using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton, QuitGameButton;

    public ObjectEventSO newGameEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");
        QuitGameButton = rootElement.Q<Button>("QuitGameButton");

        newGameButton.clicked += NewGameButton_clicked;
        QuitGameButton.clicked += QuitGameButton_clicked;
    }

    private void QuitGameButton_clicked()
    {
        Application.Quit();
    }

    private void NewGameButton_clicked()
    {
        newGameEvent.RaisEvent(null, this);
    }
}
