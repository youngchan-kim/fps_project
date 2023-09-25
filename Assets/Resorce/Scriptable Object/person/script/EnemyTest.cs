using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;

    Rigidbody target;
    EnemyData data;

    //적들의 업데이트시간을 0.04f로 쓴다.
    //업데이트 시간을 공용으로 사용하고 읽기 전용으로 사용한다.
    static readonly float updateTime = 0.04f;

    StateData stateData = null;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();
    }
    void ErrorMessage(string msg)
    {
        Debug.LogError(msg);
        gameObject.SetActive(false);
    }

    public void Initialize(StateData data)
    {
        stateData = data;
    }

    public bool SetData(EnemyData enemyData) { return true; }

    public void OnIbleState() { }
    public void OnChaseState() { }
    public void OnAttackState() { }
    public void OnDamagedState() { }
    public void OnDieState() { }
}
