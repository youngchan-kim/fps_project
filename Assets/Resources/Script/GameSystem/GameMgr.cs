using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameMgr : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    public GameObject player;
    [Header("Enemy")]
    [SerializeField]
    public GameObject enumy;
    [Header("UI")]
    [SerializeField]
    public GameObject GUI_Mgr;

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
        // Enemy는 자신의 Start()에서 초기화
        //enumy.GetComponent<Enemy>().Initialize();
        GUI_Mgr.GetComponent<GUI_Mgr>().Initialize();
    }
    public GameObject GetCollierPlayer()
    {
        return player;
    }


}

