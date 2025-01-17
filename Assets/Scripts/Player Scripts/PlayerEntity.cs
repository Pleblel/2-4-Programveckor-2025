using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : BaseEntity
{
    public float playerDamage;


    protected override void Start()
    {
        maxHealth = 100f;
        currentHealth = maxHealth;
        damage = 40.0f;
        attackSpeed = 1.0f;
        defense = 1.0f;

        playerDamage = damage;
    }


    protected override void Update()
    {
        Mathf.RoundToInt(maxHealth);
        Mathf.RoundToInt(currentHealth);
    }

    public override void Attack(ILivingEntity entity)
    {
      
    }


    public override void Death()
    {
        if (!isAlive)
            Destroy(gameObject);
    }
    public override void TakeDamage(float damage)
    {
        currentHealth -= damage / defense;
    }
}
