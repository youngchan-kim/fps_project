using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_ScreenCenter : MonoBehaviour
{
    public RaycastHit Aimrayhit;
    private Vector3 ray_front;

    private Vector3 ScreenCenterpoint;

    private Vector3 aimpoint;

    private float maxrange = 5f;
    [Header("Aim")]
    [SerializeField]
    private GameObject aim;

    [Header("Player")]
    [SerializeField]
    private Transform Player_position;
    [SerializeField]
    private Transform Player_Eyes;

    //카메라에서 플레이어 까지의 벡터값 (크기와 방향)
    private Vector3 Camera_btw_player_tune;

    //뷰포인트상의 벡터값
    private Vector3 ViewPointMin;
    private Vector3 ViewPointMax;

    //월드 포인터로 뷰포인터의 벡터값을 변환된 값
    private Vector3 WorldPointMinPoint;
    private Vector3 WorldPointMaxPoint;



    private void Update()
    {

        Camera_btw_player_tune = Player_position.position - Camera.main.transform.position;

        //뷰포인트 상에서의 가로세로는 0~1로 정할 수 있는 UV 좌표계를 사용한다.
        //중간 값인 0.5f를 사용했고 월드좌표상으로 카메라와 떨어져있을 거리를 입력하면 
        //화면에서는 항상 중앙에 있지만 거리또한 항상 일정한 좌표가 완성된다.
        ViewPointMin = new Vector3(0.5f, 0.5f, Camera_btw_player_tune.z * 1.1f);
        ViewPointMax = new Vector3(0.5f, 0.5f, 100.0f);
        //문제는 카메라가 움직이면 뷰포인트가 바라보는 월드상의 좌표가 달라지는 데 계속 업데이트 해줘야
        //뷰포인트의 중간점을 갱신하게 되는데 업데이트 하지않고 한번만 호출한게 문제였다.
        //카메라가 바라보는 곳의 중앙
        WorldPointMinPoint = Camera.main.ViewportToWorldPoint(ViewPointMin);
        WorldPointMaxPoint = Camera.main.ViewportToWorldPoint(ViewPointMax);

        //위치에서 방향으로 레이가 맞는 물체의 정보를 range만큼 쏠때 충돌이 있다면 ture반환
/*        if (Physics.Raycast(WorldPointMinPoint, WorldPointMaxPoint, out ))
        else //충돌이 없는 경우
        {
            //기본적으로 가장 멀리 위치해야함
            aim.transform.position = WorldPointMaxPoint;
        }
*/
        //플레이어가 에임을 바라본다.
        Player_Eyes.LookAt(aim.transform.position);
        //플레이어의 눈 위치에서 에임까지의 선
        Debug.DrawLine(Player_Eyes.transform.position, aim.transform.position, Color.blue);
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.yellow);
    }
    //ray_front

    public void SetMaxRange(float range)
    {
        maxrange = range;
    }
}
