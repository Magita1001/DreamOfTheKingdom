using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    private Button backToStartButton;
    public ObjectEventSO loadMenuEvent;

    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartButton").clicked += GameOverPanel_clicked; ;
    }

    private void GameOverPanel_clicked()
    {
        loadMenuEvent.RaisEvent(null, this);   
    }
}
