using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy 
{
    public int health;
    public int attackType;
    public int attackDamage;
    public int attackRange;
    public float attackFrequency;

    public bool bossEnemy;
    
    public Enemy()
    {
        health = 1;
        attackDamage = 1;
        attackRange = 1;
        attackFrequency = 1f;
    }

    public Enemy(int health, int attackType, int attackDamage, int attackRange)
    {
        this.health = health;
        this.attackType = attackType;
        this.attackDamage = attackDamage;
        this.attackRange = attackRange;
        setAttackFrequency();
    }

    public Enemy(int health, int attackType, int attackDamage, int attackRange, bool staticEnemy = false)
    {
        this.health = health;
        this.attackType = attackType;
        this.attackDamage = attackDamage;
        this.attackRange = attackRange;
        setAttackFrequency();
        this.bossEnemy = staticEnemy;
    }

    private void setAttackFrequency()
    {
        switch(attackType)
        {
            case Constants.ATTACK_TYPE_MELEE:
                attackFrequency = Random.Range(1f, 1.5f);
                break;
            case Constants.ATTACK_TYPE_PROJECTILE:
                attackFrequency = Random.Range(1.5f, 2f);
                break;
            case Constants.ATTACK_TYPE_MAGIC:
                attackFrequency = Random.Range(2f, 2.5f);
                break;
        }
    }

}
