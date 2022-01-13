using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour, IPunObservable
{
    //Max health and current health
    public float MaxHealth = 100f;
    public float Health;
    
    //Check for being alive
    public bool IsAlive = true;

    //Check for being hit and freezing time for it
    private float HitTimer = 0.15f;
    public bool IsHit = false;

    //PhotonView and animator
    public Animator Animator;
    PhotonView view;
    
    //Every player in room 
    GameObject[] Players;

    //Players healthbar
    public GameObject healthbar;

    //Rigidbody for movement
    public Rigidbody2D MyRigidbody2D;

    void Start()
    {
        //Giving the right healthbar for the player
        if(GetComponent<PhotonView>().Owner.IsMasterClient){
        healthbar = GameObject.Find("HealthbarP1");
        }else{
        healthbar = GameObject.Find("HealthbarP2");
        }
        //Setting the health value
        Health = MaxHealth;
        healthbar.GetComponent<HealthBar>().SetMaxHealth(MaxHealth);
        view = GetComponent<PhotonView>();
        MyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Dying if health hits zero
        if(Health <= 0){
            Die();
        }
        //Checking if any player has died and going back to lobby
        Players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject Player in Players){
            if(Player.GetComponent<HealthScript>().IsAlive == false){
                if(PhotonNetwork.IsMasterClient){
                    PhotonNetwork.Destroy(gameObject);
                }
                SceneManager.LoadScene("GameLobby");
            }
        }    
    }


    //Making the player take damage from enemy attack and checking if player is blocking
    [PunRPC]
    public void TakeDamage(float Damage){
        if(!IsHit){
            if(GetComponent<FightingScript>().BlockCheck == true){
                Health -= Damage/2;
                MyRigidbody2D.velocity = new Vector2((GetComponent<MovementScript>().Facing * -1.5f),1f);
            }
            else{
                Health -= Damage;
                StartCoroutine(DamageAnimation());
            }
            healthbar.GetComponent<HealthBar>().SetHealth(Health);
        }
    }

    //Playing taking damage animation if player wasn't blocking
    IEnumerator DamageAnimation(){
        IsHit = true;
        MyRigidbody2D.velocity = new Vector2((GetComponent<MovementScript>().Facing * -2.5f),2.5f);
        Animator.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(HitTimer);
        IsHit = false;
    }


    public virtual void OnPhotonSerializeView(PhotonStream Stream, PhotonMessageInfo Info){
        if(Stream.IsWriting){
            Stream.SendNext(Health);
        }
        else if(Stream.IsReading){
            Health = (float)Stream.ReceiveNext();
        }
    }

    //Playing the dying animation and starting coroutine so players don't immediately go back to lobby
    void Die(){
        Animator.SetTrigger("Die");
        StartCoroutine(Dying());
    }

    //Making sure the dying animation gets played before players are sent back to lobby
    IEnumerator Dying(){
        IsHit = true;
        MyRigidbody2D.velocity = new Vector2(0f,MyRigidbody2D.velocity.y);
        yield return new WaitForSeconds(5f);
        IsAlive = false;
    }
}
