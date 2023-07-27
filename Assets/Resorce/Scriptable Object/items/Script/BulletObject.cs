using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Bullet Object", menuName = " Inventory System/Items/Bullet")]

public class BulletObject : ItemObject
{
    //총알에 대한 정보 필요(무게 등등 탄속 수량)
    public int count;
    public void Awake()
    {
        type = ItemType.Bullet;
    }
}