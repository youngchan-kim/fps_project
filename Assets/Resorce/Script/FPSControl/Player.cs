using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;

    public GameObject playercam;
    public GameObject Inventory;
    private bool mode_chage;


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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playercam.gameObject.SetActive(mode_chage);
            Inventory.SetActive(!mode_chage);
            mode_chage = !mode_chage;  
        }

        if (Getlife())
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

        CuserControl(mode_chage);
    }
    void CuserControl(bool mode_chage)
    {
        //커서를 움직이지 않도록 하고 보이지 않도록 해야함.
        if (mode_chage == false)
        { 
            Cursor.lockState = CursorLockMode.Locked; 
        }

        else
        { 
            Cursor.lockState = CursorLockMode.Confined; 
        }
        Cursor.visible = mode_chage;
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
