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
        attackFrequency = 1f;
    }

    public Enemy(int health, int attackType, int attackDamage, int attackRange, bool staticEnemy = false)
    {
        this.health = health;
        this.attackType = attackType;
        this.attackDamage = attackDamage;
        this.attackRange = attackRange;
        attackFrequency = 1f;
        this.bossEnemy = staticEnemy;
    }

}
