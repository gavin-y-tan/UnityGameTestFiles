using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour
{
    public GameObject model;
    


    [SyncVar]
    private bool _isDead = false;
    public bool isDead



    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }


    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= amount;
        Debug.Log(transform.name + " now has" + currentHealth + " health");

        if (currentHealth<=0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        for (int i=0; i<disableOnDeath.Length;i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider _col = GetComponent<Collider>(); //disable collider
        if (_col != null)
        {
            _col.enabled = false;
        }
        Debug.Log(transform.name + " has died");

        model.GetComponent<Renderer>().material.color = Color.green; //change color
        
        //Respawning
        StartCoroutine(Respawn());

    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        



        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name+" respawned");
        SetDefaults();
    }
    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        model.GetComponent<Renderer>().material.color = Color.red;

        for (int i=0; i<disableOnDeath.Length;i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }




    }





}
