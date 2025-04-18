using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置表")]
    public MapConfigSO mapConfigSO;

    [Header("预制体")]
    public Room roomPrefab;

    [Header("地图布局")]
    public MapLayoutSO mapLayout;


    public LineRenderer linePrefab;

    private float screenHeight;
    private float screenWidth;

    private float columnWidth;
    private Vector3 generatePoint;

    public float border;

    private List<Room> rooms = new List<Room>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    public List<RoomDataSO> roomDataList = new List<RoomDataSO>();
    public Dictionary<RoomType, RoomDataSO> roomDataDic= new Dictionary<RoomType, RoomDataSO>();

    private void Awake()
    {
        //得到屏幕比例在世界坐标系下的长宽
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        columnWidth = screenWidth / (mapConfigSO.roomBlueprints.Count);

        foreach (var room in roomDataList)
        {
            roomDataDic.Add(room.roomType, room);
        }
    }
    //private void Start()
    //{
    //    CreateMap();
    //}

    private void OnEnable()
    {
        if (mapLayout.mapRoomDataList.Count > 0)
        {
            LoadMap();
        }
        else
        {
            CreateMap();
        }
    }

    public void CreateMap()
    {
        //创建前一列房间列表
        List<Room> previousColumnRooms = new List<Room>();

        for (int column = 0; column < mapConfigSO.roomBlueprints.Count; column++)
        {
            RoomBlueprint Blueprint = mapConfigSO.roomBlueprints[column];
            int amount = UnityEngine.Random.Range(Blueprint.min, Blueprint.max);

            float starHeight = screenHeight / 2 - screenHeight / (amount + 1);

            generatePoint = new Vector3(-screenWidth / 2 + border + columnWidth * column, starHeight, 0);

            Vector3 newPosition = generatePoint;
            var roomGapY = screenHeight / (amount + 1);

            //当前列房间列表
            List<Room> currentColumnRooms = new List<Room>();
     

            //循环当前列以创建房间
            for (int i = 0; i < amount; i++)
            {
                //判断是否是第一列与最后一列 第一列x不偏移，最后一列位置固定
                if(column == mapConfigSO.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 2;
                }else if (column != 0)
                {
                    newPosition.x = generatePoint.x + UnityEngine.Random.Range(-border / 2, border / 2);
                }

                newPosition.y = starHeight - roomGapY * i;
                //生成房间
                Room room = Instantiate(roomPrefab, newPosition, Quaternion.identity, this.transform);
                RoomType newType = GetRandomRoomType(mapConfigSO.roomBlueprints[column].roomType);

                //在SetupRoom前设置房间的状态 只有第一列的房间可以进入
                if (column == 0)
                {
                    room.roomState = RoomState.Attainable;
                }
                else
                {
                    room.roomState |= RoomState.Locked;
                }

                room.SetupRoom(column, i, GetRoomData(newType));               
                //将此房间存入房间列表中
                rooms.Add(room);
                //将此房间存入本列房间列表中
                currentColumnRooms.Add(room);
            }

            //判断当前列是否为第一列 不是则连接上一列 能进到这个if里说明这不是第一列
            if (previousColumnRooms.Count > 0)
            {
                //创建两个房间之间连线
                CreateConnections(previousColumnRooms, currentColumnRooms);
            }
            previousColumnRooms = currentColumnRooms;
        }

        SaveMap();
    }

    /// <summary>
    /// 重新生成地图
    /// </summary>
    [ContextMenu("ReGenerateRoom")]
    public void ReGenerateRoom()
    {
        foreach (var item in rooms)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in lines)
        {
            Destroy(item.gameObject);
        }
        rooms.Clear();
        lines.Clear();

        CreateMap();
    }

    private void CreateConnections(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms = new HashSet<Room>(); //代表第二列中已经被连上的房间
        
        //让第一列中的房间随机连接到第二列中的房间
        foreach (var room in column1)
        {
            Room targetRoom = ConnectToRandomRoom(room, column2, false);
            connectedColumn2Rooms.Add(targetRoom);
        }
        //手动连接没有连接到第二排的房间
        foreach (var room in column2)
        {
            if (!connectedColumn2Rooms.Contains(room))
            {
                ConnectToRandomRoom(room, column1, true);
            }
        }
    }

    private Room ConnectToRandomRoom(Room room, List<Room> column2, bool check)
    {
        Room targetRoom;
        targetRoom = column2[UnityEngine.Random.Range(0, column2.Count)];

        if (check)
        {
            targetRoom.linkTo.Add(new(room.column, room.line));
        }
        else
        {
            room.linkTo.Add(new(targetRoom.column, targetRoom.line));
        }

        //创建房间之间的连线
        var line = Instantiate(linePrefab,this.transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);

        lines.Add(line);
        return targetRoom;
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDic[roomType];
    }
    /// <summary>
    /// 通过传入的RoomType列表 随机挑选一个返回出去
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(",");
        string randomOptions = options[UnityEngine.Random.Range(0, options.Length)];

        RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType), randomOptions);
        return roomType;
    }

    public void SaveMap()
    {
        mapLayout.mapRoomDataList = new();

        //添加所有已经生成的房间
        for (int i = 0; i < rooms.Count; i++)
        {
            var room = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                colum = rooms[i].column,
                line = rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].linkTo,
            };
            mapLayout.mapRoomDataList.Add(room);
        }

        mapLayout.linePositionList = new();
        //添加所有连线
        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition()
            {
                startPos = new SerializVector3(lines[i].GetPosition(0)),
                endPos = new SerializVector3(lines[i].GetPosition(1)),
            };
            mapLayout.linePositionList.Add(line);
        }


    }
    public void LoadMap()
    {
        //读取房间数据生成房间
        for (int i = 0; i < mapLayout.mapRoomDataList.Count; i++)
        {
            var newPos = new Vector3(mapLayout.mapRoomDataList[i].posX, mapLayout.mapRoomDataList[i].posY, 0);
            var newRoom = Instantiate(roomPrefab, newPos, Quaternion.identity, this.transform);
            newRoom.roomState = mapLayout.mapRoomDataList[i].roomState;
            newRoom.SetupRoom(mapLayout.mapRoomDataList[i].colum, mapLayout.mapRoomDataList[i].line, mapLayout.mapRoomDataList[i].roomData);
            newRoom.linkTo = mapLayout.mapRoomDataList[i].linkTo;
            rooms.Add(newRoom);
        }

        //读取连线
        for (int i = 0; i < mapLayout.linePositionList.Count; i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.linePositionList[i].startPos.ToVector3());
            line.SetPosition(1, mapLayout.linePositionList[i].endPos.ToVector3());

            lines.Add(line);
        }
    }
}
