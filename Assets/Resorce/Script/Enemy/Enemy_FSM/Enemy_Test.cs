using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Test : MonoBehaviour
{

    public enum States
    {
        IDLE,
        MOVE,
        RUN,
        ATK
    }
    private States _state;
    private FSM _fsm;
    // Start is called before the first frame update
    void Start()
    {
        _state = States.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case States.IDLE:
                //타깃 설정
                //if(무기보다 플레이어가 가까운 경우)
                //{
                //  타깃 : 플레이어
                //  MOVE 전환
                //}
                //타깃 : 무기 
                break;

            case States.MOVE:
                //타깃을 향해 이동
                // if(총을 가진 경우)
                // 플레이어와 장애물이 없으면 ATK 전환
                // 타깃을 향해 이동
                // if(체력이 50이하)
                // RUN 전환
                break;

            case States.ATK:
                //플레이어 공격
                //if(공격 가능 범위 이내이면 공격)
                //
                break;

            case States.RUN:
                //if(범위 안에 플레이어가 있으면)
                //  범위 내의 오브젝트 뒤로 이용
                //if(범위 안에 플레이어가 없으면)
                //치료
                //
                break;
        }

    }
}