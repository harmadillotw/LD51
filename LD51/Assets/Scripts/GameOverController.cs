using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    // Start is called before the first frame update

    public Text endingText;
    void Start()
    {
        if (Singleton.Instance.bossDead)
        {
            endingText.text = " Congratulations.\n The Choas Overlord is dead.\n You have Won.\n\n For now.";
        }
        else
        {
            endingText.text = " Game Over\n You have died.\n Better luck next time.\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
