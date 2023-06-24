using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    public int damage = 1;

    public bool player = false;

    //Pause values
    private bool paused = false;
    private GameController gameController;
    private Animator animator;
    private float prevSpeed;

    private void Awake()
    {
        paused = false;
        prevSpeed = 1;
        gameController = GameObject.FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();
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
        
    }

    public void setPlayer()
    {
        this.player = true;
    }
    private void processPauseEvent(object sender, System.EventArgs e)
    {
        paused = true;
        var animator = GetComponent<Animator>();
        prevSpeed = animator.speed;
        animator.speed = 0;
    }

    private void processUnpauseEvent(object sender, System.EventArgs e)
    {
        paused = false;
        animator.speed = prevSpeed;
    }



    private void OnDestroy()
    {
        gameController.pauseEvent -= processPauseEvent;
        gameController.unpauseEvent -= processUnpauseEvent;

        if (player)
        {
            transform.parent.parent.transform.Find("sword2").gameObject.SetActive(true);
        }
        else
        {
            transform.parent.parent.GetComponent<EnemyController>().enableSword();
        }

        
        //r.GetComponent<MeleeController>().setPlayer();
    }
}
