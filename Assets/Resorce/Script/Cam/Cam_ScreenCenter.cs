using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_ScreenCenter : MonoBehaviour
{
    public RaycastHit Aimrayhit;
    public RaycastHit rayhitObject;

    private float range;
    private float maxrange = 100f;
    [Header("Look")]
    [SerializeField]
    private GameObject look; //바라보는 최대 위치를 확인하기위해 사용
    [SerializeField]
    private GameObject test; //에임을 바라보는 위치값을 알기위해 사용


    [Header("Aim")]
    [SerializeField]
    private GameObject aim; //에임의 위치를 set하기 위해 사용


    [Header("Player")]
    [SerializeField]
    private GameObject Player_Head;
    [SerializeField]
    private Transform Player_Eyes;

    private Vector3 ScreenMax;
    private Vector3 ScreenMin;
    private Vector3 ScreenMaxPoint;
    private Vector3 ScreenMinPoint;

    private void Start()
    {
        range = maxrange;
    }
    private void Update()
    {
        //Player Diraction Camera
        //Vector3 PDC = Player_Eyes.transform.position - Camera.main.transform.position;

        //뷰포인트 상에서의 가로세로는 0~1로 정할 수 있는 UV 좌표계를 사용한다.
        //중간 값인 0.5f를 사용했고 월드좌표상으로 카메라와 떨어져있을 거리를 입력하면 
        //화면에서는 항상 중앙에 있지만 거리또한 항상 일정한 좌표가 완성된다.
        //ScreenMin = new Vector3(0.5f, 0.5f, 7f);의 Z값은 3인칭 캠의 가장 멀리서 플레이어를 봤을때
        //ScreenMin의 z값이 카메라와 플레이어사이의 값보다 큰값을 써야한다.
        //그래야 항상 aim이 플레이보다 앞에 있을 수 있다.
        ScreenMin = new Vector3(0.5f, 0.5f, 7f);
        ScreenMax = new Vector3(0.5f, 0.5f, range);


        //문제는 카메라가 움직이면 뷰포인트가 바라보는 월드상의 좌표가 달라지는 데 계속 업데이트 해줘야
        //뷰포인트의 중간점을 갱신하게 되는데 업데이트 하지않고 한번만 호출한게 문제였다.
        //카메라가 바라보는 곳의 중앙
        ScreenMinPoint = Camera.main.ViewportToWorldPoint(ScreenMin);
        ScreenMaxPoint = Camera.main.ViewportToWorldPoint(ScreenMax);

        test.transform.position = ScreenMinPoint;
        //에임의 위치
        look.transform.position = ScreenMaxPoint;

        Vector3 diraction = ScreenMaxPoint - ScreenMinPoint;

        //플레이어가 보는 곳에 물체가 있는 경우
        if (Physics.Raycast(ScreenMinPoint, diraction, out rayhitObject, maxrange))
        {
            aim.transform.position = rayhitObject.point;
        }
        else //ray가 없는 경우 에임의 위치를 변경
        {
            aim.transform.position = ScreenMaxPoint;
            range = maxrange;
        }
        Player_Head.transform.LookAt(aim.transform.position);
        Debug.DrawLine(Camera.main.transform.position, ScreenMaxPoint, Color.yellow);
    }

    public void SetMaxRange(float anotherrange)
    {
        maxrange = anotherrange;
    }
}
