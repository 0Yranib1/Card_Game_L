using UnityEngine;
using UnityEngine.AddressableAssets;


[CreateAssetMenu(fileName = "RomDataSO", menuName = "Map/RoomDataSO")]
public class RoomDataSO : ScriptableObject
{
    public Sprite roomIcon;
    public RoomType  roomType;
    public AssetReference sceneToLoad;
}
