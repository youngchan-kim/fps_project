using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//추상클래스 : 이런 클래스가 있을꺼야
//추상 클래스(메소드)
// 추상클래스를 상속받은 자식이 반드시 오버라이딩 하여 
// 추상메소드를 구형시키게하기위함
public abstract class BaseState
{
    //이 추상클래스에는 Enemy Class 변수가 있을꺼야
    protected Enemy_Test _enemy;

    //Enemy Class를 변경하기 위한 BaseState함수가 있을꺼야
    protected BaseState(Enemy_Test enemy)
    {
        _enemy = enemy;
    }
    //새로운 스테이트가 실행할 때 필요한함수와
    //스테이트의 업데이트를 위한 함수,
    //스테이트를 끝내기 위한 함수가 있어야해
    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}