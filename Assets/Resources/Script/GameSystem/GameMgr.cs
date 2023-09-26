using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mesh;

public sealed class GameMgr : MonoBehaviour
{
    public GameObject player;
    public EnemyData testData;
    public EnemyTest testEnemy;
    
    static GameMgr instance = null;
    public static GameMgr Instance
    {
        get
        {
            if(!instance) 
            {
                instance = FindObjectOfType<GameMgr>();
                if (!instance) instance = new GameObject("GameManager").AddComponent<GameMgr>();

                instance.Initialize();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    private void Awake() { if (this != Instance) Destroy(gameObject); }
    private void Start()
    {
        Initialize();
    }
    void Initialize() 
    {
        StartGame();
    }
    void StartGame() 
    {
        player.GetComponent<Player>().Initialize();
        EnemyTest();
    }
    public GameObject GetCollierPlayer()
    {
        return player;
    }

    void EnemyTest()
    {
        IState idle = (IState)Resources.Load("Scriptable Object/person/Enemy_FSM/Idle State"); // 로드 경로에 주의!!
        IState chase = (IState)Resources.Load("Scriptable Object/person/Enemy_FSM/Chase State"); // State를 만들어 둔 경로로 변경할 것!!
        IState attack = (IState)Resources.Load("Scriptable Object/person/Enemy_FSM/Attack State"); // 잘못된 경로일 경우 null을 반환!!
        StateData data = ScriptableObject.CreateInstance<StateData>(); // State Data 생성.
        data.SetData(idle, chase, attack, null, null); // 아직 Damaged와 Die는 테스트 하지 않는다.
        testEnemy.Initialize(data);
        testEnemy.SetData(testData);
    }

}

