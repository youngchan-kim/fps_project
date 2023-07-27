using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Bag Object", menuName = " Inventory System/Items/Bag")]

public class BagObject : ItemObject
{
    public int restoreHealthValue;
    public void Awake()
    {
        type = ItemType.Bag;
    }
}