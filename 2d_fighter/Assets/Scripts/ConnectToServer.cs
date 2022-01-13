using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    //Making sure player is connecting to closest region and synchronizing scenes
    void Start()
    {
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    //Joining a lobby when player is connected to servers
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    //Switching the scene to lobby
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
