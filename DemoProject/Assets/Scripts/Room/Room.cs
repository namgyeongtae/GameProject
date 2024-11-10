using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        None,
        Battle,
        Puzzle,
        Save,
        Boss
    }

    protected Action EnterRoom;
    protected Action ExitRoom;

    protected RoomType _roomType;

    public virtual void ClearRoom() { }
}
