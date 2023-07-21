using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_ScreenCenter : MonoBehaviour
{
    public RaycastHit Aimrayhit;

    Ray ray;
    private Vector3 ray_front;

    private Vector3 ScreenCenterpoint;
    private Vector3 tamp;

    private Vector3 aimpoint;

    private float maxrange = 5f;
    [Header("Aim")]
    [SerializeField]
    private GameObject aim;

    [Header("Player")]
    [SerializeField]
    private GameObject Player_Eyes;

    private Vector3 ScreenCenter;
    private Vector3 ScreenMax;
    private Vector3 ScreenMin;
    private Vector3 ScreenMaxPoint;
    private Vector3 ScreenMinPoint;
    private Vector3 ScreenCenterpos;


    private void Update()
    {
        //뷰포인트 상에서의 가로세로는 0~1로 정할 수 있는 UV 좌표계를 사용한다.
        //중간 값인 0.5f를 사용했고 월드좌표상으로 카메라와 떨어져있을 거리를 입력하면 
        //화면에서는 항상 중앙에 있지만 거리또한 항상 일정한 좌표가 완성된다.
        ScreenMax = new Vector3(0.5f, 0.5f, 100.0f);
        //문제는 카메라가 움직이면 뷰포인트가 바라보는 월드상의 좌표가 달라지는 데 계속 업데이트 해줘야
        //뷰포인트의 중간점을 갱신하게 되는데 업데이트 하지않고 한번만 호출한게 문제였다.
        //카메라가 바라보는 곳의 중앙
        ScreenMaxPoint = Camera.main.ViewportToWorldPoint(ScreenMax);
        ScreenCenter = ScreenMaxPoint - ScreenMinPoint;

        //에임의 위치
        aim.transform.position = ScreenMaxPoint;

        ScreenCenter = Camera.main.transform.forward * 100f;
        Player_Eyes.transform.rotation = Camera.main.transform.rotation;

        
        Debug.DrawLine(Player_Eyes.transform.position, aim.transform.position, Color.blue);
        Debug.DrawRay(Camera.main.transform.position, ScreenCenter, Color.yellow);
    }
    //ray_front

    public void SetMaxRange(float range)
    {
        maxrange = range;
    }
}
