using UnityEditor.Embree;
using UnityEngine;

public enum RoomType
{
    MinorEnemy,EliteEnemy,Shop,Treasure,RestRoom,Boss
}

public enum RoomState
{
    Locked,Visited,Attainable
}