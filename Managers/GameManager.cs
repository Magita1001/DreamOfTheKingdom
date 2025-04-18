using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    public List<Enemy> aliveEnemyList;

    [Header("事件通知")]
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;


    public void UpdateMapLayoutData(object value)
    {
        var roomVector=(Vector2Int)value;

        if (mapLayout.mapRoomDataList.Count == 0)
            return;

        //根据传入的roomVector在mapRoomDataList列表中找出房间
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.colum == roomVector.x && r.line == roomVector.y);

        //把当前访问的房间状态设为访问过
        currentRoom.roomState = RoomState.Visited;

        //更新相邻房间的数据 把同一列中别的房间锁上
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.colum == currentRoom.colum);
       
        foreach (var room in sameColumnRooms)
        {
            //筛选出同一列中 不是同一行的房间
            if (room.line != roomVector.y)
            {
                room.roomState = RoomState.Locked;
            }            
        }

        //把连接的房间状态设为可进入
        foreach (var link in currentRoom.linkTo)
        {
            var linkRoom = mapLayout.mapRoomDataList .Find(r => r.colum == link.x && r.line == link.y);
            linkRoom.roomState = RoomState.Attainable;
        }

        aliveEnemyList.Clear();
    }

    public void OnRoomLoadEvent(object obj)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            aliveEnemyList.Add(enemy);
        }
    }


    /// <summary>
    /// 人物死亡的事件
    /// </summary>
    /// <param name="character"></param>
    public void OnCharacterDeadEvent(object character)
    {
        if (character is Player)
        {
            //发出失败的通知
            StartCoroutine(EventDelayAction(gameOverEvent));
        }

        if (character is Boss)
        {
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        else if (character is Enemy)
        {

            aliveEnemyList.Remove(character as Enemy);

            if (aliveEnemyList.Count == 0)
            {
                //发出胜利的通知
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }

        
    }
    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO?.RaisEvent(null,this);
    }

    public void OnNewGame()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
    }
}
