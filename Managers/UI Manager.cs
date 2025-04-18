using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject gameplayerPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject RestRoomPanel;

    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = (Room)data;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gameplayerPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                RestRoomPanel.SetActive(true);
                break;

        }
    }

    /// <summary>
    /// loadMap时调用
    /// </summary>
    public void HideAllPanel()
    {
        gameplayerPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        RestRoomPanel.SetActive(false);
    }

    public void OnGameWinEVent()
    {
        gameplayerPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    public void OnGameOverEVent()
    {
        gameplayerPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }
}
