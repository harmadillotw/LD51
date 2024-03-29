using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    public Vector3 offset;
    private Rigidbody2D playerRb;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        playerRb = player.GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
