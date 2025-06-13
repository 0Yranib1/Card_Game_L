using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        SetupRoom(0,0,roomData);
    }

    private void OnMouseDown()
    {
        Debug.Log("点击房间"+roomData.roomType);
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
    }
}
