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

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    public string roomName;
    public int roomLevel;
    public int buildCost;
    public Stat upgradeCost;

    public virtual void Update()
    {

    }

    public virtual void UseRoom()
    {

    }

    public void Upgrade()
    {

    }
}
