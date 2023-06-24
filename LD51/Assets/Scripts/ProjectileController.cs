using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public int damage = 1;

    // Pause values
    private bool paused = false;
    private GameController gameController;
    private Vector2 savedVelocity;
    private float savedAngularVelocity;

    private Rigidbody2D body;

    private void Awake()
    {
        paused = false;
        gameController = GameObject.FindObjectOfType<GameController>();
        body = GetComponent<Rigidbody2D>();
        gameController.pauseEvent += processPauseEvent;
        gameController.unpauseEvent += processUnpauseEvent;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ProjectileController OnTriggerEnter2D " + gameObject.tag + "," + collision.gameObject.tag);
        if (gameObject.tag == "EnemyProjectile")
        {
        }
        else
        {
            if ((collision.tag == "Enemy") || (collision.tag == "Wall"))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        gameController.pauseEvent -= processPauseEvent;
        gameController.unpauseEvent -= processUnpauseEvent;
    }
}
