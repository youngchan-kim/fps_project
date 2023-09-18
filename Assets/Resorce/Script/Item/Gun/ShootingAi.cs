using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingAi : Enemy
{
    public GameObject thisHpBar;

    private void Start()
    {
        //thisHpBar = GameObject.Find("GameUI/")
    }
    void Update()
    {
        //적이 자신의 앞인지 뒤인지을 체크
       thisHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,1.5f,0));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        thisHpBar.GetComponent<Slider>().maxValue = health;
        if (health <= 0)
        {
            thisHpBar.gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }

}
