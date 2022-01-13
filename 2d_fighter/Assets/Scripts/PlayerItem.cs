using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    //Buttons for changing avatar
    public GameObject NextButton;
    public GameObject PreviousButton;
    //Player avatar and list of all avatars
    public SpriteRenderer PlayerAvatar;
    public Sprite[] Avatars;

    //Players custom properties (avatar and name)
    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();
    
    Player Player;

    //Hide your buttons for all other players
    public void ApplyLocalChanges(){
        PreviousButton.SetActive(false);
        NextButton.SetActive(false);
    }

    public void SetPlayerInfo(Player _player){
        Player = _player;
        UpdatePlayerItem(Player);
    }

    //Changes your avatar for a previous one
    public void OnClickPreviousButton(){
        if((int)PlayerProperties["PlayerAvatar"] == 0){
            PlayerProperties["PlayerAvatar"] = Avatars.Length - 1;
        }
        else{
            PlayerProperties["PlayerAvatar"] = (int)PlayerProperties["PlayerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(PlayerProperties);
    }

    //Changes your avatar to the next one
    public void OnClickNextButton(){
        if((int)PlayerProperties["PlayerAvatar"] == Avatars.Length - 1){
            PlayerProperties["PlayerAvatar"] = 0;
        }
        else{
            PlayerProperties["PlayerAvatar"] = (int)PlayerProperties["PlayerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(PlayerProperties);
    }

    //Applies the player property changes for everyone
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable PlayerProperties){
        if(Player == targetPlayer){
            UpdatePlayerItem(targetPlayer);
        }
    }

    //Makes the player property changes visible
    void UpdatePlayerItem(Player Player){
        if(Player.CustomProperties.ContainsKey("PlayerAvatar")){
            PlayerAvatar.sprite = Avatars[(int)Player.CustomProperties["PlayerAvatar"]];
            PlayerProperties["PlayerAvatar"] = (int)Player.CustomProperties["PlayerAvatar"];
        }
        else{
            PlayerProperties["PlayerAvatar"] = 0;
        }
    }
}