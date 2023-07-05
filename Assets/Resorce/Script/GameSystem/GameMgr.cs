using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameMgr : MonoBehaviour
{
    public Transform FPS_Cam;
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
        //FPS_Cam.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
    void Initialize() { }
    void StartGame() { }
}

