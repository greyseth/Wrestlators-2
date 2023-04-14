using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;

    EnemyBasicMovement movement;
    float health;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        movement = GetComponent<EnemyBasicMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount, bool knock)
    {
        health -= amount;
        if (knock) movement.Knock(health <= 0 ? true : false);
        else movement.Stun();
    }
}
