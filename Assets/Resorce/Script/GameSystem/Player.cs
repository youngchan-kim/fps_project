using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;

    private bool life;
    // Start is called before the first frame update
    void Start()
    {
        life = true;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Getlife())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(15);
            }
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                life = false;
            }
        }
    }
    
    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public bool Getlife()
    {
        return life;
    }
}
