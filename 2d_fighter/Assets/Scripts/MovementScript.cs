using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovementScript : MonoBehaviour
{
    //Walking speed and jumping force
    public float Speed = 5f;
    public float JumpForce = 7.5f;

    //Direction wich you are walking and looking
    private float HorizontalMovement = 0f;
    public int Facing = 1;

    //Rigidbody for movement
    public Rigidbody2D MyRigidbody2D;

    //PhotonView and animator
    public Animator Animator;
    PhotonView view;

    //Your lower collider and list of every player in room
    public CircleCollider2D Feet;
    private GameObject[] Players;


    void Start(){
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        //Checking if someone attacked the player
        bool IsHit = GetComponent<HealthScript>().IsHit;
        if(view.IsMine && IsHit == false){
            //Making the player look at the other player
            if(Players != null){
                foreach(GameObject Player in Players){
                    if(Player.gameObject != this.gameObject){
                        if(transform.position.x <= Player.transform.position.x){
                            Flip(1);
                        }
                        else{
                            Flip(-1);
                        }
                    }
                }
            }
            //Moving direction
            HorizontalMovement = Input.GetAxis("Horizontal");
            //Jumping
            if(Input.GetButtonDown("Jump")&& Feet.IsTouchingLayers(LayerMask.GetMask("Ground"))){
                MyRigidbody2D.AddForce(new Vector2(0f,JumpForce), ForceMode2D.Impulse);
                Animator.SetTrigger("Jump");
            }
            //Checking if player is touching ground and telling it to the animator
            if(Feet.IsTouchingLayers(LayerMask.GetMask("Ground"))){
                Animator.SetBool("IsTouchingGround", true);
            }
            else{
                Animator.SetBool("IsTouchingGround", false);
            }
        }
    }

    void FixedUpdate()
    {
        bool IsHit = GetComponent<HealthScript>().IsHit;
        if(view.IsMine && IsHit == false){
            //Making the player move and giving the info to animator
            MyRigidbody2D.velocity = new Vector2(HorizontalMovement * Speed, MyRigidbody2D.velocity.y);
            Animator.SetFloat("Speed", Mathf.Abs(HorizontalMovement));
        }
    }

    //Flip player
    public void Flip(int x)
    {
        transform.localScale = new Vector2(x,transform.localScale.y);
        Facing = x;
    }
}
