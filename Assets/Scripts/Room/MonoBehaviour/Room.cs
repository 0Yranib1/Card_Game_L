using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;
    
    public List<Vector2Int> linkTo= new List<Vector2Int>(); 
    [Header("广播")]  
    public ObjectEventSO loadRoomEvent;
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if(roomState==RoomState.Attainable)
        {
            loadRoomEvent.RaisEvent(this, this);
        }
    }

    /// <summary>
    /// 创建房间时设置房间信息
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    /// <param name="roomData"></param>
    public void SetupRoom(int column, int line, RoomDataSO roomData)
    {
        this.column=column;
        this.line=line;
        this.roomData = roomData;
        spriteRenderer.sprite = roomData.roomIcon;
        spriteRenderer.color = roomState switch
        {
            RoomState.Locked=>Color.gray,
            RoomState.Visited=>new Color(0.5f,0.8f,0.5f,0.5f),
            RoomState.Attainable=>Color.white,
            _=>throw new SystemException("房间颜色相关错误")
        };
        
    }
}
