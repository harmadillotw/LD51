using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
