using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoOptions()
    {
        SceneManager.LoadScene("OptionsScene");
    }
    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void GoCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void GoInstructions()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void GoExit()
    {
        Application.Quit();
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

    public void GoDungeonCreator()
    {
        SceneManager.LoadScene("CreateDungeonScene");
    }
    public void GoLoadGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

}
