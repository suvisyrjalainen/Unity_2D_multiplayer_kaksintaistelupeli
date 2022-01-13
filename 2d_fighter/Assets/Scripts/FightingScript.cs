using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FightingScript : MonoBehaviour
{

    //PhotonView and animator
    PhotonView view;
    public Animator Animator;

    //Block check for taking damage
    public bool BlockCheck = false;

    //Variable for randomly choosing the animations
    private int Chooser;

    //Attack cooldown time and timer
    public float Cooldown = 0.25f;
    private float CooldownTimer;

    //Booleans for the player to check if they have made an attack and got a hit (Hit is to make sure attacks don't make damage for both body and legs)
    private bool Attacking = false;
    private bool Hit = false;

    //Attack areas for punching and kicking and the range for those attacks
    public Transform PunchCheck;
    public Transform KickCheck;
    public float Range = 1.75f;

    //Layers to check when attacking
    public LayerMask EnemyLayer;

    //Damage for different attacks
    public float PunchDamage = 2f;
    public float KickDamage = 3f;

    void Start(){
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        //Checking if someone attacked the player
        bool IsHit = GetComponent<HealthScript>().IsHit;
        if(view.IsMine && IsHit == false){
            if(!BlockCheck && !Attacking && CooldownTimer <= 0){
                if(Input.GetButtonDown("Fire1")){
                    Punch();
                }
                if(Input.GetButtonDown("Fire2")){
                    Kick();
                }
            }
            //Cooldown for attacking
            if(Attacking){
                if(CooldownTimer > 0){
                    CooldownTimer -= Time.deltaTime;
                }
                else{
                    Attacking = false;
                }
            }
            //Starting and ending blocking
            if(Input.GetButtonDown("Fire3")){
                Block();
            }
            if(Input.GetButtonUp("Fire3")){
                BlockEnd();
            }
        }
    }

    //Playing one of two punch animations and calling the damage making function
    void Punch(){
        Chooser = Random.Range(0,2);
        if(Chooser == 1){
            Animator.SetTrigger("Punch1");
        }
        else{
            Animator.SetTrigger("Punch2");
        }
        Attack(PunchCheck, PunchDamage);
    }

    //Playing one of two kick animations and calling the damage making function
    void Kick(){
        Chooser = Random.Range(0,2);
        if(Chooser == 1){
            Animator.SetTrigger("Kick1");
        }
        else{
            Animator.SetTrigger("Kick2");
        }
        Attack(KickCheck, KickDamage);
    }

    //Making the enemy take damage and updating it to everyone
    void Attack(Transform Check, float Damage){
        Collider2D[] EnemyHit = Physics2D.OverlapCircleAll(Check.position, Range, EnemyLayer);
        if(EnemyHit != null){
            foreach(Collider2D Enemy in EnemyHit){
                if(Hit == false){
                    if(Enemy.gameObject != this.gameObject){
                        Enemy.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, Damage);
                        Hit = true;
                    }
                }
            }
            Hit = false;
        }
        Attacking = true;
        CooldownTimer = Cooldown;
    }

    //Starting the block animation and setting BlockCheck as true
    void Block(){
        Animator.SetTrigger("BlockTrigger");
        Animator.SetBool("Block", true);
        BlockCheck = true;
    }
    //Ending the block animation and setting BlockCheck as false
    void BlockEnd(){
        Animator.SetBool("Block", false);
        BlockCheck = false;
    }

    void OnDrawGizmosSelected(){
        if(PunchCheck == null)
        return;
        Gizmos.DrawWireSphere(PunchCheck.position, Range);
        Gizmos.DrawWireSphere(KickCheck.position, Range);  
    }

}
