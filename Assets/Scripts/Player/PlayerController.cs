using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpForce;  
    public  float moveInput ;
    public float radius;
    public float distanceToWall; 

    [SerializeField] bool isFacingRight = true;
    public bool isGrounded;
    public bool isTouchingWall; 
    public bool canJump ;
    public bool falling;
  
    public bool canFlip = true;

    public Vector2 ray = new Vector2(1, 1); 

    public Transform pointCheck;

    public LayerMask whatIsGround; 

   public  Rigidbody2D rb;
    Animator anim;
   
    private void Awake()
    {
        anim = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody2D>();
        
        
    }
   private void FixedUpdate()
    {
        
        CheckGround();
       //  CheckFalling(); 
    }
    private void Update()
    {
        AnimationControl();
        Movement();
        CheckFalling(); 
    }
    
    private void AnimationControl()
    {
        anim.SetBool("isGround", isGrounded);
        anim.SetFloat("Speed",Mathf.Abs( rb.velocity.x));
        anim.SetBool("Fall", falling);
        anim.SetBool("Jump", canJump);       
    }

    private void Movement()
    {        
            moveInput = Input.GetAxisRaw("Horizontal") * runSpeed ;
           rb.velocity = new Vector2(moveInput , rb.velocity.y);

      
           // check for flip 
           if (moveInput > 0 && isFacingRight || moveInput < 0 && !isFacingRight)
            FlipPlayer();

          // check for jump 
          //JumpHandle(); 
           Playerjump(); 
          // checkFalling( ) 

           
    }
    void CheckFalling()
    {
        if (rb.velocity.y != 0 && !isGrounded)
        {
            falling = true; 
        }
        else falling = false; 
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(pointCheck.position, radius , whatIsGround); 
    }
   
    private void Playerjump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
           
        }

      if( rb.velocity.y > 0 && !isGrounded)
        {
            canJump = true;  // isjumping
        }
      else
        {
            canJump = false;
        }

       
    }
    void FlipPlayer()
    {
        if (canFlip)
        {
            isFacingRight = !isFacingRight;
            Vector3 transformScale = transform.localScale;
            transformScale.x *= -1;
            transform.localScale = transformScale;
        }
        else return; 
      
      
    }
   

    private void OnDrawGizmos()
    {
      Gizmos.DrawWireSphere(pointCheck.position, radius);
      
    }

}
