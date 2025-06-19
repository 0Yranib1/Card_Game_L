using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")] 
    public MapLayoutSO mapLayout;


    /// <summary>
    /// 更新房间事件监听函数
    /// </summary>
    /// <param name="roomVector"></param>
    public void UpdateMapLayoutData(object value)
    {
        var roomVector = (Vector2Int)value;
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.colum == roomVector.x && r.line == roomVector.y);
        currentRoom.roomState = RoomState.Visited;
        //更新同列房间数据
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.colum == currentRoom.colum);

        foreach (var room in sameColumnRooms)
        {
            if(room.line!=roomVector.y)
            {
                room.roomState = RoomState.Locked;
            }
        }

        foreach (var link in currentRoom.linkTo)
        {
            var linkRoom = mapLayout.mapRoomDataList.Find(r => r.colum == link.x && r.line == link.y);
            linkRoom.roomState = RoomState.Attainable;
        }
    }
}
