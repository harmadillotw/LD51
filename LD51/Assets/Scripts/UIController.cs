using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIController : MonoBehaviour
{

    public GameObject player;

    public GameObject gameController;

    private GameObject blackOutSquare;

    private GameObject screenMaskImage;

    private Text Healthtext;
    private Text MeleeDamagetext;
    private Text RangeDamagetext;
    private Text Magictext;

    private Text MeleeBonustext;
    private Text RangeBonustext;
    private Text MagicBonustext;

    private Text BossHealthtext;

    private Image MeleeInd;
    private Image RangedInd;
    private Image MagicInd;

    private GameObject pauseMenuPanel;

    private Toggle debugToggle;

    public event EventHandler enableDebugModeEvent;
    public event EventHandler disableDebugModeEvent;

    private void Awake()
    {
        pauseMenuPanel = GameObject.Find("PauseMenuPanel");
        screenMaskImage = GameObject.Find("ScreenMaskImage");
        debugToggle = GameObject.Find("DebugToggle").GetComponentInChildren<Toggle>();
        gameController.GetComponent<GameController>().darkEvent += processDarkEvent;
        gameController.GetComponent<GameController>().lightEvent += processLightEvent;
        gameController.GetComponent<GameController>().pauseEvent += processPauseEvent;
        gameController.GetComponent<GameController>().unpauseEvent += processUnpauseEvent;
        

    }
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuPanel.SetActive(Singleton.Instance.paused);
        Healthtext = GameObject.Find("HealthText").GetComponent<Text>();
        MeleeDamagetext = GameObject.Find("MeleeText").GetComponent<Text>();
        RangeDamagetext = GameObject.Find("RangedText").GetComponent<Text>();
        Magictext = GameObject.Find("MagicText").GetComponent<Text>();
        blackOutSquare = GameObject.Find("FadeOutImage");

        MeleeBonustext = GameObject.Find("MeleeBonusText").GetComponent<Text>();
        RangeBonustext = GameObject.Find("RangedBonusText").GetComponent<Text>();
        MagicBonustext = GameObject.Find("MagicBonusText").GetComponent<Text>();

        MeleeInd = GameObject.Find("MeleeInd").GetComponent<Image>();
        RangedInd = GameObject.Find("RangedInd").GetComponent<Image>();
        MagicInd = GameObject.Find("MagicInd").GetComponent<Image>();

        BossHealthtext = GameObject.Find("BossHealthText").GetComponent<Text>();

        
        //screenMaskImage = GameObject.Find("ScreenMaskImage");

        //gameController.GetComponent<GameController>().darkEvent += processDarkEvent;
        //gameController.GetComponent<GameController>().lightEvent += processLightEvent;
    }

    // Update is called once per frame
    void Update()
    {
        Healthtext.text = "" + player.GetComponent<PlayerController>().health;
        MeleeDamagetext.text = "" + player.GetComponent<PlayerController>().meleeDamage;
        RangeDamagetext.text = "" + player.GetComponent<PlayerController>().rangedDamage;
        Magictext.text = "" + player.GetComponent<PlayerController>().magicDamage;

        if (Singleton.Instance.meleeMod > 0)
        {
            MeleeBonustext.text = "+" + Singleton.Instance.meleeMod;
        }
        else if (Singleton.Instance.meleeMod < 0)
        {
            MeleeBonustext.text = "" + Singleton.Instance.meleeMod;
        }
        else
        {
            MeleeBonustext.text = "";
        }
        if (Singleton.Instance.rangedMod > 0)
        {
            RangeBonustext.text = "+" + Singleton.Instance.rangedMod;
        }
        else if (Singleton.Instance.rangedMod < 0)
        {
            RangeBonustext.text = "" + Singleton.Instance.rangedMod;
        }
        else
        {
            RangeBonustext.text = "";
        }
        if (Singleton.Instance.magicMod > 0)
        {
            MagicBonustext.text = "+" + Singleton.Instance.magicMod;
        }
        else if (Singleton.Instance.magicMod < 0)
        {
            MagicBonustext.text = "" + Singleton.Instance.magicMod;
        }
        else
        {
            MagicBonustext.text = "";
        }

        if (player.GetComponent<PlayerController>().attackType == Constants.ATTACK_TYPE_MELEE)
        {
            MeleeInd.color = Color.green;
        }
        else
        {
            MeleeInd.color = Color.white;
        }
        if (player.GetComponent<PlayerController>().attackType == Constants.ATTACK_TYPE_PROJECTILE)
        {
            RangedInd.color = Color.green;
        }
        else
        {
            RangedInd.color = Color.white;
        }
        if (player.GetComponent<PlayerController>().attackType == Constants.ATTACK_TYPE_MAGIC)
        {
            MagicInd.color = Color.green;
        }
        else
        {
            MagicInd.color = Color.white;
        }
        //switch (player.GetComponent<PlayerController>().attackType)
        //{
        //    case Constants.ATTACK_TYPE_MELEE:
        //        //MeleeDamagetext.text = "*Me" + player.GetComponent<PlayerController>().meleeDamage;
        //        MeleeInd.color = Color.green;
        //        break;
        //    case Constants.ATTACK_TYPE_PROJECTILE:
        //        RangeDamagetext.text = "*R" + player.GetComponent<PlayerController>().rangedDamage;
        //        break;
        //    case Constants.ATTACK_TYPE_MAGIC:
        //        Magictext.text = "*Ma" + player.GetComponent<PlayerController>().magicDamage;
        //        break;
        //}

        if (Singleton.Instance.startGameOver)
        {
            Singleton.Instance.startGameOver = false;
            StartCoroutine(FadeBlackOutSquare(true,5,true));
        }
        if (Singleton.Instance.roomTransition)
        {
            Singleton.Instance.roomTransition = false;
            StartCoroutine(FadeBlackOutSquare(true, 5, false));
        }

        if (Singleton.Instance.bossRoom)
        {
            BossHealthtext.gameObject.SetActive(true);
            BossHealthtext.text = "Chaos Boss Health: " + Singleton.Instance.bossHealth;
        }
        else
        {
            BossHealthtext.gameObject.SetActive(false);
        }    


    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack , int fadeSpeed , bool gameOver)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            if (gameOver)
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {
                SceneManager.LoadScene("MainGameScene");
            }
        }
        else
        {
            while (blackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        if(gameOver)
        {

        }
        else
        {

        }
    }

    private void processDarkEvent(object sender, EventArgs e)
    {
        StartCoroutine(FadeScreenMaskImage(true, 5));
    }

    private void processLightEvent(object sender, EventArgs e)
    {
        StartCoroutine(FadeScreenMaskImage(false, 5));
    }
    private void processPauseEvent(object sender, EventArgs e)
    {
        pauseMenuPanel.SetActive(true);
        if (pauseMenuPanel.activeInHierarchy)
        {
            debugToggle.isOn = Singleton.Instance.debugMode;
        }
    }
    private void processUnpauseEvent(object sender, EventArgs e)
    {
        pauseMenuPanel.SetActive(false);
    }

    public IEnumerator FadeScreenMaskImage(bool fadeToBlack, int fadeSpeed)
    {
        Color objectColor = screenMaskImage.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (screenMaskImage.GetComponent<Image>().color.a < 0.8)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                screenMaskImage.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        else
        {
            while (screenMaskImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                screenMaskImage.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }

    public void toggleDebug()
    {
        Singleton.Instance.debugMode = !Singleton.Instance.debugMode;
        if (Singleton.Instance.debugMode)
        {
            enableDebugModeEvent?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            disableDebugModeEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnDestroy()
    {
        gameController.GetComponent<GameController>().pauseEvent -= processPauseEvent;
        gameController.GetComponent<GameController>().unpauseEvent -= processUnpauseEvent;
    }
}
