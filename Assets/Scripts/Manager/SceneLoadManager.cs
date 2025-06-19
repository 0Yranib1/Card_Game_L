using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;

    private Vector2Int currentRoomVector;

    [Header("广播")] 
    public ObjectEventSO afterRoomLoadedEvent;
    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            Room currentRoom = data as Room;
            var currentData = currentRoom.roomData;
            currentRoomVector = new(currentRoom.column, currentRoom.line);
            // Debug.Log(currentRoom.roomType);
            currentScene = currentData.sceneToLoad;
        }
        //加载房间
        await UnloadSceneTask();
        await LoadSceneTask();
        
        afterRoomLoadedEvent.RaisEvent(currentRoomVector,this);
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;
        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    private async Awaitable UnloadSceneTask()
    {
       await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    /// <summary>
    /// 监听返回房间的事件函数
    /// </summary>
    public async void LoadMap()
    {
        await UnloadSceneTask();
        currentScene = map;
        await LoadSceneTask();
    }
}
