using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;

    Animator anim; 

    private void Awake()
    {
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame   
   
    public void PlayerTakeDamage( int damage)
    {
        playerHealth -= damage; 
          if( playerHealth <0)
        {
            anim.SetBool("Death", true);    
        }
    }
}
