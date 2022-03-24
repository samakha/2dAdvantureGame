using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage; 
    private bool right; 
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player").transform.localScale.x > 0)
        {
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
            right = true; 
        }
        else
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            right = false; 
        }
        Destroy(gameObject, 3); 
    }


    // Update is called once per frame
    void Update()
    {
        if (right) transform.Translate(Vector2.right * speed);
        else transform.Translate(Vector2.left * speed); 
    }
    private void OnTriggerEnter2D(Collider2D target)
    {
        if( target.tag=="Enemy")
        {
            Destroy(gameObject); 
            target.GetComponent<EnemyHealth>().TakeDamage(damage); 
        }
    }
}
