using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column; //代表纵向
    public int line;   //代表横向


    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;

    public List<Vector2Int> linkTo = new();

    [Header("广播")]
    public ObjectEventSO loadRoomEvent;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("鼠标点击房间" + roomData.roomType);
        if (roomState == RoomState.Attainable)
        {
            loadRoomEvent.RaisEvent(this, this);
        }
        
    }

    /// <summary>
    /// 外部创建房间时调用配置房间
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    /// <param name="roomDataSO"></param>
    public void SetupRoom(int column, int line, RoomDataSO roomDataSO)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomDataSO;

        spriteRenderer.sprite = roomDataSO.roomIcon;

        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => new Color(0.5f, 0.5f, 0.5f, 1f),
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 0.5f),
            RoomState.Attainable => Color.white,
            _ => throw new System.NotImplementedException(),
        };
    }
}
