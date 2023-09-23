using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 1f;
    public float health = 1f;
    [Tooltip("Method to be run upon health reaching 0")]
    public UnityEvent onDeath;

    public void Start()
    {
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    /// <summary>
    /// Heals the specified amount of health
    /// </summary>
    /// <param name="amount">The amount of health to heal; must be a positive number</param>
    /// <param name="allowOverheal">If true, this is allowed to make health exceed its maximum value</param>
    public void Heal(float amount, bool allowOverheal)
    {
        if (amount > 0)
        {
            // Change amount of health
            if (allowOverheal)
            {
                health += amount;
            }
            else
            {
                health = Mathf.Clamp(health + amount, 0, maxHealth);
            }
        }
    }

    /// <summary>
    /// Deals the specified amount of damage, and invokes the object's method for dying if health is 0 or less
    /// </summary>
    /// <param name="amount">The amount of health to take away; must be a positive number</param>
    /// <returns>true if the object still has health left, false if the object has just died</returns>
    public bool Hurt(float amount)
    {
        if (amount > 0)
        {
            // Change amount of health
            health -= amount;
            //Debug.Log($"{gameObject.name} has taken {amount} damage! New health: {health}");
            
            // If the object has no health left, return false and run the appropriate death method
            if (health <= 0.001f)
            {
                //Debug.Log($"{gameObject.name} has died!");
                onDeath.Invoke();
                return false;
            }
        }

        // If still alive, return true
        return true;
    }
}
