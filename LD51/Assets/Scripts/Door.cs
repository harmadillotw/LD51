using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door 
{
    // wall 
    // 0 = N
    // 1 = E
    // 2 = S
    // 3 = W
    public int doorId;
    public int destinationRoomId;
    public int roomId;
    public int wall;
    public bool exit = false;

    public Door()
    {
        doorId = Singleton.Instance.maxDoorId++;
        wall = 0;
    }

    public Door(int wall)
    {
        doorId = Singleton.Instance.maxDoorId++;
        this.wall = wall;
    }
    public Door(int wall,  int room)
    {
        doorId = Singleton.Instance.maxDoorId++;
        this.roomId = room;
        this.wall = wall;
        exit = false;
    }
        
    public Door(int wall, int roomId, int destinationRoomId)
    {
        doorId = Singleton.Instance.maxDoorId++;
        
        this.destinationRoomId = destinationRoomId;
        this.roomId = roomId;
        this.wall = wall;
        exit = false;

    }

    public Door(int wall, int destination, int myRoom, bool exit)
    {
        doorId = Singleton.Instance.maxDoorId++;
        destinationRoomId = destination;
        this.roomId = myRoom;
        this.wall = wall;
        this.exit = exit;

    }

}
