using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 1;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void processPauseEvent(object sender, System.EventArgs e)
    {
        paused = true;
        var animator = GetComponent<Animator>();
        prevSpeed = animator.speed;
   
 
        animator.speed = 0f;
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
    }
}
