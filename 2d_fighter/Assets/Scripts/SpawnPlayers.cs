using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


//Spawns players at the start of the game
public class SpawnPlayers : MonoBehaviour
{
    //List of all playable characters (must be in same order as player avatars in PlayerItem)
    public GameObject[] PlayerPrefabs;

    //Spawn positions for both players
    public float PosY = -3f;
    public float Player1Pos = -13f;
    public float Player2Pos = 13f;

    //Final spawn position for both players
    private Vector2 SpawnPos;

    //Selected character
    public string PrefabName;

    void Start()
    {
        Spawn();
    }

    //Checks if player has selected prefab and spawns player on the right spot based on if player is masterclient
    void Spawn(){
        if(PhotonNetwork.LocalPlayer.CustomProperties["PlayerAvatar"] != null){
            PrefabName = PlayerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerAvatar"]].name;
        }
        else{
            PrefabName = "Man";
        }
        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            SpawnPos = new Vector2(Player1Pos, PosY);
        }
        else{
            SpawnPos = new Vector2(Player2Pos, PosY);
        }
        PhotonNetwork.Instantiate(PrefabName, SpawnPos, Quaternion.identity);
    }
}
