using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsController : MonoBehaviour
{
    public Text controlsText;
    public Text enemiesText;
    public Text pickupsText;

    public Image controlsImage;
    public Image enemiesImage;
    public Image pickupsImage;

    private int screen;

    // Start is called before the first frame update
    void Start()
    {
        screen = 0;

        controlsText.gameObject.SetActive(true);
        enemiesText.gameObject.SetActive(false);
        pickupsText.gameObject.SetActive(false);

        controlsImage.gameObject.SetActive(true);
        enemiesImage.gameObject.SetActive(false);
        pickupsImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScreen()
    {
        if (screen == 0)
        {
            screen = 1;
            controlsText.gameObject.SetActive(false);
            enemiesText.gameObject.SetActive(true);
            pickupsText.gameObject.SetActive(false);

            controlsImage.gameObject.SetActive(false);
            enemiesImage.gameObject.SetActive(true);
            pickupsImage.gameObject.SetActive(false);
        } else if (screen == 1)
        {
            screen = 2;
            controlsText.gameObject.SetActive(false);
            enemiesText.gameObject.SetActive(false);
            pickupsText.gameObject.SetActive(true);

            controlsImage.gameObject.SetActive(false);
            enemiesImage.gameObject.SetActive(false);
            pickupsImage.gameObject.SetActive(true);
        } else if (screen == 2)
        {
            screen = 0;
            controlsText.gameObject.SetActive(true);
            enemiesText.gameObject.SetActive(false);
            pickupsText.gameObject.SetActive(false);

            controlsImage.gameObject.SetActive(true);
            enemiesImage.gameObject.SetActive(false);
            pickupsImage.gameObject.SetActive(false);
        }
    }
}
