using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class ReatRoomPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button restRoom, backToMap;

    public Effect effect;
    public ObjectEventSO loadMap;

    private CharacterBase player;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        restRoom = rootElement.Q<Button>("RestButton");
        backToMap = rootElement.Q<Button>("BackToMapRoom");

        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);

        restRoom.clicked += RestRoom_clicked;
        backToMap.clicked += BackToMap_clicked;
    }

    private void BackToMap_clicked()
    {
        loadMap.RaisEvent(null, this);
    }

    private void RestRoom_clicked()
    {
        effect.Execute(player, null);
        restRoom.SetEnabled(false);
    }
}
