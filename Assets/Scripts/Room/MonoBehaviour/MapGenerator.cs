using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public MapConfigSO mapConfig;
    public Room roomPrefab;

    private float screenHeight;
    private float screenWidth;
    private float columnWidth;
    private Vector3 generatePoint;
    public float border;

    private List<Room> rooms= new List<Room>();
    
    private void Awake()
    {
        screenHeight=  Camera.main.orthographicSize  * 2;
        screenWidth = screenHeight*  Camera.main.aspect;
        columnWidth = screenWidth / (mapConfig.roomBluePrints.Count+1);
    }

    private void Start()
    {
        CreateMap();
    }

    public void CreateMap()
    {
        for (int column = 0; column < mapConfig.roomBluePrints.Count; column++)
        {
            var blueprint=mapConfig.roomBluePrints[column];
            var amount=Random.Range(blueprint.min, blueprint.max+1);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);
            
            generatePoint=new Vector3(-screenWidth/2+border+columnWidth*column, startHeight, 0);
            
            var newPosition=generatePoint;


            
            var roomGapY=screenHeight/(amount+1);
            for (int i = 0; i < amount; i++)
            {
                
                //判断最后一列
                if (column == mapConfig.roomBluePrints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 2;
                }else if (column != 0)
                {
                    newPosition.x = generatePoint.x+ Random.Range(-border/2, border/2);
                }

                
                newPosition.y=startHeight-roomGapY*i;
                var room=Instantiate(roomPrefab, newPosition,Quaternion.identity,transform);
                
                rooms.Add(room);
            }
            
        }
    }

    [ContextMenu("重新生成地图")]
    /// <summary>
    /// 测试 重新生成地图
    /// </summary>
    public void ReGenerateRoom()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }
        rooms.Clear();
        CreateMap();
    }
}
