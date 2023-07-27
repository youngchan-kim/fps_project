using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Armor Object", menuName = " Inventory System/Items/Armor")]

public class ArmorObject : ItemObject
{
    //갑옷 정보 필요
    public float defenceBonus;
    public void Awake()
    {
        type = ItemType.Armor;
    }
}