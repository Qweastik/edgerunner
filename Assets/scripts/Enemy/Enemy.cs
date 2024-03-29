using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    

    public int maxHealth = 1;
    int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        anim.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }

        void Die()
        {
            anim.SetBool("isDead", true);

            GetComponent<Collider2D>().enabled = false;

            this.enabled = false;
        }
    }

}
