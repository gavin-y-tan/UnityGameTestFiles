using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Objective : NetworkBehaviour
{



    [SerializeField]
    public float maxHealth = 100;

    public bool isGenerator;
    public bool isHullDamage;
    public bool isNavigator;

    [SyncVar]
    public float currentHealth=100;




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ClientRpc]
    public void RpcInteractAmount(float amount)
    {


        currentHealth -= amount;



        if (currentHealth <= 0 && this.isHullDamage==true)
        {

            NetworkIdentity.Destroy(this.gameObject);
        }
        if (currentHealth <= 0 && this.isGenerator==true)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
