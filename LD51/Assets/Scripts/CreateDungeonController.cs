using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateDungeonController : MonoBehaviour
{
    public GameObject roomGO;
    public GameObject doorGO;
    public GameObject bossGO;
    private GameObject dungeonParent;
    private Dungeon dungeon;
    private int x = 0;
    private int y = 0;

    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        y = -3;
        Singleton.Instance.fromDoor = null;
        Singleton.Instance.currentRoomId = 1;
        Singleton.Instance.rooms = new Dictionary<int, Room>();
        dungeon = new Dungeon();
        dungeon.createDungeon(10, true);
        
        //drawDungeon(dungeon);
        Singleton.Instance.currentRoomId = 1;
        Singleton.Instance.dungeon = dungeon; 
        GoNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void drawDungeon(Dungeon d)
    {
        dungeonParent = new GameObject("DungeonParent");
        int minX = 0;
        int maxX = 0;
        int minY = 0;
        int maxY = 0;
        foreach ( IntPair pair in d.locations)
        {
            if (pair.x < minX)
            {
                minX = pair.x;
            }

            if (pair.x > maxX)
            {
                maxX = pair.x;
            }

            if (pair.y < minY)
            {
                minY = pair.y;
            }

            if (pair.y > maxY)
            {
                maxY = pair.y;
            }
        }

        float yBase = 0f;
        float xBase = 0f;
        if (minY < 0)
        {
            yBase = (((float)maxY + (float)minY) / 2) - 1;
            Debug.LogWarning("yBase " + yBase + " ((" + maxY + " + " + minY + ")/2) - 1");
        }
        else
        {
            yBase = (((float)maxY - (float)minY) / 2) - 1;
            Debug.LogWarning("yBase " + yBase + " ((" + maxY + " - " + minY + ")/2( - 1");
        }
        if (minX < 0)
        {
            xBase = (((float)maxX + (float)minX) / 2);
            Debug.LogWarning("xBase " + xBase + " (" + maxX + " + " + minX + ")/2");
        }
        else
        {
            xBase = (((float)maxX - (float)minX) / 2);
            Debug.LogWarning("xBase " + xBase + " (" + maxX + " - " + minX + ")/2");
        }
        
        
        foreach (var item in d.rooms)
        {
            
            Room room = item.Value;

            Debug.Log("Room:" + room.RoomId + ":" + room.location.x + "," + room.location.y + ":" + room.doors.Count);
            foreach(Door dr in room.doors)
            {
                Debug.Log("DoorWall:" + dr.wall);
            }


            int roomId = item.Key;
            //if (room.location != null)
            //{
            //    Debug.Log("Room " + roomId + " x:" + room.location.x + " y:" + room.location.y + "Door: " + ro);
            //}
            //else
            //{
            //    Debug.Log("Room " + roomId + " has no location");
            //}
            float nx = -xBase + room.location.x;
            float ny = -yBase + room.location.y;
            GameObject rGo = Instantiate(roomGO, new Vector3(nx, ny, 0), Quaternion.identity);
            rGo.transform.parent = dungeonParent.transform;
            rGo.GetComponentInChildren<TextMeshProUGUI>().text = "" + roomId;
            GameObject hGo = null;
            foreach(Door dor in room.doors)
            {
                switch (dor.wall)
                {
                    case 0:
                        hGo = Instantiate(doorGO, new Vector3(nx, ny + 0.3f, 0), Quaternion.identity);
                        hGo.transform.parent = rGo.transform;
                        break;
                    case 1:
                        hGo = Instantiate(doorGO, new Vector3(nx + 0.3f, ny, 0), Quaternion.identity);
                        hGo.transform.parent = rGo.transform;
                        break;
                    case 2:
                        hGo = Instantiate(doorGO, new Vector3(nx, ny - 0.3f, 0), Quaternion.identity);
                        hGo.transform.parent = rGo.transform;
                        break;
                    case 3:
                        hGo = Instantiate(doorGO, new Vector3(nx - 0.3f, ny, 0), Quaternion.identity);
                        hGo.transform.parent = rGo.transform;
                        break;
                    default:
                        hGo = Instantiate(doorGO, new Vector3(nx, ny, 0), Quaternion.identity);
                        hGo.transform.parent = rGo.transform;
                        break;
                }
            }
            if (room.bossRoom)
            {
                GameObject bGo = Instantiate(bossGO, new Vector3(nx, ny, 0), Quaternion.identity);
                bGo.transform.parent = dungeonParent.transform;

            }
        }

        
    }

    public void redraw()
    {
        Destroy(dungeonParent);
        Singleton.Instance.currentRoomId = 1;
        Singleton.Instance.rooms = new Dictionary<int, Room>();

        dungeon = new Dungeon();
        dungeon.createDungeon(10, true);
        drawDungeon(dungeon);
        Singleton.Instance.currentRoomId = 1;
        Singleton.Instance.dungeon = dungeon;
    }

    public void GoNewGame()
    {
        Singleton.Instance.newGame = true;
        Singleton.Instance.startGameOver = false;
        Singleton.Instance.playerHealth = 10;
        Singleton.Instance.playerMeleeDamage = 1;
        Singleton.Instance.playerRangedDamage = 1;
        Singleton.Instance.playerMagicDamage = 1;
        Singleton.Instance.maxDoorId = 0;
        Singleton.Instance.maxRoomId = 0;

        Singleton.Instance.currentRoomId = 1;
        //Singleton.Instance.newGame = false;
        Singleton.Instance.countdownTimer = 0;
        Singleton.Instance.setCondition = 0;
        Singleton.Instance.rooms = new Dictionary<int, Room>();
        Singleton.Instance.playerAttackType = Constants.ATTACK_TYPE_MELEE;
        Singleton.Instance.reverseControls = false;
        Singleton.Instance.attackLocked = false;
        Singleton.Instance.bossDead = false;


        SceneManager.LoadScene("MainGameScene");
    }
}
