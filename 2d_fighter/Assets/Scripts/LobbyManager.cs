using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //List for playeritems (players) in lobby
    public List<PlayerItem> PlayerItemsList = new List<PlayerItem>();

    //Portrait of players currently selected character
    public PlayerItem PlayerItemPrefab;

    //Place to put all playeritems
    public Transform PlayerItemParent;

    //Play button for master client
    public GameObject PlayButton;

    void Start(){
        UpdatePlayerList();
    }

    //Clears and puts new playeritems when the list is updated
    void UpdatePlayerList(){  
        foreach(PlayerItem item in PlayerItemsList){
            Destroy(item.gameObject);
        }
        PlayerItemsList.Clear();
        if(PhotonNetwork.CurrentRoom == null){
            return;
        }
        foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players){
            PlayerItem newPlayerItem = Instantiate(PlayerItemPrefab, PlayerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            if(player.Value != PhotonNetwork.LocalPlayer){
                newPlayerItem.ApplyLocalChanges();
            }
            PlayerItemsList.Add(newPlayerItem);
        }
    }

    //Update list when player joins room
    public override void OnPlayerEnteredRoom(Player newPlayer){
        UpdatePlayerList();
    }

    //Update list when players leave room
    public override void OnPlayerLeftRoom(Player newPlayer){
        UpdatePlayerList();
    }

    //Leave room
    public void OnClickLeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    //Connect back to server when leaving a room
    public override void OnLeftRoom(){
        SceneManager.LoadScene("Loading");
    }

    void Update()
    {
        //Giving the master client the play button when the room is full
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2){
            PlayButton.SetActive(true);
        }
        else{
            PlayButton.SetActive(false);
        }
    }

    //Load the game when master client clicks play
    public void OnClickPlayButton(){
        PhotonNetwork.LoadLevel("ForestLevel");
    }
}
