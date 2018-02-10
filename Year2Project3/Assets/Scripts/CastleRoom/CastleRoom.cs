using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleRoom : MonoBehaviour 
{


    public enum RoomType
    {
        Knight,
        Ranger,
        Spearman,
        Gold
    }
    public RoomType roomType;

    public string roomName;
    public int roomLevel;
}
