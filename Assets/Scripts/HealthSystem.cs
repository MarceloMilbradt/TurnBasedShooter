using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private int healthMax;
    public event EventHandler OnDie;
    public event EventHandler OnDamage;
    private void Awake()
    {
        healthMax = health;
    }
    public void Damage(int damageAmout)
    {
        health -= damageAmout;
        if (health < 0)
            health = 0;
        OnDamage?.Invoke(this, EventArgs.Empty);
        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
    }
    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
