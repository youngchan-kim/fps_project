using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Visible_HPbar : MonoBehaviour
{
    public GameObject HpBar;
    Vector3 TargetDir = new Vector3();
    public float ViewAngle;
    float dot;

    void Update()
    {
        //Camera.main.transform.forward = Player.transform.forward;
        ViewAngle = 180 - Camera.main.fieldOfView;
        //플레이어로 향하는 target의 벡터
        TargetDir = (Camera.main.transform.position - transform.position).normalized;

        //기준이 되는 
        dot = Vector3.Angle(Camera.main.transform.forward, TargetDir);

        // Debug.Log("카메라 Angle : " + ViewAngle);
        // Debug.Log("Angle : " +dot);

        if (dot > ViewAngle)
        {
            if (HpBar.activeSelf == false)
            {
                HpBar.SetActive(true);
            }
            HpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
            Debug.Log("적이 시야에 있습니다.");
        }
        else
        {
            if (HpBar.activeSelf == true)
            {
                HpBar.SetActive(false);

            }
            Debug.Log("적이 시야에 없습니다.");
        }
    }
}
