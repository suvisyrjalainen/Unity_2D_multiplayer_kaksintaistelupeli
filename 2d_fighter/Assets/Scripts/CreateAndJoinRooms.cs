using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{

    //Lobby name for private lobby
    public InputField LobbyName;

    //Creating a room private/public depending on if lobby name has been set
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.BroadcastPropsChangeToAll = true;
        
        if(LobbyName != null){
            roomOptions.IsVisible = false;
            PhotonNetwork.CreateRoom(LobbyName.text, roomOptions, null);
        }
        else{
            PhotonNetwork.CreateRoom(null, roomOptions, null);
        }
    }

    //Joining a random public room
    public void QuickMatch()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //Creating a room if player didn't find a room and making the lobby name null so the room doesn't go private
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        LobbyName.text = null;
        CreateRoom();
    }

    //Joining a private room
    public void JoinRoom(){
        PhotonNetwork.JoinRoom(LobbyName.text);
    }

    //If private room wasn't created yet create it
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    //After finding a room go to the game lobby
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameLobby");
    }
}
