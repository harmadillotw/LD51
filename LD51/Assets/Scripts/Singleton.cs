using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }

    public bool newGame = true;
    public bool paused = false;
    public bool transition = false;
    public bool startPause = false;

    public bool startGameOver = false;

    public bool roomTransition = false;

    public bool bossDead = false;
    public bool bossRoom = false;

    public int bossHealth = 0;

    public int maxRoomId = 0;
    public int maxDoorId = 0;

    public int fromRoomId = 0;
    public Door fromDoor;

    public int currentRoomId = 1;

    public int playerHealth = 10;
    public int playerMeleeDamage = 1;
    public int playerRangedDamage = 1;
    public int playerMagicDamage = 1;

    public int playerAttackType = Constants.ATTACK_TYPE_MELEE;

    public Dictionary<int, Room> rooms = new Dictionary<int, Room>();

    public Dictionary<int, IntPair> starts = new Dictionary<int, IntPair>();

    public int condition = 0;

    public int meleeMod = 0;
    public int rangedMod = 0;
    public int magicMod = 0;

    public int setCondition = 0;

    public float countdownTimer = 0f;

    public bool reverseControls = false;
    public bool attackLocked = false;

    public bool debugMode = false;

    public Dungeon dungeon;

    

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool isPaused()
    {
        if (paused || transition || startPause)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isTransition()
    {
        return transition;
    }

    public void addRoom(Room room)
    {
        rooms.Add(room.RoomId, room);
    }

    public void addStart(int num,  IntPair start)
    {
        starts.Add(num, start);
    }

}
