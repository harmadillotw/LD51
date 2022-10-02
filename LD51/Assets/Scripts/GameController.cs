using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject coundownGO;

    public GameObject transitionGO;
    //TextMeshPro countdownText;
    Text countdownText;
    Text TransitionText;
    float timer = 0.0f;
    // Start is called before the first frame update

    public event EventHandler darkEvent;

    public event EventHandler lightEvent;

    public event EventHandler spawnEnemiesEvent;

    private MusicController mc;

    void Start()
    {
        mc = GameObject.FindObjectOfType<MusicController>();

        countdownText = coundownGO.GetComponent<Text>();
        TransitionText = transitionGO.GetComponent<Text>();
        timer = Singleton.Instance.countdownTimer;

        float displayTimer = (10f - timer);
        if (displayTimer < 0)
        {
            displayTimer = 0;
        }
        countdownText.text = "" + (displayTimer);

        setModifier(Singleton.Instance.setCondition);

    }

    // Update is called once per frame
    void Update()
    {
        if (!Singleton.Instance.isPaused())
        {
            timer += Time.deltaTime;
            if (timer > 10f)
            {
                timer = 0f;
                newModifier();
            }
            float displayTimer = (10f - timer);
            if (displayTimer < 0)
            {
                displayTimer = 0;
            }
            countdownText.text = "" + (displayTimer);
        }

    }

    private void newModifier()
    {
        int newCondition = UnityEngine.Random.Range(0, 11);
        if (newCondition != 0)
        {
            transitionGO.SetActive(true);
        }

        // Undo fade to black
        if (Singleton.Instance.setCondition == Constants.CONDITION_DARK)
        {
            lightEvent?.Invoke(this, EventArgs.Empty);
        }
        if (Singleton.Instance.setCondition == Constants.CONDITION_REVERSE_CONTROLS)
        {
            Singleton.Instance.reverseControls = false;
        }
        Singleton.Instance.meleeMod = 0;
        Singleton.Instance.rangedMod = 0;
        Singleton.Instance.magicMod = 0;

        Singleton.Instance.attackLocked = false;

        //if (Singleton.Instance.setCondition == Constants.CONDITION_MUSIC_CHANGE)
        //{
        //    newCondition = 0;
        //}
        //else
        //{
        //    newCondition = Constants.CONDITION_MUSIC_CHANGE;
        //}
        setModifier(newCondition);
    }
    private void setModifier(int newCondition)
    { 
        Singleton.Instance.setCondition = newCondition;
        switch (newCondition)
        {
            case 0:
                Singleton.Instance.condition = Constants.CONDITION_NORMAL;
                transitionGO.SetActive(false);
                break;
            case 1:
                Singleton.Instance.condition = Constants.CONDITION_PLUS_MELEE;
                TransitionText.text = "+MELEE";
                Singleton.Instance.meleeMod = 5;
                break;
            case 2:
                Singleton.Instance.condition = Constants.CONDITION_PLUS_RANGED;
                TransitionText.text = "+RANGED";
                Singleton.Instance.rangedMod = 5;
                break;
            case 3:
                Singleton.Instance.condition = Constants.CONDITION_PLUS_MAGIC;
                TransitionText.text = "+MAGIC";
                Singleton.Instance.magicMod = 5;
                break;
            case 4:
                Singleton.Instance.condition = Constants.CONDITION_MINUS_MELEE;
                TransitionText.text = "-MELEE";
                Singleton.Instance.meleeMod = -5;
                break;
            case 5:
                Singleton.Instance.condition = Constants.CONDITION_MINUS_RANGED;
                TransitionText.text = "-RANGED";
                Singleton.Instance.rangedMod = -5;
                break;
            case 6:
                Singleton.Instance.condition = Constants.CONDITION_MINUS_MAGIC;
                TransitionText.text = "-MAGIC";
                Singleton.Instance.magicMod = -5;
                break;
            case 7:
                Singleton.Instance.condition = Constants.CONDITION_SPAWN_ENEMIES;       
                TransitionText.text = "INVASION";
                spawnEnemiesEvent?.Invoke(this, EventArgs.Empty);
                break;
            case 8:
                Singleton.Instance.condition = Constants.CONDITION_REVERSE_CONTROLS;    
                TransitionText.text = "INVERTED";
                Singleton.Instance.reverseControls = true; 
                break;
            case 9:
                Singleton.Instance.condition = Constants.CONDITION_NO_SOUND;            //TODO
                TransitionText.text = "QUIET";
                break;
            case 10:
                Singleton.Instance.condition = Constants.CONDITION_DARK;
                TransitionText.text = "DARK";
                darkEvent?.Invoke(this, EventArgs.Empty);
                break;
            case 11:
                Singleton.Instance.condition = Constants.CONDITION_ATTACK_LOCKED;
                TransitionText.text = "WEAPON LOCKED";
                Singleton.Instance.attackLocked = true; ;
                break;

            case 12:
                Singleton.Instance.condition = Constants.CONDITION_MUSIC_CHANGE;
                TransitionText.text = "";
                mc.switchTrack();
                break;

        }


    }

    void OnDestroy()
    {
        Singleton.Instance.countdownTimer = timer;
    }
}
