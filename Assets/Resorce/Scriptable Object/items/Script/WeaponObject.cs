using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon Object", menuName = " Inventory System/Items/Weapon")]

public class WeaponObject : ItemObject
{
    public float atkDamege;
    //무기의 정보들이 필요
    //총기 정보입력
    public void Awake()
    {
        type = ItemType.Weapon;
    }
}