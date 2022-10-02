using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public int RoomId;
    public List<Enemy> enemies;
    public List<Door> doors;
    public bool bossRoom = false;


    public Room(int roomId)
    {

        this.RoomId = roomId;       
        enemies = new List<Enemy>();
        doors = new List<Door>();
    }

    public void addDoor(Door door)
    {
        doors.Add(door);
    }

    public void addEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

}
