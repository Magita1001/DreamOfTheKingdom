using UnityEngine;

public class TurnBaseManger : MonoBehaviour
{
    public GameObject playerObj;

    private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;

    public bool battleEnd = true;

    [SerializeField]
    private float timeCounter; //�غϼ�ʱ��

    public float enemyTurnDuration;
    public float playerTurnDuration;

    [Header("�¼��㲥")]
    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;

    private void Update()
    {
        if (battleEnd)
            return;
        if(isEnemyTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration)
            {
                timeCounter = 0f;
                //���˻غϽ���
                EnemyTurnEnd();
                //��һغϿ�ʼ
                isPlayerTurn = true;
            }
        }

        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0f;
                //��һغϿ�ʼ
                PlayerTurnBegin();
                isPlayerTurn = false;
            }
        }
    }

    [ContextMenu("��Ϸ��ʼ")]
    public void GameStart()
    {
        isPlayerTurn = true;
        isEnemyTurn = false;
        battleEnd = false;
        timeCounter = 0;
    }

    public void PlayerTurnBegin()
    {
        playerTurnBegin.RaisEvent(null, this);
    }

    public void EnemyTurnBegin()
    {
        isEnemyTurn = true;
        enemyTurnBegin.RaisEvent(null, this);
    }

    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        enemyTurnEnd.RaisEvent(null, this);
    }

    /// <summary>
    /// ע��ʱ�亯�� afterRoomLoad
    /// </summary>
    public void OnRoomLoadedEvent(object obj)
    {
        Room room = obj as Room;
        switch (room.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                playerObj.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
            case RoomType.Treasure:
                playerObj.SetActive(false);
                break;
            case RoomType.RestRoom:
                playerObj.SetActive(true);
                playerObj.GetComponent<PlayerAnimation>().SetSleepAnimator();
                break;
        }
    }

    public void StopTurnBaseSystem(object obj)
    {
        battleEnd = true;
        playerObj.SetActive(false);
    }

    public void NewGame()
    {
        playerObj.GetComponent<Player>().NewGame();
    }

}
