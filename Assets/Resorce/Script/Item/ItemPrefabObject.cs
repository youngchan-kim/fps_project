using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabObject : MonoBehaviour
{
    GameObject gameobject;
    
    public void SetObject(GameObject _gameobject)
    {
        gameobject = _gameobject;
    }

    public GameObject GetObject()
    {
        if (gameobject == null)
            return null;
        return gameobject;
    }
}
