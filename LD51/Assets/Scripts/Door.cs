using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door 
{
    public int doorId;
    public int destinationRoom;
    public int myRoom;
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

    public Door(int wall, int destination, int myRoom)
    {
        doorId = Singleton.Instance.maxDoorId++;
        destinationRoom = destination;
        this.myRoom = myRoom;
        this.wall = wall;
        exit = false;

    }

    public Door(int wall, int destination, int myRoom, bool exit)
    {
        doorId = Singleton.Instance.maxDoorId++;
        destinationRoom = destination;
        this.myRoom = myRoom;
        this.wall = wall;
        this.exit = exit;

    }



}
