using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    //Every player in room
    private GameObject[] Players;

    //Walls for camera so players don't go out of sight
    public GameObject Wall1, Wall2;

    //Setting players to target group and wall positions
    void SetCameraPos() {
        //Getting every player by tag
        Players = GameObject.FindGameObjectsWithTag("Player");
        if(Players != null){
            foreach(GameObject Player in Players){
                //Seeing if player is already in target group and putting them if they are not
                if(GetComponent<CinemachineTargetGroup>().FindMember(Player.transform) == -1){
                    GetComponent<CinemachineTargetGroup>().AddMember(Player.transform, 1f, 0f);
                }
            }        
        }
        //Updating wall position
        Wall1.transform.position = new Vector3(
            transform.position.x - 17.5f,
            transform.position.y,
            transform.position.z
        );
        Wall2.transform.position = new Vector3(
            transform.position.x + 17.5f,
            transform.position.y,
            transform.position.z
        );
    }
    void FixedUpdate()
    {
        SetCameraPos();
    }
}
