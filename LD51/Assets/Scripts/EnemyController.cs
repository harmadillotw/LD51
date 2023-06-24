using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public GameObject player;

    public List<Sprite> EnemySprites;


    public GameObject meleeAttackPrefab;
    public GameObject rangedAttackPrefab;
    public GameObject magicAttackPrefab;

    public GameObject bossMeleeAttackPrefab;
    public GameObject bossRangedAttackPrefab;
    public GameObject bossMagicAttackPrefab;



    public List<GameObject> DropPrefabs;

    public AudioSource playerAudioSource;

    
    public AudioClip rangeClip;
    public AudioClip meleeClip;
    public AudioClip magicClip;
    public AudioClip damageClip;

    // Pause values
    private GameController gameController;
    private Vector2 savedVelocity;
    private float savedAngularVelocity;

    //debug
    public TextMeshProUGUI debugText;
    private UIController uiController;
    public Canvas debugCanvas;



    private float rangeAttackSpeed = 3.5f;

    private Enemy enemy;
    private Rigidbody2D body;

    private Vector2 direction;
    private float speed = 0.5f;
    private float bossSpeed = 0.2f;

    private float attackTimer;

    private float bossTimer;

    private bool paused = false;

    private bool debugEnabled = false; 

    private bool isBeingDestroyed;



    private void Awake()
    {
        paused = false;
        gameController = GameObject.FindObjectOfType<GameController>();
        gameController.pauseEvent += processPauseEvent;
        gameController.unpauseEvent += processUnpauseEvent;

        uiController = GameObject.FindObjectOfType<UIController>();
        uiController.enableDebugModeEvent += processenableDebugModeEvent;
        uiController.disableDebugModeEvent += processdisableDebugModeEvent;
        debugCanvas = GetComponentInChildren<Canvas>();

    }
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        debugText = GetComponentInChildren<TextMeshProUGUI>();
        debugCanvas.gameObject.SetActive(Singleton.Instance.debugMode);
        attackTimer = 0;
        bossTimer = 0f;
        if (enemy.attackType == Constants.ATTACK_TYPE_MELEE)
        {
            transform.Find("sword2").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("sword2").gameObject.SetActive(false);
        }

        if (enemy.attackType == Constants.ATTACK_TYPE_PROJECTILE)
        {
            transform.Find("Bow").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Bow").gameObject.SetActive(false);
        }

        if (enemy.bossEnemy)
        {
            Singleton.Instance.bossHealth = enemy.health;
        }
        debugText.transform.parent.transform.LookAt(Camera.main.transform);
        debugText.text = "H:" + enemy.health + " D:" + enemy.attackDamage;

    }
 

    // Update is called once per frame
    void Update()
    {
        if ((!Singleton.Instance.isPaused()) && !paused)
        {

            if (enemy.bossEnemy)
            {
                bossTimer += Time.deltaTime;
                if (bossTimer > 5f)
                {
                    enemy.attackType++;
                    if (enemy.attackType > 3)
                    {
                        enemy.attackType = 0;
                    }
                    if (enemy.attackType == Constants.ATTACK_TYPE_MELEE)
                    {
                        transform.Find("sword2").gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.Find("sword2").gameObject.SetActive(false);
                    }

                    if (enemy.attackType == Constants.ATTACK_TYPE_PROJECTILE)
                    {
                        transform.Find("Bow").gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.Find("Bow").gameObject.SetActive(false);
                    }
                    bossTimer = 0f;
                }
            }
            direction = new Vector2(
                player.transform.position.x - transform.position.x,
                player.transform.position.y - transform.position.y);
            attackTimer += Time.deltaTime;
            if (attackTimer > enemy.attackFrequency)
            {
                attack();
                attackTimer = 0;
            }
            checkHealth();
            debugText.text = "H:" + enemy.health + " D:" + enemy.attackDamage;
            debugText.transform.parent.transform.LookAt(Camera.main.transform);
        }
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            if (!Singleton.Instance.isPaused())
            {
                if (enemy.bossEnemy)
                {
                    Vector2 movement = new Vector2(direction.x * speed, direction.y * bossSpeed);
                    body.velocity = movement;
                }
                else
                {
                    Vector2 movement = new Vector2(direction.x * speed, direction.y * speed);
                    body.velocity = movement;
                }

                transform.up = direction;
            }
            else
            {
                Vector2 movement = new Vector2(0, 0);
                body.velocity = movement;
                body.angularVelocity = 0f;
            }
        }
        
    }
    private void processPauseEvent(object sender, System.EventArgs e)
    {
        paused = true;

        savedVelocity = body.velocity;
        savedAngularVelocity = body.angularVelocity;
        Vector2 movement = new Vector2(0, 0);
        body.velocity = movement;
        body.angularVelocity = 0f;

    }

    private void processUnpauseEvent(object sender, System.EventArgs e)
    {
        paused = false;
        body.velocity = savedVelocity;
        body.angularVelocity = savedAngularVelocity;
    }

    private void processenableDebugModeEvent(object sender, System.EventArgs e)
    {
        debugEnabled = Singleton.Instance.debugMode;
        if (debugCanvas != null)
        {
            debugCanvas.gameObject.SetActive(debugEnabled);
        }
    }

    private void processdisableDebugModeEvent(object sender, System.EventArgs e)
    {
        debugEnabled = Singleton.Instance.debugMode;
        if (debugCanvas != null)
        {
            debugCanvas.gameObject.SetActive(debugEnabled);
        }
    }
    private void attack()
    {
        switch (enemy.attackType)
        {
            case Constants.ATTACK_TYPE_MELEE:
                meleeAttack();
                break;
            case Constants.ATTACK_TYPE_PROJECTILE:
                rangedAttack();
                break;
            case Constants.ATTACK_TYPE_MAGIC:
                magicAttack();
                break;

        }
    }

    void meleeAttack()
    {
        playAudio(meleeClip, playerAudioSource, false);
        transform.Find("sword2").gameObject.SetActive(false);
        if (enemy.bossEnemy)
        {
            GameObject r = Instantiate(bossMeleeAttackPrefab);
            r.tag = "EnemyMelee";
            foreach (Transform t in r.transform)
            {
                t.gameObject.tag = "EnemyMelee";
            }
            r.transform.SetParent(transform);
            r.transform.localPosition = new Vector3(0, 1, 0);
            r.transform.up = direction;
            Destroy(r, 0.3f);
        }
        else
        {
            GameObject r = Instantiate(meleeAttackPrefab);
            r.tag = "EnemyMelee";
            foreach (Transform t in r.transform)
            {
                t.gameObject.tag = "EnemyMelee";
            }
            r.transform.SetParent(transform);
            r.transform.localPosition = new Vector3(0, 1, 0);
            r.transform.up = direction;
            Destroy(r, 0.3f);
        }
    }

    void rangedAttack()
    {
        playAudio(rangeClip, playerAudioSource, false);
        if (enemy.bossEnemy)
        {
            GameObject r = Instantiate(bossRangedAttackPrefab);
            r.tag = "EnemyProjectile";
            foreach (Transform t in r.transform)
            {
                t.gameObject.tag = "EnemyProjectile";
            }
            r.transform.position = transform.Find("Bow").position;
            r.transform.up = direction;
            Rigidbody2D rb = r.GetComponent<Rigidbody2D>();
            rb.angularVelocity = 0f;
            rb.velocity = (direction * rangeAttackSpeed);
        }
        else
        {
            GameObject r = Instantiate(rangedAttackPrefab);
            r.tag = "EnemyProjectile";
            foreach (Transform t in r.transform)
            {
                t.gameObject.tag = "EnemyProjectile";
            }
            r.transform.position = transform.Find("Bow").position;
            r.transform.up = direction;
            Rigidbody2D rb = r.GetComponent<Rigidbody2D>();
            rb.angularVelocity = 0f;
            rb.velocity = (direction * rangeAttackSpeed);
        }

    }

    void magicAttack()
    {
        playAudio(magicClip, playerAudioSource, false);
        if (enemy.bossEnemy)
        {
            GameObject r = Instantiate(bossMagicAttackPrefab);
            r.tag = "EnemyMagic";
            foreach (Transform t in r.transform)
            {
                t.gameObject.tag = "EnemyMagic";
            }
            r.transform.SetParent(transform);
            r.transform.localPosition = new Vector3(0, 1, 0);
            r.transform.up = direction;
            Destroy(r, 0.5f);
        }
        else
        {
            GameObject r = Instantiate(magicAttackPrefab);
            r.tag = "EnemyMagic";
            foreach (Transform t in r.transform)
            {
                t.gameObject.tag = "EnemyMagic";
            }
            r.transform.SetParent(transform);
            r.transform.localPosition = new Vector3(0, 1, 0);
            r.transform.up = direction;
            Destroy(r, 0.5f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Debug.Log("Collission" + collision.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy Controller OnTriggerEnter2D " + gameObject.tag + "," + collision.gameObject.tag);
        if (collision.tag == "Projectile") 
        {
            playAudio(damageClip, playerAudioSource, false);
            int damage = collision.gameObject.GetComponent<ProjectileController>().damage + Singleton.Instance.rangedMod;
            if (damage < 1)
            {
                damage = 1;
            }
            takeDamage(damage);
        }
        else if (collision.tag == "Melee")
        {
            playAudio(damageClip, playerAudioSource, false);
            int damage = collision.transform.parent.GetComponentInChildren<MeleeController>().damage + Singleton.Instance.meleeMod;
            if (damage < 1)
            {
                damage = 1;
            }
            takeDamage(damage);
        }
        else if (collision.tag == "Magic")
        {

            int damage = collision.gameObject.GetComponent<MagicController>().damage + Singleton.Instance.magicMod;
            if (damage < 1)
            {
                damage = 1;
            }
            takeDamage(damage);
        }
    }

    public void populateEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        setAttackType(enemy.attackType);
    }

    public Enemy getEnemy()
    {
        return this.enemy;
    }

    private void takeDamage(int damage)
    {
        enemy.health -= damage;
        if (enemy.bossEnemy)
        {
            Singleton.Instance.bossHealth = enemy.health;
        }
    }

    private void checkHealth()
    {
        if (enemy.health <= 0)
        {
            int drop = Random.Range(0, 4);
            if (drop < DropPrefabs.Count)
            {
                GameObject r = Instantiate(DropPrefabs[drop]);
                switch (drop)
                {
                    case 0:
                        r.tag = "DropMelee";
                        break;
                    case 1:
                        r.tag = "DropRange";
                        break;
                    case 2:
                        r.tag = "DropMagic";
                        break;
                    case 3:
                        r.tag = "DropHealth";
                        break;

                }
                r.transform.position = transform.position;
                
            }
            if (enemy.bossEnemy)
            {
                Singleton.Instance.roomTransition = true;
            }
            DestroyThisEnemy();
        }
    }

    private void DestroyThisEnemy()
    {
        isBeingDestroyed = true;
        Destroy(gameObject);
    }

    public void enableSword()
    {
        if (!isBeingDestroyed)
        {
            transform.Find("sword2").gameObject.SetActive(true);
        }
    }
    public void setAttackType(int attackType)
    {
        switch (attackType)
        {
            case Constants.ATTACK_TYPE_MELEE:
                //GetComponent<SpriteRenderer>().sprite = EnemySprites[0];
                transform.Find("EnemyBody").GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case Constants.ATTACK_TYPE_PROJECTILE:
                //GetComponent<SpriteRenderer>().sprite = EnemySprites[1];
                transform.Find("EnemyBody").GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case Constants.ATTACK_TYPE_MAGIC:
                //GetComponent<SpriteRenderer>().sprite = EnemySprites[2];
                transform.Find("EnemyBody").GetComponent<SpriteRenderer>().color = Color.blue;
                break;

        }
    }
    private void playAudio(AudioClip clip, AudioSource audioSource, bool contPlay)
    {
        if ((contPlay) && (audioSource.isPlaying))
        {
            return;
        }
        int volumeSet = PlayerPrefs.GetInt("FXvolumeSet");
        float vol = 1f;
        if (volumeSet > 0)
        {
            int volume = PlayerPrefs.GetInt("FXVolume");
            vol = 1f;
            vol = (float)volume / 100f;
        }

        audioSource.PlayOneShot(clip, vol);
    }

    private void OnDestroy()
    {
        gameController.pauseEvent -= processPauseEvent;
        gameController.unpauseEvent -= processUnpauseEvent;
    }
}
