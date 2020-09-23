
using UnityEngine;
using Mirror;


[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] ctd; //components to disable


    [SerializeField]
    string RemoteLayerName = "RemotePlayer";

    Camera sc; //scene camera

    void Start()
    {

        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sc = Camera.main;
            if (sc!=null)
            {
                sc.gameObject.SetActive(false);
            }
        }

        GetComponent<Player>().Setup();


    } 

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(netID, _player);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(RemoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < ctd.Length; i++)
        {
            ctd[i].enabled = false;
        }
    }

    void OnDisable()
    {
        if (sc!=null)
        {
            sc.gameObject.SetActive(true);
        }
        GameManager.UnregisterPlayer(transform.name);
    }

}
