using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public void OnLoadRoomEvent(object data)
    {
        if (data is RoomDataSO)
        {
            var currentRoom = (RoomDataSO)data;
            Debug.Log(currentRoom.roomType);
        }
    }
}
