using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splashController : MonoBehaviour
{

    private float splashTimer;

    // How long we see the splash screen for. Make longer the first time a user sees it.
    private float splashPeriod = 3f;
    // Start is called before the first frame update
    void Start()
    {


        Singleton.Instance.starts.Add(0, new IntPair(-14, 10));
        Singleton.Instance.starts.Add(1, new IntPair(-10, 10));
        Singleton.Instance.starts.Add(2, new IntPair(-6, 10));
        Singleton.Instance.starts.Add(3, new IntPair(-2, 10));
        Singleton.Instance.starts.Add(4, new IntPair(2, 10));
        Singleton.Instance.starts.Add(5, new IntPair(6, 10));
        Singleton.Instance.starts.Add(6, new IntPair(10, 10));
        Singleton.Instance.starts.Add(7, new IntPair(14, 10));

        Singleton.Instance.starts.Add(8, new IntPair(-14, 6));
        Singleton.Instance.starts.Add(9, new IntPair(14, 6));
        Singleton.Instance.starts.Add(10, new IntPair(-14, 2));
        Singleton.Instance.starts.Add(11, new IntPair(14, 2));
        Singleton.Instance.starts.Add(12, new IntPair(-14, -2));
        Singleton.Instance.starts.Add(13, new IntPair(14, -2));

        Singleton.Instance.starts.Add(14, new IntPair(-14, -6));
        Singleton.Instance.starts.Add(15, new IntPair(-10, -6));
        Singleton.Instance.starts.Add(16, new IntPair(-6, -6));
        Singleton.Instance.starts.Add(17, new IntPair(-2, -6));
        Singleton.Instance.starts.Add(18, new IntPair(2, -6));
        Singleton.Instance.starts.Add(19, new IntPair(6, -6));
        Singleton.Instance.starts.Add(20, new IntPair(10, -6));
        Singleton.Instance.starts.Add(21, new IntPair(14, -6));

        splashTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        splashTimer += Time.deltaTime;
        if ((Input.anyKey) || splashTimer > splashPeriod)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
        
    }
}
