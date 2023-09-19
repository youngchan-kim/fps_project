using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ShootingAi : HealthBar
{
    public GameObject healthBar;
    //public GameObject Player;
    Vector3 v = new Vector3();
    Vector3 w = new Vector3();
    Vector3 TargetDir = new Vector3();
    Vector3 PlayerDir = new Vector3();
    public float ViewAngle;
    float dot;
    float playerdot;
    GameObject target;
    private void Start()
    {
        //healthBar.SetMaxHealth(health);
        //ViewAngle = (180 -Camera.main.fieldOfView);
        target = transform.GetChild(0).gameObject;
        //thisHpBar = GameObject.Find("GameUI/")
    }
    void Update()
    {
        //Camera.main.transform.forward = Player.transform.forward;
        ViewAngle = 180 - Camera.main.fieldOfView;
        //플레이어로 향하는 target의 벡터
        TargetDir = (Camera.main.transform.position - transform.position).normalized;

        //기준이 되는 
        dot = Vector3.Angle(Camera.main.transform.forward, TargetDir);
        //playerdot = Vector3.Angle(transform.forward, PlayerDir);
        Debug.Log("카메라 Angle : " + ViewAngle);
        Debug.Log("Angle : " +dot);
        //Debug.Log("Angle : " + playerdot);

        if (dot > ViewAngle)
        {
             if (healthBar.activeSelf == false)
             {
                 //thisHpBar.gameObject.SetActive(true);
                 //target.SetActive(true);
             }
             //thisHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
             Debug.Log("적이 시야에 있습니다.");
        }
        else {
            if (healthBar.activeSelf == true)
            {
                healthBar.gameObject.SetActive(false);
                //target.SetActive(false);

            }
            Debug.Log("적이 시야에 없습니다.");
        }
        
        //적이 자신의 앞인지 뒤인지을 체크
        //if (Player_from_Angle()) thisHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));

    }

    public void TakeDamage(int damage)
    {
        //health -= damage;
        //healthBar.GetComponent<Slider>().maxValue = health;
        /*if (health <= 0)
        {
            healthBar.gameObject.SetActive(false);
            Destroy(gameObject);
        }*/

    }

}
