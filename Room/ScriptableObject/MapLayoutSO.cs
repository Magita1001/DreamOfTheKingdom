using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图布局数据
/// </summary>
[CreateAssetMenu(fileName = "MapConfigSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> mapRoomDataList = new List<MapRoomData>();
    public List<LinePosition> linePositionList = new List<LinePosition>();

}

[System.Serializable]
public class MapRoomData
{
    public float posX, posY; //保存坐标
    public int colum, line;  //保存行列

    public RoomDataSO roomData;
    public RoomState roomState;

    public List<Vector2Int> linkTo;


}

[System.Serializable]
public class LinePosition
{
    public SerializVector3 startPos, endPos;
}