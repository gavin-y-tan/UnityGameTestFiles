using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.Basic;

using UnityEngine.UI;
using TMPro;

public class GunScript : NetworkBehaviour
{
    public GameObject gun;

    public Player player;

    private const string PlayerTag="Player";
    private const string ObjectiveTag = "Objective";
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 5f;
    public float impactForce = 50f;

    public Text navigatorMenu;
    
    //
    public int[,] map =
  { { 0, 0, 1, 0, 1, 0, 0 },
        { 0, 1, 0, 1, 0, 1, 0 },
        { 1, 0, 1, 0, 1, 0, 1 },
        { 0, 1, 0, 1, 0, 1, 0 },
        { 0, 0, 1, 0, 1, 0, 0 } };
    public int currentRow = 2;
    public int currentCol = 0;
    //

    public float irange = 1f;

    public Image bar;

    [SerializeField]
    private Camera fpsCam;

    [SerializeField]
    private LayerMask mask;

    public float max = 100;
    public float current = 0;




    public ParticleSystem flash;


    public GameObject impact;
    public GameObject hullDamage;


    public float nttf = 0f;


    public float ntti = 0f;
    public float nttim = 0f;
    void Start()
    {
        if (fpsCam==null)
        {
            Debug.LogError("No player camera");
            this.enabled = false;
        }


    }

    void Update()
    {
        //Shooting
        if (Input.GetButton("Fire1") && Time.time >= nttf && gun.GetComponent<MeshRenderer>().enabled == true)
        {

            nttf = Time.time + (.15f);

            Shoot();

        }
        //

        //Interacting

        if (Input.GetKey(KeyCode.E) && navigatorMenu.GetComponent<Text>().enabled==true && Time.time>=ntti)
        {
            navigatorMenu.GetComponent<Text>().enabled = false;
            player.GetComponent<PlayerMovementScript>().canMove = true;
        }
        if (Input.GetKey(KeyCode.E) && navigatorMenu.GetComponent<Text>().enabled==false)
        {
            if (Time.time >= ntti)
            {
                ntti = Time.time + .05f;
                Interact(false);
            }

            gun.GetComponent<MeshRenderer>().enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            gun.GetComponent<MeshRenderer>().enabled = true;
        }
        if (bar.fillAmount >= 1f)
        {
            bar.fillAmount = 0f;
        }
        //

        //MonsterInteracting
        if (Input.GetKey(KeyCode.V) && this.GetComponent<MonsterPlayer>().isMonster==true)
        {
            if (Time.time >= nttim)
            {
                nttim = Time.time + 5f;
                Interact(true);
            }
            gun.GetComponent<MeshRenderer>().enabled = false;

        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            gun.GetComponent<MeshRenderer>().enabled = true;
        }

        


    }

    void EnableGun()
    {
        gun.GetComponent<MeshRenderer>().enabled = true;
    }

    [Command]
    void CmdNetworkInteract(GameObject gameObject, int damage)
    {
        Objective objective = gameObject.GetComponent<Objective>();

        objective.RpcInteractAmount(damage);


    }

    [Client]
    void Interact(bool destroy)
    {

        RaycastHit ihit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out ihit, irange, mask))
        {

            if (destroy == true)
            {
                if (ihit.collider.gameObject.GetComponent<Objective>().isGenerator==true)
                {
 
                    CmdNetworkInteract(ihit.collider.gameObject, -1);
                    current = ihit.collider.gameObject.GetComponent<Objective>().currentHealth;
                    Debug.Log("MonsterInteracting " +current);
                    bar.fillAmount= (ihit.collider.gameObject.GetComponent<Objective>().maxHealth - current) / ihit.collider.gameObject.GetComponent<Objective>().maxHealth;
                }
            }

            if (ihit.collider.tag==ObjectiveTag && destroy==false && ihit.collider.GetComponent<Objective>().isNavigator==false)
            {

                CmdNetworkInteract(ihit.collider.gameObject, 1);

                current=ihit.collider.gameObject.GetComponent<Objective>().currentHealth;
                
                bar.fillAmount = (ihit.collider.gameObject.GetComponent<Objective>().maxHealth-current) / ihit.collider.gameObject.GetComponent<Objective>().maxHealth;


            }

            if (ihit.collider.GetComponent<Objective>().isNavigator==true)
            {
                navigatorMenu.enabled = true;

                if (Input.GetKey(KeyCode.Alpha1))
                {
                    if (currentRow<=4 && currentCol<=6)
                    {
                        currentCol += 1;
                        currentRow -= 1;
                    }
                    else
                    {
                        Debug.Log("Out of bounds");
                    }
                }
                if (Input.GetKey(KeyCode.Alpha2))
                {
                    if (currentRow >=0 && currentCol <= 6)
                    {
                        currentCol += 1;
                        currentRow += 1;
                    }
                    else
                    {
                        Debug.Log("Out of bounds");
                    }
                }

                player.GetComponent<PlayerMovementScript>().canMove = false;
                ntti = Time.time + 1f;
            }

            else
            {
                bar.fillAmount = 0;
            }
            
        }


    }









    [Client]
    void Shoot()
    {

        RaycastHit hit;
        flash.Emit(1);
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, mask))
        {
            if (hit.collider.tag==PlayerTag)
            {
                CmdPlayerShot(hit.collider.name, 10);
            }
            GameObject impactGO=NetworkIdentity.Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, .2f);
        }
    }

    [Command]
    void CmdPlayerShot(string playerID, int damage)
    {
        Debug.Log(playerID + " has been shot");

        Player _player = GameManager.GetPlayer(playerID);
        _player.RpcTakeDamage(damage);
    }



}
