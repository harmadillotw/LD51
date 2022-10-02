using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{
    public GameObject gameController;

    public List<GameObject> floorTilePrefab;
    public List<GameObject> floorTile2Prefab;
    public GameObject EnemyPrefab;
    public GameObject DoorHorPrefab;
    public GameObject DoorVertPrefab;
    public GameObject StaticEnemyPrefab;

    public GameObject BossEnemyPrefab;

    public int room;

    private Text StartCountText;
    private int columnCount = 8;
    private int rowCount = 5;

    private int enemyCount = 3;
    private List<int> enemyStartPos = new List<int>();

    private float startTimer = 0f;

    private Room previousRoom;

    private bool bossRoom = false;

    //private Dictionary<int,IntPair> starts = new Dictionary<int, IntPair>();
    // Start is called before the first frame update
    private void Awake()
    {
        gameController.GetComponent<GameController>().spawnEnemiesEvent += processSpawnEnemiesEvent;
    }
    void Start()
    {

        if (Singleton.Instance.newGame )
        {
            Singleton.Instance.startGameOver = false;
            Singleton.Instance.playerHealth = 10;
            Singleton.Instance.playerMeleeDamage = 1;
            Singleton.Instance.playerRangedDamage = 1;
            Singleton.Instance.playerMagicDamage = 1;
            Singleton.Instance.maxDoorId = 0;
            Singleton.Instance.maxRoomId = 0;

            Singleton.Instance.currentRoomId = 1;
            Singleton.Instance.newGame = false;
            Singleton.Instance.countdownTimer = 0;
            Singleton.Instance.setCondition = 0;
            Singleton.Instance.playerAttackType = Constants.ATTACK_TYPE_MELEE;
            Singleton.Instance.reverseControls = false;
            Singleton.Instance.attackLocked = false;
            Singleton.Instance.bossDead = false;

        }
        else
        {
            if (Singleton.Instance.rooms.ContainsKey(Singleton.Instance.fromRoomId))
            {
                previousRoom = Singleton.Instance.rooms[Singleton.Instance.fromRoomId];
            }
            
        }

        Singleton.Instance.startPause = true;
        int x = -14;
        int y = 10;

        StartCountText = GameObject.Find("StartCountText").GetComponent<Text>();
        StartCountText.gameObject.SetActive(true);
        StartCountText.text = "3";
        // Create singleton values. This will be in splash screen but here for testing
        /****/
        if (Singleton.Instance.starts.Count == 0)
        {
            Singleton.Instance.starts.Add(0, new IntPair(-14, 10));
            Singleton.Instance.starts.Add(1, new IntPair(-10, 10));
            Singleton.Instance.starts.Add(2, new IntPair(-6, 10));
            Singleton.Instance.starts.Add(3, new IntPair(-2, 10));
            Singleton.Instance.starts.Add(4, new IntPair(2, 10));
            Singleton.Instance.starts.Add(5, new IntPair(6, 10));
            Singleton.Instance.starts.Add(6, new IntPair(10, 10));
            Singleton.Instance.starts.Add(7, new IntPair(14, 10));

            Singleton.Instance.starts.Add(8, new IntPair(-14, 6));
            Singleton.Instance.starts.Add(9, new IntPair(14, 6));
            Singleton.Instance.starts.Add(10, new IntPair(-14, 2));
            Singleton.Instance.starts.Add(11, new IntPair(14, 2));
            Singleton.Instance.starts.Add(12, new IntPair(-14, -2));
            Singleton.Instance.starts.Add(13, new IntPair(14, -2));

            Singleton.Instance.starts.Add(14, new IntPair(-14, -6));
            Singleton.Instance.starts.Add(15, new IntPair(-10, -6));
            Singleton.Instance.starts.Add(16, new IntPair(-6, -6));
            Singleton.Instance.starts.Add(17, new IntPair(-2, -6));
            Singleton.Instance.starts.Add(18, new IntPair(2, -6));
            Singleton.Instance.starts.Add(19, new IntPair(6, -6));
            Singleton.Instance.starts.Add(20, new IntPair(10, -6));
            Singleton.Instance.starts.Add(21, new IntPair(14, -6));
        }

        if (Singleton.Instance.rooms.ContainsKey(Singleton.Instance.currentRoomId))
        {
            // got a room, we can skip creation
        }
        //Attempt at boss room
        //else if (Singleton.Instance.currentRoomId == 1)
        //{
        //    Room room1 = new Room(Singleton.Instance.currentRoomId);
        //    Door newDoor = new Door(0, Singleton.Instance.currentRoomId + 1, Singleton.Instance.currentRoomId, true);
        //    room1.addDoor(newDoor);
        //    Enemy newEnemy1 = new Enemy(1, Constants.ATTACK_TYPE_MELEE, 1, 1, true);
        //    room1.addEnemy(newEnemy1);
        //    room1.bossRoom = true;
        //    Singleton.Instance.addRoom(room1);
        //}
        //Create first room
        else if (Singleton.Instance.currentRoomId == 1)
        {
            Room room1 = new Room(Singleton.Instance.currentRoomId);

            Door newDoor = new Door(0, Singleton.Instance.currentRoomId + 1, Singleton.Instance.currentRoomId);
            room1.addDoor(newDoor);

            Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
            room1.addEnemy(newEnemy1);
            Enemy newEnemy2 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
            room1.addEnemy(newEnemy2);
            Enemy newEnemy3 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
            room1.addEnemy(newEnemy3);

            Singleton.Instance.addRoom(room1);


        }
        else if (Singleton.Instance.currentRoomId == 2)
        {
            //Room previousRoom = null;

            //if (Singleton.Instance.rooms.ContainsKey(Singleton.Instance.fromRoomId))
            //{
            //     previousRoom = (Singleton.Instance.rooms[Singleton.Instance.fromRoomId]);

            //}
            Room room1 = new Room(Singleton.Instance.currentRoomId);

            Door newDoor = new Door(0, Singleton.Instance.currentRoomId +1, Singleton.Instance.currentRoomId);
            room1.addDoor(newDoor);

            Door otherDoor = null;
            if (previousRoom != null)
            {
                if (Singleton.Instance.fromDoor != null)
                {
                    int doorSide = (Singleton.Instance.fromDoor.wall + 2) % 4;
                    otherDoor = new Door(doorSide, Singleton.Instance.fromDoor.myRoom, Singleton.Instance.currentRoomId);
                }
            }
            if (otherDoor != null)
            {
                room1.addDoor(otherDoor);
            }
            Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_PROJECTILE, 1, 1);
            room1.addEnemy(newEnemy1);
            Enemy newEnemy2 = new Enemy(2, Constants.ATTACK_TYPE_PROJECTILE, 1, 1);
            room1.addEnemy(newEnemy2);
            Enemy newEnemy3 = new Enemy(2, Constants.ATTACK_TYPE_PROJECTILE, 1, 1);
            room1.addEnemy(newEnemy3);

            Singleton.Instance.addRoom(room1);
            SavedSettings.currentLevel++;
            //Singleton.Instance.currentRoomId = room1.RoomId;
        }
        else if (Singleton.Instance.currentRoomId == 3)
        {
            Room room1 = new Room(Singleton.Instance.currentRoomId);

            Door newDoor = new Door(0, Singleton.Instance.currentRoomId + 1, Singleton.Instance.currentRoomId);
            room1.addDoor(newDoor);


            Door otherDoor = null;
            if (previousRoom != null)
            {
                if (Singleton.Instance.fromDoor != null)
                {
                    int doorSide = (Singleton.Instance.fromDoor.wall + 2) % 4;
                    otherDoor = new Door(doorSide, Singleton.Instance.fromDoor.myRoom, Singleton.Instance.currentRoomId);
                }
            }
            if (otherDoor != null)
            {
                room1.addDoor(otherDoor);
            }

            Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_MAGIC, 1, 1);
            room1.addEnemy(newEnemy1);
            Enemy newEnemy2 = new Enemy(2, Constants.ATTACK_TYPE_MAGIC, 1, 1);
            room1.addEnemy(newEnemy2);
            Enemy newEnemy3 = new Enemy(2, Constants.ATTACK_TYPE_MAGIC, 1, 1);
            room1.addEnemy(newEnemy3);

            Singleton.Instance.addRoom(room1);
            
            //Singleton.Instance.currentRoomId = room1.RoomId;
        }
        //else if (Singleton.Instance.currentRoomId == 4)
        //{
        //    Room room1 = new Room(Singleton.Instance.currentRoomId);
        //    Door newDoor = new Door(0, Singleton.Instance.currentRoomId + 1, Singleton.Instance.currentRoomId, true);
        //    room1.addDoor(newDoor);
        //    Enemy newEnemy1 = new Enemy(1, Constants.ATTACK_TYPE_MELEE, 1, 1, true);
        //    room1.addEnemy(newEnemy1);
        //    room1.bossRoom = true;
        //    Singleton.Instance.addRoom(room1);
        //}
        else 
        {
            int isBossRoom = 0;
            if (Singleton.Instance.currentRoomId > 10)
            {
                isBossRoom = Random.Range(0, 2);
            }
            if (isBossRoom > 0)
            {
                Room room1 = new Room(Singleton.Instance.currentRoomId);
                Door newDoor = new Door(0, Singleton.Instance.currentRoomId + 1, Singleton.Instance.currentRoomId, true);
                room1.addDoor(newDoor);
                Enemy newEnemy1 = new Enemy(100, Constants.ATTACK_TYPE_MELEE, 1, 1, true);
                room1.addEnemy(newEnemy1);
                room1.bossRoom = true;
                Singleton.Instance.addRoom(room1);
            }
            else
            {
                Room room1 = new Room(Singleton.Instance.currentRoomId);
                Door otherDoor = null;
                if (previousRoom != null)
                {
                    if (Singleton.Instance.fromDoor != null)
                    {
                        int doorSide = (Singleton.Instance.fromDoor.wall + 2) % 4;
                        otherDoor = new Door(doorSide, Singleton.Instance.fromDoor.myRoom, Singleton.Instance.currentRoomId);
                    }
                }
                if (otherDoor != null)
                {
                    room1.addDoor(otherDoor);
                }

                for (int i = 0; i < 4; i++)
                {
                    int isDoor = Random.Range(0, 4);

                    if (isDoor == 0)
                    {
                        bool addDoor = true;
                        foreach (Door d in room1.doors)
                        {
                            if (d.wall == i)
                            {
                                addDoor = false;
                            }
                        }
                        if (addDoor)
                        {
                            Door newDoor = new Door(i, Singleton.Instance.currentRoomId + 1, Singleton.Instance.currentRoomId);
                            room1.addDoor(newDoor);
                        }
                    }
                }
                if (room1.doors.Count == 1)
                {
                    int doorSide = (otherDoor.wall + 2) % 4;
                    Door newDoor = new Door(doorSide, Singleton.Instance.currentRoomId + 1, Singleton.Instance.currentRoomId);
                    room1.addDoor(newDoor);
                }

                int numEnemies = Random.Range(1, 6);
                for (int i = 0; i < numEnemies; i++)
                {
                    int enemyType = Random.Range(0, 3);
                    Enemy newEnemy = new Enemy(Singleton.Instance.currentRoomId, enemyType, 1, 1);
                    room1.addEnemy(newEnemy);
                }
                Singleton.Instance.addRoom(room1);


                //Singleton.Instance.currentRoomId = room1.RoomId;
            }
        }

        /*****/

        room = Singleton.Instance.currentRoomId;
        // Create Floor
        int roomtype = Random.Range(0, 2);
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                //GameObject r = Instantiate(floorTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                
                if (roomtype == 0)
                {
                    int prefabNum = Random.Range(0, floorTilePrefab.Count);
                    GameObject r = Instantiate(floorTilePrefab[prefabNum]);
                    r.transform.SetParent(transform);
                    r.transform.localPosition = new Vector3(x, y, 0);
                    x += 4;
                }
                else
                {
                    int prefabNum = Random.Range(0, floorTile2Prefab.Count);
                    GameObject r = Instantiate(floorTile2Prefab[prefabNum]);
                    r.transform.SetParent(transform);
                    r.transform.localPosition = new Vector3(x, y, 0);
                    x += 4;
                }
                
            }
            x = -14;
            y -= 4;
        }
        int ec = 0;

        //Create Doors
        Room newRoom = Singleton.Instance.rooms[Singleton.Instance.currentRoomId];
        if (newRoom.bossRoom)
        {
            Singleton.Instance.bossRoom = true;
        }
        else
        {
            Singleton.Instance.bossRoom = false;
        }
        foreach( Door door in newRoom.doors)
        {
            if (newRoom.bossRoom)
            {
                bool alldead = true;
                foreach ( Enemy enemy in newRoom.enemies)
                {
                    if (enemy.health > 0)
                    {
                        alldead = false;
                    }
                }
                if (!alldead)
                {
                    continue;
                }
            }
            switch (door.wall)
            {
                case 0:
                    GameObject doorGO = Instantiate(DoorHorPrefab);
                    doorGO.transform.SetParent(transform);
                    doorGO.transform.localPosition = new Vector3(0, 11.5f, 0);
                    
                    doorGO.GetComponent<DoorController>().door = door;
                    break;
                case 1:
                    GameObject doorGO2 = Instantiate(DoorVertPrefab);
                    doorGO2.transform.SetParent(transform);
                    doorGO2.transform.localPosition = new Vector3(15.5f, 0, 0);
                    doorGO2.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 270));
                    doorGO2.GetComponent<DoorController>().door = door;
                    break;
                case 2:
                    GameObject doorGO3 = Instantiate(DoorHorPrefab);
                    doorGO3.transform.SetParent(transform);
                    doorGO3.transform.localPosition = new Vector3(0, -7.5f, 0);
                    doorGO3.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 180));
                    doorGO3.GetComponent<DoorController>().door = door;
                    break;
                case 3:
                    GameObject doorGO4 = Instantiate(DoorVertPrefab);
                    doorGO4.transform.SetParent(transform);
                    doorGO4.transform.localPosition = new Vector3(-15.5f, 0, 0);
                    
                    doorGO4.GetComponent<DoorController>().door = door;
                    break;
            }
            
        }
        //Create Enemies
        //enemyCount = 0;
        foreach (Enemy enemy in newRoom.enemies)
        {
            if (enemy.health > 0)
            {
                if (enemy.bossEnemy)
                {
                    int startLocation = Random.Range(0, 22);
                    if (enemyStartPos.Contains(startLocation))
                    {
                        startLocation = Random.Range(0, 22);
                    }
                    if (!enemyStartPos.Contains(startLocation))
                    {
                        enemyStartPos.Add(startLocation);

                        GameObject enemyGO = Instantiate(BossEnemyPrefab);
                        enemyGO.transform.SetParent(transform);
                        enemyGO.transform.localPosition = new Vector3(Singleton.Instance.starts[startLocation].x, Singleton.Instance.starts[startLocation].y, 0);

                        enemyGO.GetComponent<EnemyController>().populateEnemy(enemy);
                        ec++;

                    }
                }
                else
                {


                    int startLocation = Random.Range(0, 22);
                    if (enemyStartPos.Contains(startLocation))
                    {
                        startLocation = Random.Range(0, 22);
                    }
                    if (!enemyStartPos.Contains(startLocation))
                    {
                        enemyStartPos.Add(startLocation);

                        GameObject enemyGO = Instantiate(EnemyPrefab);
                        enemyGO.transform.SetParent(transform);
                        enemyGO.transform.localPosition = new Vector3(Singleton.Instance.starts[startLocation].x, Singleton.Instance.starts[startLocation].y, 0);

                        enemyGO.GetComponent<EnemyController>().populateEnemy(enemy);
                        ec++;

                    }
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        startTimer += Time.deltaTime;
        StartCountText.text = "" + (int)(2f-startTimer);
        if (startTimer > 2f)
        {
            Singleton.Instance.startPause = false;
            StartCountText.gameObject.SetActive(false);
        }
    }

    private void processSpawnEnemiesEvent(object sender, System.EventArgs e)
    {
        Room room1 = null;
        if (Singleton.Instance.rooms.ContainsKey(Singleton.Instance.currentRoomId))
        {
            room1 = Singleton.Instance.rooms[Singleton.Instance.currentRoomId];
        }
        else
        {
            return;
        }
        if (Singleton.Instance.currentRoomId == 1)
        {
            Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_MELEE, 1, 1);
            room1.addEnemy(newEnemy1);
            spawnEnemy(newEnemy1);
        }
        else if (Singleton.Instance.currentRoomId == 2)
        {
            Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_PROJECTILE, 1, 1);
            room1.addEnemy(newEnemy1);
            spawnEnemy(newEnemy1);
            Enemy newEnemy2 = new Enemy(2, Constants.ATTACK_TYPE_PROJECTILE, 1, 1);
            room1.addEnemy(newEnemy2);
            spawnEnemy(newEnemy2);
        }
        else if (Singleton.Instance.currentRoomId == 3)
        {
            Enemy newEnemy1 = new Enemy(2, Constants.ATTACK_TYPE_MAGIC, 1, 1);
            room1.addEnemy(newEnemy1);
            spawnEnemy(newEnemy1);
            Enemy newEnemy2 = new Enemy(2, Constants.ATTACK_TYPE_MAGIC, 1, 1);
            room1.addEnemy(newEnemy2);
            spawnEnemy(newEnemy2);
            Enemy newEnemy3 = new Enemy(2, Constants.ATTACK_TYPE_MAGIC, 1, 1);
            room1.addEnemy(newEnemy3);
            spawnEnemy(newEnemy3);
        }
        else
        {
            int numEnemies = Random.Range(1, 6);
            for (int i = 0; i < numEnemies; i++)
            {
                int enemyType = Random.Range(0, 3);
                Enemy newEnemy = new Enemy(Singleton.Instance.currentRoomId, enemyType, 1, 1);
                room1.addEnemy(newEnemy);
                spawnEnemy(newEnemy);
            }
        }
    }

    private void spawnEnemy(Enemy enemy)
    {
        int startLocation = Random.Range(0, 22);
        if (!enemyStartPos.Contains(startLocation))
        {
            enemyStartPos.Add(startLocation);

            GameObject enemyGO = Instantiate(EnemyPrefab);
            enemyGO.transform.SetParent(transform);
            enemyGO.transform.localPosition = new Vector3(Singleton.Instance.starts[startLocation].x, Singleton.Instance.starts[startLocation].y, 0);

            enemyGO.GetComponent<EnemyController>().populateEnemy(enemy);
        }
    }
}
