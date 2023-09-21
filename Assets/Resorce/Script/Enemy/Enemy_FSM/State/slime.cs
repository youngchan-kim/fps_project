using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime : Enemy_Test
{
    private enum State
    {
        IDLE,
        MOVE,
        RUN,
        ATK
    }

    private State _currentState;
    private FSM _fsm;

    private void Start()
    {
        _currentState = State.IDLE;
        _fsm = new FSM(new IDLE_State(this));
    }

    private void Update()
    {

        switch (_currentState)
        {
            case State.IDLE:
                if(CanSeePlayer())
                {
                    if (CanAtkPlayer())
                        _fsm.ChangeState(State.ATK);
                }
                //타깃 설정
                //if(무기보다 플레이어가 가까운 경우)
                //{
                //  타깃 : 플레이어
                //  MOVE 전환
                //}
                //타깃 : 무기 
                break;

            case State.MOVE:
                //타깃을 향해 이동
                // if(총을 가진 경우)
                // 플레이어와 장애물이 없으면 ATK 전환
                // 타깃을 향해 이동
                // if(체력이 50이하)
                // RUN 전환
                break;

            case State.ATK:
                //플레이어 공격
                //if(공격 가능 범위 이내이면 공격)
                //
                break;

            case State.RUN:
                //if(범위 안에 플레이어가 있으면)
                //  범위 내의 오브젝트 뒤로 이용
                //if(범위 안에 플레이어가 없으면)
                //치료
                //
                break;
        }

    }

    bool CanSeePlayer() { return 0; }


    bool CanAtkPlayer() { return 0; }

}