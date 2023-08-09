using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameMgr : MonoBehaviour
{
    public Player player;

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
        player.Initialize();
    }
    public Player GetCollierPlayer()
    {
        return player;
    }
}

