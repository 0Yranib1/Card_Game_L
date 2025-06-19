using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置")]
    public MapConfigSO mapConfig;
    [Header("地图布局")]
    public MapLayoutSO mapLayout;
    [Header("预制体")]
    public Room roomPrefab;

    public LineRenderer linePrefab;

    private float screenHeight;
    private float screenWidth;
    private float columnWidth;
    private Vector3 generatePoint;
    public float border;

    private List<Room> rooms= new List<Room>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    
    public List<RoomDataSO> roomDataList= new List<RoomDataSO>();
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new Dictionary<RoomType, RoomDataSO>();
    private void Awake()
    {
        screenHeight=  Camera.main.orthographicSize  * 2;
        screenWidth = screenHeight*  Camera.main.aspect;
        columnWidth = screenWidth / (mapConfig.roomBluePrints.Count+1);

        foreach (var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }
    }

    private void Start()
    {
        // CreateMap();
    }

    private void OnEnable()
    {
        if(mapLayout.mapRoomDataList.Count>0)
            LoadMap();
        else
        {
            CreateMap();
        }
    }

    public void CreateMap()
    {
        
        //创建前一列房间列表
        List<Room> previousColumnRooms = new List<Room>();
        for (int column = 0; column < mapConfig.roomBluePrints.Count; column++)
        {
            var blueprint=mapConfig.roomBluePrints[column];
            var amount=Random.Range(blueprint.min, blueprint.max+1);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);
            
            generatePoint=new Vector3(-screenWidth/2+border+columnWidth*column, startHeight, 0);
            
            var newPosition=generatePoint;

            //当前列房间列表
            List<Room> currentColumnRooms = new List<Room>();
            
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
                //生成房间
                var room=Instantiate(roomPrefab, newPosition,Quaternion.identity,transform);
                RoomType newType= GetRandomRoomType(mapConfig.roomBluePrints[column].roomType) ;
                room.SetupRoom(column, i, GetRoomData(newType));
                
                
                rooms.Add(room);
                currentColumnRooms.Add(room);
            }
            //判断当前列是否为第一列 不是则连接到上一列
            if (previousColumnRooms.Count > 0)
            {
                //创建两个列表的房间连线
                CreateConnections(previousColumnRooms, currentColumnRooms);
            }
            previousColumnRooms = currentColumnRooms;


        }
        
        SaveMap();
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

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        rooms.Clear();
        lines.Clear();
        CreateMap();
    }

    void CreateConnections(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms = new HashSet<Room>();
        foreach (var room in column1)
        {
            var targetRoom= ConnectToRandomRoom(room, column2);
            connectedColumn2Rooms.Add(targetRoom);
        }
        foreach (var room in column2)
        {
            if (!connectedColumn2Rooms.Contains(room))
            {
                ConnectToRandomRoom(room, column1);
            }
        }
    }

    private Room ConnectToRandomRoom(Room room, List<Room> column2)
    {
        Room targetRoom;
        targetRoom=column2[Random.Range(0, column2.Count)];
        
        //创建连线
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);
        
        return targetRoom;
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');
        string randomOption= options[Random.Range(0, options.Length)];
        return (RoomType)Enum.Parse(typeof(RoomType), randomOption);
    }

    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new List<MapRoomData>();
        //添加房间
        for (int i = 0; i < rooms.Count; i++)
        {
            var romm = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                colum = rooms[i].column,
                line = rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState
            };
            mapLayout.mapRoomDataList.Add(romm);
        }

        mapLayout.LinePositions = new List<LinePosition>();
        //添加连线
        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition()
            {
                starPos  =new SerializeVector3(lines[i].GetPosition(0)),
                endPos = new SerializeVector3(lines[i].GetPosition(1))
            };
            mapLayout.LinePositions.Add(line);
        }
    }
    
    private void LoadMap()
    {
        //读取房间数据
        for (int i=0; i<mapLayout.mapRoomDataList.Count; i++)
        {
            var newPos = new Vector3(mapLayout.mapRoomDataList[i].posX, mapLayout.mapRoomDataList[i].posY, 0);
            var newroom = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
            newroom.roomState= mapLayout.mapRoomDataList[i].roomState;
            newroom.SetupRoom(mapLayout.mapRoomDataList[i].colum, mapLayout.mapRoomDataList[i].line, mapLayout.mapRoomDataList[i].roomData);
            rooms.Add(newroom);
        }
        //读取连线
        for (int i = 0; i < mapLayout.LinePositions.Count; i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.LinePositions[i].starPos.ToVector3());
            line.SetPosition(1, mapLayout.LinePositions[i].endPos.ToVector3());
            lines.Add(line);
        }
    }
}
