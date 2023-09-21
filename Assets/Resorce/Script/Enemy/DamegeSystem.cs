using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamegeSystem : MonoBehaviour
{
    // Start is called before the first frame update
    //최대체력 100을 가짐
    public float health = 100;
    public GameObject HpBar;
    public void Start()
    {

        HpBar.GetComponent<HPBar>().SetMaxHealth(health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        HpBar.GetComponent<HPBar>().SetHealth(health);
        if (health <= 0)
        {
            HpBar.SetActive(false);
            Destroy(gameObject);
        }

    }
}
