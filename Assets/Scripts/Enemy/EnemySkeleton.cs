using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
    [Header("enemy movement")]
    [SerializeField] float distanceMove;
    public float maxDis;
    public float minDis;
    [SerializeField] float EnemyMoveSpeed;

    private bool patrol;
    private bool detect;
    private bool canFlip = true; 

    [Header("Attack ")]
    [SerializeField] Transform attackPos;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float attackRange;
    [SerializeField] float detectRange; 
    [SerializeField]  int damage; 

    [SerializeField] int destination =1 ; 

    public Rigidbody2D rb;
    Transform playerPos;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody2D>(); 
        maxDis = transform.position.x + distanceMove / 2;
        minDis = transform.position.x - distanceMove / 2;
        playerPos = GameObject.Find("Player").transform; 
    }

    private void Update()
    {
        CheckToFlip(); 
        if (Vector2.Distance(transform.position, playerPos.position) < 4f) patrol = false;
        else patrol = true;
      // exeptions case 
      if(playerPos.position ==transform.position &&  !canFlip)
        {
            transform.position = Vector2.zero; 
        }
    }
    void FixedUpdate()
    {
        SkeletonMove();
       
        if(anim.GetBool("Death"))
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            return; 
        }
    }
   private void SkeletonMove()
    {
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            anim.SetBool("Attack", false); 
        }
        else if (rb.velocity.x < 0) transform.localScale = new Vector2(-1, transform.localScale.y);

        if (anim.GetBool("Detect")) rb.velocity = Vector2.zero; 
      

        if (patrol)
        {
            detect = false; 

            switch (destination)
            {
                case 1:
                    if (transform.position.x < maxDis)
                    {
                        rb.velocity = new Vector2(EnemyMoveSpeed, rb.velocity.y);
                    }
                    else
                    {
                        destination = -1;
                    }
                    break;
                case -1:
                    if (transform.position.x > minDis)
                    {
                        rb.velocity = new Vector2(-EnemyMoveSpeed, rb.velocity.y);
                    }
                    else destination = 1;
                    break;
            }
        }


        else
        {
            if( Vector2.Distance(transform.position , playerPos.position ) >=  detectRange)
            {
               if(!detect)

                {
                    detect = true;
                    anim.SetTrigger("Detect");
                    // rb.velocity = new Vector2(0, rb.velocity.y); 
                }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Detect")) return;

                Vector3 playerDir = (playerPos.position - transform.position).normalized;

                if (playerDir.x > 0)
                    rb.velocity = new Vector2(EnemyMoveSpeed + 0.7f, rb.velocity.y); 
                else // playerDir.x <=0
                {
                    rb.velocity = new Vector2(-(EnemyMoveSpeed + 0.7f), rb.velocity.y);
                }               
            }
            if (Vector2.Distance(playerPos.position, transform.position) < 1.5f) // within attack range 
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.SetBool("Attack", true);
            }
            else anim.SetBool("Attack", false); 
        }
    }
    private void CheckToFlip()
    {
        if ( transform.position == playerPos.position)
        {
            canFlip = false; 
        }
        else
        {
            canFlip = true; 
        }
    }
    public void AttackPlayer()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        
        Collider2D attackPlayer = Physics2D.OverlapCircle(attackPos.position, attackRange, playerLayer); 
        if(attackPlayer !=null)
        {
            if( attackPlayer.tag == "Player")
            {
               attackPlayer.gameObject.GetComponent<PlayerHealth>().PlayerTakeDamage(damage); 
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRange); 
        Gizmos.DrawWireSphere(transform.position, detectRange); 

    }
}
