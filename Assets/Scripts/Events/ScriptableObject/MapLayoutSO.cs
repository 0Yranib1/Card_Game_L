using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> mapRoomDataList= new List<MapRoomData>();

    public List<LinePosition> LinePositions = new List<LinePosition>();
}

[System.Serializable]
public class MapRoomData
{
    public float posX, posY;
    public int colum, line;
    public RoomDataSO roomData;
    public RoomState roomState;
}

[System.Serializable]
public class LinePosition
{
    public SerializeVector3 starPos, endPos;
}