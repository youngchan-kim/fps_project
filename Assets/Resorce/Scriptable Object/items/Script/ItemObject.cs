using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Food,
    Equipment,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    //아이템의 오브젝트
    public GameObject prefab;
    //아이템 타입
    public ItemType type;

    [TextArea(15, 20)]
    public string description;
}
