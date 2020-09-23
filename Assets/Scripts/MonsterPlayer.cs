using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonsterPlayer : NetworkBehaviour
{

    public bool isMonster;
    public Player player;


    void Start()
    {
        isMonster = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(this.gameObject.name + " is now a monster");
            isMonster = true;
            MonsterSetup();
        }
    }


    void MonsterSetup()
    {
        
    }
}
