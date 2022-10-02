using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack 
{
    private GameObject attack;
    public float startTime;

    public Attack(GameObject go)
    {
        attack = go;
        startTime = Time.time;
    }


}
