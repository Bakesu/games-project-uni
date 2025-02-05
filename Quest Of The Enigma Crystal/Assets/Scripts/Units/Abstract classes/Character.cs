using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;

    [Header("Attack")]
    [SerializeField] public int attackDistance;
    [SerializeField] public int maxAttackDistance;
    [SerializeField] public float damageDealt;

    [Header("Movement")]
    [SerializeField] public int moveDistance;
    public bool hasMoved = false;

    [Header("Gold")]
    [SerializeField] public int gold;

    [Header("attachedObjects")]
    [SerializeField] public HealthBar healthBar;

    public Animator animator;
    private string currentAnimationState;

    internal virtual void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
    }
    
    internal void ChangeAnimationState(string newState)
    {
        //Stop animation from playing if it is already playing
        //if (currentAnimationState == newState) return;  

        animator.Play(newState);
        currentAnimationState = newState;
    }

    // Returns bool if character is killed
    internal bool TakeDamage(int damage)
    {
        if (currentHealth <= damage)
        {
            currentHealth = 0;
            killCharacter();
            return true;
        }
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
        return false;
    }

    internal void Heal(int healAmount)
    {
        if (currentHealth + healAmount >= maxHealth)
        {
            currentHealth = maxHealth;
            healthBar.setHealth(currentHealth);
        } else
        {
            currentHealth += healAmount;
            healthBar.setHealth(currentHealth);
        }
    }

    internal void killCharacter()
    {
        //play death animation

        //Should maybe just destroy??
        gameObject.SetActive(false);

    }
    public void addCharacter()
    {
    }


}
