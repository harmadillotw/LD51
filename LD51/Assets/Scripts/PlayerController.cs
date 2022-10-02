using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //public Vector2 speed = new Vector2(10, 10);
    public float speed = 20.0f;
    public GameObject meleeAttackPrefab;
    public GameObject rangedAttackPrefab;
    public GameObject magicAttackPrefab;

    public Sprite RangeSprite;

    public int meleeDamage;
    public int rangedDamage;
    public int magicDamage;
    public int attackType = Constants.ATTACK_TYPE_MELEE;

    public int health;

    public AudioSource playerAudioSource;
    public AudioClip rangeClip;
    public AudioClip dropClip;
    public AudioClip meleeClip;
    public AudioClip magicClip;
    public AudioClip damageClip;

    //private GameObject attackPosition;

    private Rigidbody2D body;
    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;

    private Vector3 mouseposition;
    private Vector2 direction;

    private float rangeAttackSpeed = 5f;

    private List<Attack> attacks;

    




    // Start is called before the first frame update
    void Start()
    {

        body = GetComponent<Rigidbody2D>();
        //attackPosition = GameObject.Find("Attackposition");
        attacks = new List<Attack>();
        meleeDamage = Singleton.Instance.playerMeleeDamage; ;
        rangedDamage = Singleton.Instance.playerRangedDamage;
        magicDamage = Singleton.Instance.playerMagicDamage;

        health = Singleton.Instance.playerHealth;

        attackType = Singleton.Instance.playerAttackType;
        showWeapon(attackType);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Singleton.Instance.paused = !Singleton.Instance.paused;
        }
        if (!Singleton.Instance.isPaused())
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if (Singleton.Instance.reverseControls)
            {
                horizontal *= -1;
                vertical *= -1;
            }

            // Change Attack
            if (Input.GetMouseButtonDown(1))
            {
                if (!Singleton.Instance.attackLocked)
                {
                    attackType++;
                    if (attackType > Constants.MAX_ATTACKS)
                    {
                        attackType = 0;
                    }
                    showWeapon(attackType);
                }
            }

            // Attack
            if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetMouseButtonDown(0)))
            {
                switch (attackType)
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


            mouseposition = Input.mousePosition;
            mouseposition = Camera.main.ScreenToWorldPoint(mouseposition);
            direction = new Vector2(
                    mouseposition.x - transform.position.x,
                    mouseposition.y - transform.position.y);
            transform.up = direction;

            checkHealth();
        }
    }
    void FixedUpdate()
    {
        if (!Singleton.Instance.isPaused())
        {
            if (horizontal != 0 && vertical != 0)
            {
                // limit diagonal speed to 70%.
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }
            //Quaternion rotation;
            Vector2 movement = new Vector2(horizontal * speed, vertical * speed);



            body.velocity = movement;
            transform.up = direction;
        }
        else
        {
            Vector2 movement = new Vector2(0, 0);
            body.velocity = movement;
            body.angularVelocity = 0f;
        }

    }

    void showWeapon(int attacktype)
    {
        if (attackType == Constants.ATTACK_TYPE_MELEE)
        {
            transform.Find("sword2").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("sword2").gameObject.SetActive(false);
        }

        if (attackType == Constants.ATTACK_TYPE_PROJECTILE)
        {
            transform.Find("Bow").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Bow").gameObject.SetActive(false);
        }


    }
    void meleeAttack()
    {
        playAudio(meleeClip, playerAudioSource, false);
        transform.Find("sword2").gameObject.SetActive(false);
        GameObject r = Instantiate(meleeAttackPrefab);
        r.transform.SetParent(transform);
        r.transform.localPosition = new Vector3(0, 1, 0);
        r.transform.up = direction;
        Destroy(r, 0.3f);
    }

    void rangedAttack()
    {
        playAudio(rangeClip, playerAudioSource, false);
        GameObject r = Instantiate(rangedAttackPrefab);
        r.transform.position = transform.Find("Bow").position;
        r.transform.up = direction;
        r.GetComponent<SpriteRenderer>().sprite = RangeSprite;
        Rigidbody2D rb = r.GetComponent<Rigidbody2D>();
        rb.angularVelocity = 0f;
        rb.velocity = (direction * rangeAttackSpeed);

    }

    void magicAttack()
    {
        playAudio(magicClip, playerAudioSource, false);
        GameObject r = Instantiate(magicAttackPrefab);
        r.transform.SetParent(transform);
        r.transform.localPosition = new Vector3(0, 1, 0);
        r.transform.up = direction;
        Destroy(r, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy Controller OnTriggerEnter2D " + gameObject.tag + "," + collision.gameObject.tag);
        if (collision.tag == "EnemyProjectile")
        {
            playAudio(damageClip, playerAudioSource, false);
            int damage = collision.gameObject.GetComponent<ProjectileController>().damage + Singleton.Instance.rangedMod;
            if (damage < 1)
            {
                damage = 1;
            }
            takeDamage(damage);
        }
        else if (collision.tag == "EnemyMelee")
        {
            playAudio(damageClip, playerAudioSource, false);
            int damage = collision.transform.parent.GetComponentInChildren<MeleeController>().damage + Singleton.Instance.meleeMod;
            if (damage < 1)
            {
                damage = 1;
            }
            takeDamage(damage);
        }
        else if (collision.tag == "EnemyMagic")
        {
            int damage = collision.gameObject.GetComponent<MagicController>().damage + Singleton.Instance.magicMod;
            if (damage < 1)
            {
                damage = 1;
            }
            takeDamage(damage);
        }
        else if (collision.tag == "Door")
        {
            //enterDoor(collision.gameObject.GetComponent<>
            // );
            //collision.gameObject.GetComponent<DoorController>().door.destinationRoom;
            enterDoor(collision.gameObject.GetComponent<DoorController>().door);
        }
        else if (collision.tag == "DropMelee")
        {
            playAudio(dropClip, playerAudioSource, false);
            meleeDamage++;
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "DropRange")
        {
            rangedDamage++;
            playAudio(dropClip, playerAudioSource, false);
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "DropMagic")
        {
            magicDamage++;
            playAudio(dropClip, playerAudioSource, false);
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "DropHealth")
        {
            playAudio(dropClip, playerAudioSource, false);
            health += 4;
            Destroy(collision.gameObject);
        }
    }
    private void takeDamage(int damage)
    {
        health -= damage;
    }

    private void checkHealth()
    {
        if (health <= 0)
        {
            Singleton.Instance.startGameOver = true;
        }
    }

    private void enterDoor(Door door)
    {
        if (door.exit)
        {
            Singleton.Instance.bossDead = true;
            Singleton.Instance.startGameOver = true;
        }
        else
        {
            //Singleton.Instance.paused = true;
            Singleton.Instance.roomTransition = true;

            Singleton.Instance.playerHealth = health;
            Singleton.Instance.playerMeleeDamage = meleeDamage;
            Singleton.Instance.playerRangedDamage = rangedDamage;
            Singleton.Instance.playerMagicDamage = magicDamage;
            Singleton.Instance.playerAttackType = attackType;

            Singleton.Instance.fromRoomId = Singleton.Instance.currentRoomId;
            Singleton.Instance.currentRoomId = door.destinationRoom;
            Singleton.Instance.fromDoor = door;
        }
        // save enemies
        // fade to black.
        // load new room.
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

}


