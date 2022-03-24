using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

   [SerializeField]  bool activeTimeReset; 

   [SerializeField] float attackRange;
    private float defaulTimeBTWAttack = .2f;
    private float currentTimerCombo;
    private float coolDown; 

    [SerializeField] int comboAttack = 0;
    [SerializeField] int damage = 30;
  
    public bool airAttack = false;
    private bool canShoot; 

    [Header("Bow Attack")]

    [SerializeField] LayerMask enemy;
    [SerializeField] Transform hitBox; 
    Animator anim;
    PlayerController player;

    [SerializeField] GameObject arrow;
    [SerializeField] Transform shootPoint; 
   //   Vector3 hitBoxSize = new Vector3(0.798f, 1.2949f, 1); 
    private void Awake()
    {
        player = GetComponent<PlayerController>(); 
        anim = GetComponent<Animator>(); 
    }
    private void Start()
    {
        currentTimerCombo = defaulTimeBTWAttack; 
    }
    void Update()
    {

        SwordHandleAttack();
        BowAttack(); 
        
        ResetComboState(); 

  
       if(anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerNormalShoot") )
        {
            player.rb.velocity = new Vector2(0, player.rb.velocity.y);
            player.canFlip = false;
            Debug.Log("playernormalshoot"); 
            
        }
      

       else
        {
            player.canFlip = true; 
          
        }
    }
    void SwordHandleAttack()
    {
         if( Input.GetKeyDown(KeyCode.F))
        {
         
           if(! player.isGrounded) //check for onjump and falling to airAtttack 
            {
                anim.SetBool("AirAttack", true); 
            }
           else if(player.isGrounded  )
            {
                anim.SetBool("AirAttack", false); 
            }
         
            if(comboAttack <3)
            {
                Collider2D[] attackEnemies = Physics2D.OverlapCircleAll(hitBox.position, attackRange, enemy); 

                foreach( Collider2D enemy in attackEnemies)
                {
                   // Debug.Log("hit"); 

                    if( enemy.GetComponent<EnemyHealth>().skeletonHealth > 0)
                    {
                        enemy.GetComponent<EnemyHealth>().TakeDamage(damage); 
                    }
                }

                anim.SetBool("SwordAttack", true);
                activeTimeReset = true;
                currentTimerCombo = defaulTimeBTWAttack; 
            }
            else
            {
                anim.SetBool("SwordAttack", false); 
            }
        }
        
            else
            {
                anim.SetBool("AirAttack", false); 
             
            }
        
    }
    private void CoolDownTimer() // check to shoot ( cooldown timer to shoot at the end of combo 3 ) 

    {
        coolDown -= Time.deltaTime; 
        if (coolDown > 0)
            canShoot = false;
        else if (coolDown <= 0) canShoot = true; 
    }
    private void BowAttack( )
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerNomarlShoot"))
            {
            player.rb.velocity = new Vector2(0, player.rb.velocity.y);
        
        }
        CoolDownTimer();
        if( Input.GetKeyDown(KeyCode.G) && canShoot) // check if onAir or not
        {
          
            if( !player.isGrounded)
            {
                anim.SetTrigger("AirShoot");
                coolDown = 1; 
            }
            else // normal shoot 
                  // 
            {
                //  player.rb.velocity = new Vector2(0, player.rb.velocity.y); 
                anim.SetTrigger("Shoot");
             
                coolDown = 1; 
            }
        }
    }
    public void SpawArrow()
    {
        Instantiate(arrow, shootPoint.position, Quaternion.identity);
     
    }
    public void ResetAttack()
    {
        comboAttack = 0; 
    }
    public void IncreaseAttack()
    {
        comboAttack++; 
    }
    public void ResetComboState() // reset  each time at last animations 
    {
        if( activeTimeReset) // Time.deltaTime ; 
        { 
            currentTimerCombo -= Time.deltaTime; 
            if( currentTimerCombo <=0)
            {
                activeTimeReset = false;
                anim.SetBool("SwordAttack", false);
                currentTimerCombo = defaulTimeBTWAttack; 
            }
        }
    }
    public void OnDrawGizmos()
    {
     //   Gizmos.DrawWireSphere(player.pointCheck.position, player.radius);
        //  Gizmos.DrawCube(hitBox.position,new Vector3(0.798f, 1.2949f, 1)) ;
        //  Gizmos.color = Color.blue; 
        Gizmos.DrawWireSphere(hitBox.position, attackRange); 
    }



}
