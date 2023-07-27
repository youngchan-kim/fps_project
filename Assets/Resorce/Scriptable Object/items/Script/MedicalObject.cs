using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Medical Object", menuName = " Inventory System/Items/Medical")]

public class MedicalObject : ItemObject
{
    //회복량에 대한 정보 필요
    public int restoreHealthValue;
    public void Awake()
    {
        type = ItemType.Medical;
    }
}