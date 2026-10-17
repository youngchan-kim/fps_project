using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// EnemyFSM.cs - 적 AI의 상태 관리 전담
// 기존 코드:
// - Enemy.cs에서 직접 상태를 관리하여 유지보수 어려움
// - 상태 전환 로직이 분산되어 가독성이 떨어짐
// 변경된 코드:
// - EnemyFSM 클래스를 생성하여 상태 관리를 전담하도록 변경
// - 상태 전환을 메서드화하여 코드 가독성 향상
// 주요 이점:
// - 코드 유지보수성 향상 및 상태 확장 용이
// - 상태 관리 로직이 모듈화되어 가독성 증가

public class EnemyFSM
{
    private Enemy enemy;
    private IEnemyState currentState;

    public EnemyFSM(Enemy enemy)
    {
        this.enemy = enemy;
        ChangeState(new IdleState(enemy));
    }

    public void Update()
    {
        if (enemy.IsDead() && !(currentState is DieState))
        {
            ChangeState(new DieState(enemy));
        }

        currentState?.Update();
    }
    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}

// IEnemyState 인터페이스 정의
public interface IEnemyState
{
    void Enter();
    void Update();
    void Exit();
}

// Idle 상태 구현
public class IdleState : IEnemyState
{
    
    private Enemy enemy;

    public IdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Idle 상태 시작");
    }

    public void Update()
    {
        if (enemy.HasTarget())
        {
            enemy.FSM.ChangeState(new RunState(enemy));
        }
        else
        {
            enemy.UpdateIdle();
            enemy.FSM.ChangeState(new ReconnaissanceState(enemy));
        }
    }

    public void Exit()
    {
        Debug.Log("Idle 상태 종료");
    }
}
// Reconnaissance 상태 구현
public class ReconnaissanceState : IEnemyState
{
    private Enemy enemy;

    public ReconnaissanceState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Reconnaissance 상태 시작");
    }

    public void Update()
    {
        if (enemy.HasTarget())
        {
            enemy.FSM.ChangeState(new RunState(enemy));
            return;
        }

        enemy.UpdateReconnaissance();
    }

    public void Exit()
    {
        Debug.Log("Reconnaissance 상태 종료");
    }
}

// Run 상태 구현
public class RunState : IEnemyState
{
    private Enemy enemy;

    public RunState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Run 상태 시작");
    }

    public void Update()
    {
        if (!enemy.HasTarget())
        {
            enemy.FSM.ChangeState(new IdleState(enemy));
            return;
        }

        float distance = enemy.DistanceToTarget();

        if (distance > enemy.GetNomalSiteLen())
        {
            enemy.FSM.ChangeState(new IdleState(enemy));
            return;
        }

        if (distance <= enemy.Attectlen)
        {
            enemy.FSM.ChangeState(new AttackState(enemy));
            return;
        }

        enemy.UpdateRun();
    }

    public void Exit()
    {
        Debug.Log("Run 상태 종료");
    }
}

// Attack 상태 구현
public class AttackState : IEnemyState
{
    private Enemy enemy;

    public AttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Attack 상태 시작");
    }

    public void Update()
    {
        if (!enemy.HasTarget())
        {
            enemy.FSM.ChangeState(new IdleState(enemy));
            return;
        }

        float distance = enemy.DistanceToTarget();

        if (distance > enemy.Attectlen)
        {
            enemy.FSM.ChangeState(new IdleState(enemy));
            return;
        }

        enemy.UpdateAttack();
    }

    public void Exit()
    {
        enemy.ExitAttack();

        Debug.Log("Attack 상태 종료");
    }
}

public class DieState : IEnemyState
{
    private Enemy enemy;

    public DieState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Die 상태 시작");
    }

    public void Update()
    {
        enemy.UpdateDie();
    }

    public void Exit()
    {
        Debug.Log("Die 상태 종료");
    }
}