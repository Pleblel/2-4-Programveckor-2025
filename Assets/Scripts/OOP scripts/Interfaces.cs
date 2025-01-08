using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Everyone


public interface ILivingEntity
{
    void TakeDamage(float damage);

    void Death();

    bool isAlive { get; }
}


public interface ICombatant
{
    void Attack(ILivingEntity entity);

    float damage { get; }

    float attackSpeed { get; }
}


public interface IInteractables
{
    void Function();
}


public interface IDamageAble
{
    void Damage(BaseEntity entity);
}

public interface IDamageAbleOvertime
{
    IEnumerator PassiveDamage(BaseEntity entity);

    float damageTimer { get; }
}

public interface IArmor : IEquipable
{
    float defenseBonus { get; }
}


public interface IWeapon : IEquipable
{
    float damageBonus { get; }

    float attackSpeedBonus { get; }
}

public interface IEquipable
{
    void Equip();
    void UnEquip();
}


public interface IMovable
{
    float movementSpeed { get; }
    void Move(Vector3 direction);
}


