using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Everyone


public abstract class BaseEntity : MonoBehaviour, ILivingEntity, ICombatant
{
    protected virtual void Start()
    {
        maxHealth = 100f;
        currentHealth = maxHealth;
        damage = 5.0f;
        attackSpeed = 1.0f;
        defense = 1.0f;
    }


    protected virtual void Update()
    {
        Mathf.RoundToInt(maxHealth);
        Mathf.RoundToInt(currentHealth);
    }

    public float currentHealth { get; protected set; }

    public float maxHealth { get; protected set; }

    public float defense { get; protected set; }

    public bool isAlive => currentHealth > 0;
    public float damage { get; protected set; }
    public float attackSpeed { get; protected set; }

    public abstract void Attack(ILivingEntity entity);
    public virtual void Death()
    {
        if (!isAlive)
            Destroy(gameObject);
    }
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

}

public abstract class Projectile : MonoBehaviour
{
    public float travelSpeed = 4.0f;

    public Rigidbody rb; 

    public Vector3 direction;
}

