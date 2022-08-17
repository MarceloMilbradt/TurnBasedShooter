using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health =100;
    public event EventHandler OnDie;
    public void Damage(int damageAmout)
    {
        health -= damageAmout;
        if(health < 0)
            health = 0;

        if(health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
    }
}
