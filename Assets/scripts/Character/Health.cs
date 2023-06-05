using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;
    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void  takeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if(!dead)
        {
            anim.SetTrigger("die");
            GetComponent<playerMove>().enabled = false;
            GetComponent<Weapon>().enabled = false;
            GetComponent<playerCombat>().enabled = false;
            dead = true;
        }
        
    }

    public void AddHealth(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth + _damage, 0, startingHealth);
    }

    public void Respawn()
    {
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");

        GetComponent<playerMove>().enabled = true;
        GetComponent<Weapon>().enabled = true;
        GetComponent<playerCombat>().enabled = true;
        dead = false;
    }
}
