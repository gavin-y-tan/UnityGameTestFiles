using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private const string PLAYER_ID_PREFIX = "Player ";
    
    private static Dictionary<string, Player> players=new Dictionary<string, Player>();


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }
    }

    public static void RegisterPlayer(string netID, Player _player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(playerID, _player);
        _player.transform.name = playerID;
    }




    public static void UnregisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }

}
