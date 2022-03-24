using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   public float skeletonHealth ;
    
    Animator anim; 
    void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
    }
 
    public void TakeDamage( int damage)
    {
        skeletonHealth -= damage;
        if (skeletonHealth <= 0)
        {
            anim.SetBool("Death", true);
           
        }
        else
        {
            anim.SetTrigger("Hit");
          //  GetComponent<EnemySkeleton>().rb.AddForce( Vector2.up , ForceMode2D.Impulse ); 
        }
    }
}
