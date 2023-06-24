using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    public Dictionary<int, Room> rooms;
    private Dictionary<int, Door> doors;

    public List<IntPair> locations = new List<IntPair>();
    public List<int> locationsT = new List<int>();
    //public bool tutorial;

    public void createDungeon()
    {

    }

    public void createDungeon(int numRooms, bool tutorial = true)
    {
        rooms = new Dictionary<int, Room>();
        doors = new Dictionary<int, Door>();
        int createdRooms = 0;
        int door = 0;
        List<int> returnedDoors = new List<int>();
        Queue<int> returnedDoorsQueue = new Queue<int>();
        if (tutorial)
        {
            door = createFirstRoom();
            door = createSecondRoom(door);
            returnedDoors = createThirdRoom(door);
            for (int i = 0; i < returnedDoors.Count; i++)
            {
                returnedDoorsQueue.Enqueue(returnedDoors[i]);
            }
        }
        else
        {
            returnedDoors = createNextRoom(door);
            for (int i = 0; i < returnedDoors.Count; i++)
            {
                returnedDoorsQueue.Enqueue(returnedDoors[i]);
            }
        }
        while ((returnedDoorsQueue.Count > 0) && ((createdRooms + returnedDoorsQueue.Count) < numRooms))
        {
            int fromDoor = returnedDoorsQueue.Dequeue();
            returnedDoors = createNextRoom(fromDoor);
            createdRooms++;
            for (int i = 0; i < returnedDoors.Count; i++)
            {
                returnedDoorsQueue.Enqueue(returnedDoors[i]);
            }
        }
        while (returnedDoorsQueue.Count > 1)
        {
            int fromDoor = returnedDoorsQueue.Dequeue();
            createNoDoorsRoom(fromDoor);
        }
        if (returnedDoorsQueue.Count == 1)
        {
            int fromDoor = returnedDoorsQueue.Dequeue();
            createBossRoom(fromDoor);
        }
        else
        {
            Debug.LogError("Could not create boss room. No doors left");
        }

        Debug.Log("Rooms: " + rooms.Count + " Doors:" + doors.Count);

    }

    public int createFirstRoom()
    {
        Room room1 = new Room(Singleton.Instance.currentRoomId);
        Singleton.Instance.currentRoomId++;

        Door newDoor = new Door(0, room1.RoomId);
        room1.addDoor(newDoor);
        doors.Add(newDoor.doorId, newDoor);

        Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
        room1.addEnemy(newEnemy1);
        Enemy newEnemy2 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
        room1.addEnemy(newEnemy2);
        Enemy newEnemy3 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
        room1.addEnemy(newEnemy3);

        room1.location = new IntPair(0, 0);
        locations.Add(room1.location);
        locationsT.Add((room1.location.x * 100) + room1.location.y);

        Singleton.Instance.addRoom(room1);

        rooms.Add(room1.RoomId, room1);
        return (newDoor.doorId);
    }

    public int createSecondRoom(int previousDoor)
    {
        //Create new Room
        Room room1 = new Room(Singleton.Instance.currentRoomId);
        Singleton.Instance.currentRoomId++;

        // Get Exisitng door
        Door ExistingDoor = doors[previousDoor];
        // Add new room to existing door
        ExistingDoor.destinationRoomId = room1.RoomId;

        //Add Door in reverse direction
        int onWall = (ExistingDoor.wall + 2) % 4;
        Door reverseDoor = new Door(onWall, room1.RoomId, ExistingDoor.roomId);
        doors.Add(reverseDoor.doorId, reverseDoor);
        room1.addDoor(reverseDoor);
        // New Door location
        int newDoorWall = newDoorLocation_1Door(onWall);

        // Add new Door
        Door otherDoor = new Door(newDoorWall, room1.RoomId);
        doors.Add(otherDoor.doorId, otherDoor);
        room1.addDoor(otherDoor);

        Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_PROJECTILE, 1, 1);
        room1.addEnemy(newEnemy1);
        Enemy newEnemy2 = new Enemy(2, Constants.ATTACK_TYPE_PROJECTILE, 1, 1);
        room1.addEnemy(newEnemy2);
        Enemy newEnemy3 = new Enemy(2, Constants.ATTACK_TYPE_PROJECTILE, 1, 1);
        room1.addEnemy(newEnemy3);

        Room existingRoom = rooms[ExistingDoor.roomId];

        room1.location = getLocation(ExistingDoor, existingRoom);

        locations.Add(room1.location);
        locationsT.Add((room1.location.x * 100) + room1.location.y);

        Singleton.Instance.addRoom(room1);
        rooms.Add(room1.RoomId, room1);
        return (otherDoor.doorId);
    }

    public List<int> createThirdRoom(int previousDoor)
    {
        List<int> retunedDoors = new List<int>();

        //Create new Room
        Room room1 = new Room(Singleton.Instance.currentRoomId);
        Singleton.Instance.currentRoomId++;

        // Get Exisitng door
        Door ExistingDoor = doors[previousDoor];
        // Add new room to existing door
        ExistingDoor.destinationRoomId = room1.RoomId;

        //Add Door in reverse direction
        int onWall = (ExistingDoor.wall + 2) % 4;
        Door reverseDoor = new Door(onWall, room1.RoomId, ExistingDoor.roomId);
        doors.Add(reverseDoor.doorId, reverseDoor);
        room1.addDoor(reverseDoor);

        Room existingRoom = rooms[ExistingDoor.roomId];
        room1.location = getLocation(ExistingDoor, existingRoom);
        locations.Add(room1.location);
        locationsT.Add((room1.location.x * 100) + room1.location.y);

        bool emptyLocation = false;
        while (!emptyLocation)
        {
            // New Door location
            int newDoorWall = newDoorLocation_1Door(onWall);

            IntPair newLocation = getNewLocation(newDoorWall, room1);
            // check new location is empty
            if (isnewLocationEmpty(newLocation))
            {
                // Add new Door
                emptyLocation = true;
                Door otherDoor = new Door(newDoorWall, room1.RoomId);
                doors.Add(otherDoor.doorId, otherDoor);
                room1.addDoor(otherDoor);
                retunedDoors.Add(otherDoor.doorId);
            }
        }

        Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_MAGIC, 1, 1);
        room1.addEnemy(newEnemy1);
        Enemy newEnemy2 = new Enemy(2, Constants.ATTACK_TYPE_MAGIC, 1, 1);
        room1.addEnemy(newEnemy2);
        Enemy newEnemy3 = new Enemy(2, Constants.ATTACK_TYPE_MAGIC, 1, 1);
        room1.addEnemy(newEnemy3);

        Singleton.Instance.addRoom(room1);
        rooms.Add(room1.RoomId, room1);
        return (retunedDoors);
    }

    public List<int> createNextRoom(int previousDoor)
    {
        List<int> retunedDoors = new List<int>();

        //Create new Room
        Room room1 = new Room(Singleton.Instance.currentRoomId);

        // Get Exisitng door
        Door ExistingDoor = doors[previousDoor];
        // Add new room to existing door
        ExistingDoor.destinationRoomId = room1.RoomId;

        //Add Door in reverse direction
        int onWall = (ExistingDoor.wall + 2) % 4;
        Door reverseDoor = new Door(onWall, room1.RoomId, ExistingDoor.roomId);
        doors.Add(reverseDoor.doorId, reverseDoor);
        room1.addDoor(reverseDoor);

        Room existingRoom = rooms[ExistingDoor.roomId];

        Debug.Log("From Room " + existingRoom.RoomId + " x:" + existingRoom.location.x + " y:" + existingRoom.location.y + "Door: " + ExistingDoor.wall);
        IntPair newLocation = null;

        newLocation = getLocation(ExistingDoor, existingRoom);
        if (newLocation == null)
        {
            return retunedDoors;
        }
        else
        {
            if (locationsT.Contains((newLocation.x * 100) + newLocation.y))
            {
                Debug.LogWarning("Drop duplicate location " + (newLocation.x * 100) + newLocation.y);
                return retunedDoors;
            }
        }
        room1.location = newLocation;
        locations.Add(room1.location);
        locationsT.Add((room1.location.x * 100) + room1.location.y);


        Singleton.Instance.currentRoomId++;

        int numAvailableLocations = availableLocations(room1);
        Debug.LogWarning("Available dor locations:" + numAvailableLocations);
        //place other doors
        float randDoor = Random.Range(0f, 10f);
        int numDoors = 1;
        if (randDoor > 8f)
        {
            numDoors = 3;
        }
        else if (randDoor > 4f)
        {
            numDoors = 2;
        }
        //No empty locations available
        if (numAvailableLocations == 0)
        {
            return retunedDoors;
        }
        //Can't have more doors than empty locations
        if (numDoors > numAvailableLocations)
        {
            numDoors = numAvailableLocations;
        }

        IntPair newLocationPair = null;
        int createdDoors = 0;
        int tryCount = 0;
        while ((createdDoors == 0) && (tryCount < 20))
        {
            switch (numDoors)
            {
                case 1:
                    int newDoorWall = newDoorLocation_1Door(ExistingDoor.wall);
                    newLocationPair = getNewLocation(newDoorWall, room1);
                    // check new location is empty
                    if (isnewLocationEmpty(newLocationPair))
                    {
                        Door otherDoor = new Door(newDoorWall, room1.RoomId);
                        doors.Add(otherDoor.doorId, otherDoor);
                        room1.addDoor(otherDoor);
                        retunedDoors.Add(otherDoor.doorId);
                        createdDoors++;
                    }
                    break;
                case 2:
                    int doorArrange = Random.Range(1, 4);
                    if (doorArrange == 1)
                    {
                        newDoorWall = (ExistingDoor.wall + 1) % 4;
                        newLocationPair = getNewLocation(newDoorWall, room1);
                        // check new location is empty
                        if (isnewLocationEmpty(newLocationPair))
                        {
                            Door nOtherDoor = new Door(newDoorWall, room1.RoomId);
                            doors.Add(nOtherDoor.doorId, nOtherDoor);
                            room1.addDoor(nOtherDoor);
                            retunedDoors.Add(nOtherDoor.doorId);
                            createdDoors++;
                        }

                        newDoorWall = (ExistingDoor.wall + 2) % 4;
                        newLocationPair = getNewLocation(newDoorWall, room1);
                        // check new location is empty
                        if (isnewLocationEmpty(newLocationPair))
                        {
                            Door nOtherDoor = new Door(newDoorWall, room1.RoomId);
                            doors.Add(nOtherDoor.doorId, nOtherDoor);
                            room1.addDoor(nOtherDoor);
                            retunedDoors.Add(nOtherDoor.doorId);
                            createdDoors++;
                        }
                    }
                    else if (doorArrange == 2)
                    {
                        newDoorWall = (ExistingDoor.wall + 1) % 4;
                        newLocationPair = getNewLocation(newDoorWall, room1);
                        // check new location is empty
                        if (isnewLocationEmpty(newLocationPair))
                        {
                            Door nOtherDoor = new Door(newDoorWall, room1.RoomId);
                            doors.Add(nOtherDoor.doorId, nOtherDoor);
                            room1.addDoor(nOtherDoor);
                            retunedDoors.Add(nOtherDoor.doorId);
                            createdDoors++;
                        }

                        newDoorWall = (ExistingDoor.wall + 3) % 4;
                        newLocationPair = getNewLocation(newDoorWall, room1);
                        // check new location is empty
                        if (isnewLocationEmpty(newLocationPair))
                        {                           
                            Door nOtherDoor = new Door(newDoorWall, room1.RoomId);
                            doors.Add(nOtherDoor.doorId, nOtherDoor);
                            room1.addDoor(nOtherDoor);
                            retunedDoors.Add(nOtherDoor.doorId);
                            createdDoors++;
                        }
                    }
                    else
                    {
                        newDoorWall = (ExistingDoor.wall + 2) % 4;

                        newLocationPair = getNewLocation(newDoorWall, room1);
                        // check new location is empty
                        if (isnewLocationEmpty(newLocationPair))
                        {
                            Door nOtherDoor = new Door(newDoorWall, room1.RoomId);
                            doors.Add(nOtherDoor.doorId, nOtherDoor);
                            room1.addDoor(nOtherDoor);
                            retunedDoors.Add(nOtherDoor.doorId);
                            createdDoors++;
                        }

                        newDoorWall = (ExistingDoor.wall + 3) % 4;
                        newLocationPair = getNewLocation(newDoorWall, room1);
                        // check new location is empty
                        if (isnewLocationEmpty(newLocationPair))
                        {
                            Door nOtherDoor = new Door(newDoorWall, room1.RoomId);
                            doors.Add(nOtherDoor.doorId, nOtherDoor);
                            room1.addDoor(nOtherDoor);
                            retunedDoors.Add(nOtherDoor.doorId);
                            createdDoors++;
                        }
                    }
                    break;
                case 3:
                    for (int i = 1; i < 4; i++)
                    {
                        newDoorWall = (ExistingDoor.wall + i) % 4;
                        newLocationPair = getNewLocation(newDoorWall, room1);
                        // check new location is empty
                        if (isnewLocationEmpty(newLocationPair))
                        {
                            Door nOtherDoor = new Door(newDoorWall, room1.RoomId);
                            doors.Add(nOtherDoor.doorId, nOtherDoor);
                            room1.addDoor(nOtherDoor);
                            retunedDoors.Add(nOtherDoor.doorId);
                            createdDoors++;
                        }
                    }
                    break;
            }
            tryCount++;
        }
        //room1.addDoor(ExistingDoor);
        int numEnemies = Random.Range(1, 6);
        for (int i = 0; i < numEnemies; i++)
        {
            int enemyType = Random.Range(0, 3);
            Enemy newEnemy = new Enemy(Singleton.Instance.currentRoomId, enemyType, 1, 1);
            room1.addEnemy(newEnemy);
        }




        rooms.Add(room1.RoomId, room1);

        //Debug.Log("Room " + room1.RoomId + " x:" + room1.location.x + " y:" + room1.location.y + "Door: " + ExistingDoor.wall);

        return (retunedDoors);

    }

    public void createNoDoorsRoom(int previousDoor)
    {
        //Create new Room
        Room room1 = new Room(Singleton.Instance.currentRoomId);
        Singleton.Instance.currentRoomId++;

        // Get Exisitng door
        Door ExistingDoor = doors[previousDoor];
        // Add new room to existing door
        ExistingDoor.destinationRoomId = room1.RoomId;

        //Add Door in reverse direction
        int onWall = (ExistingDoor.wall + 2) % 4;
        Door reverseDoor = new Door(onWall, room1.RoomId, ExistingDoor.roomId);
        doors.Add(reverseDoor.doorId, reverseDoor);
        room1.doors.Add(reverseDoor);

        Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
        room1.addEnemy(newEnemy1);
        Enemy newEnemy2 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
        room1.addEnemy(newEnemy2);
        Enemy newEnemy3 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
        room1.addEnemy(newEnemy3);

        int numEnemies = Random.Range(1, 6);
        for (int i = 0; i < numEnemies; i++)
        {
            int enemyType = Random.Range(0, 3);
            Enemy newEnemy = new Enemy(Singleton.Instance.currentRoomId, enemyType, 1, 1);
            room1.addEnemy(newEnemy);
        }

        Room existingRoom = rooms[ExistingDoor.roomId];
        room1.location = getLocation(ExistingDoor, existingRoom);

        locations.Add(room1.location);
        locationsT.Add((room1.location.x * 100) + room1.location.y);

        Singleton.Instance.addRoom(room1);
        rooms.Add(room1.RoomId, room1);

        Debug.Log("Room(e) " + room1.RoomId + " x:" + room1.location.x + " y:" + room1.location.y + "Door: " + ExistingDoor.wall);
    }

    public void createBossRoom(int previousDoor)
    {
        //Create new Room
        Room room1 = new Room(Singleton.Instance.currentRoomId);
        Singleton.Instance.currentRoomId++;

        // Get Exisitng door
        Door ExistingDoor = doors[previousDoor];
        // Add new room to existing door
        ExistingDoor.destinationRoomId = room1.RoomId;

        //Add Door in reverse direction
        int onWall = (ExistingDoor.wall + 2) % 4;
        Door reverseDoor = new Door(onWall, room1.RoomId, ExistingDoor.roomId);
        doors.Add(reverseDoor.doorId, reverseDoor);
        room1.doors.Add(reverseDoor);

        Enemy newEnemy1 = new Enemy(100, Constants.ATTACK_TYPE_MELEE, 1, 1, true);
        room1.addEnemy(newEnemy1);
        room1.bossRoom = true;
        Room existingRoom = rooms[ExistingDoor.roomId];
        room1.location = getLocation(ExistingDoor, existingRoom);

        locations.Add(room1.location);
        locationsT.Add((room1.location.x * 100) + room1.location.y);

        Singleton.Instance.addRoom(room1);
        rooms.Add(room1.RoomId, room1);
    }

    private int newDoorLocation_1Door(int existingWall)
    {
        int rand = Random.Range(1, 4);
        int newWall = (existingWall + rand) % 4;
        return newWall;
    }

    private IntPair getLocation(Door ExistingDoor, Room existingRoom)
    {
        IntPair newLocation = new IntPair(0, 0);
        switch (ExistingDoor.wall)
        {
            case 0:
                newLocation = new IntPair(existingRoom.location.x, existingRoom.location.y + 1);
                break;
            case 1:
                newLocation = new IntPair(existingRoom.location.x + 1, existingRoom.location.y);
                break;
            case 2:
                newLocation = new IntPair(existingRoom.location.x, existingRoom.location.y - 1);
                break;
            case 3:
                newLocation = new IntPair(existingRoom.location.x - 1, existingRoom.location.y);
                break;
        }

        return (newLocation);
    }

    private IntPair getNewLocation(int wall, Room existingRoom)
    {
        IntPair newLocation = new IntPair(0, 0);
        switch (wall)
        {
            case 0:
                newLocation = new IntPair(existingRoom.location.x, existingRoom.location.y + 1);
                break;
            case 1:
                newLocation = new IntPair(existingRoom.location.x + 1, existingRoom.location.y);
                break;
            case 2:
                newLocation = new IntPair(existingRoom.location.x, existingRoom.location.y - 1);
                break;
            case 3:
                newLocation = new IntPair(existingRoom.location.x - 1, existingRoom.location.y);
                break;
        }

        return (newLocation);
    }
    private bool isnewLocationEmpty(IntPair newLocation)
    {
        bool locationEmpty = false;
        int locationValue = (newLocation.x * 100) + newLocation.y;
        if (!locationsT.Contains(locationValue))
        {
            locationEmpty = true;
        }

        return locationEmpty;
    }
    private int availableLocations(Room room1)
    {
        int numLocations = 0;
        for (int i=0;i<4;i++)
        {
            // New Door location
            

            IntPair newLocation = getNewLocation(i, room1);
            // check new location is empty
            if (isnewLocationEmpty(newLocation))
            {
                numLocations++;
            }
        }
        return numLocations;
    }
}
