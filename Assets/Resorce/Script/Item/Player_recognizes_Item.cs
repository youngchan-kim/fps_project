using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_recognizes_Item : MonoBehaviour
{
    private GroundDynamicInterface Ground_item;
    private DynamicInterface Inventory_item;
    private StaticInterface Equip_item;
    private StaticInterface Gun_item;
    public InventoryObject Groundinventory, inventory;
    private bool player_recognizes = false;
    Item crruntitem;
    GroundItem groundItem;
    public bool pickup;
    private void Update()
    {
        Player_recognizes();
    }
    //추가 코드
    // 오브젝트 사이의 접촉이 일어난 순간 호출
    public void OnTriggerEnter(Collider other)
    {
        groundItem = other.GetComponent<GroundItem>();
        if (groundItem)
        {
            Item _item = new Item(groundItem.item);
            if (Groundinventory.AddItem(_item, 1))
            {
                crruntitem = _item;
                pickup = true;
            }
        }
    }


    public void GetRemoveditem(/*item*/)
    {
        Groundinventory.ClearItem(crruntitem);
        pickup = false;
        groundItem = null;
    }
    public GroundItem GetPickupItem()
    {
        return groundItem;
    }

    public bool GetPickup()
    {
        return pickup;
    }

    /*//실행안됨
    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            player_recognizes = true;
            //물체와 충돌하게 되면 인벤토리에 들어가게 만듦
            player.Groundinventory.AddItem(new Item(GetComponent<GroundItem>().item), 1);
        }
    }*/
    //실행안됨
    /* public void OnTriggerExit(Collider other)
     {
         player_recognizes = false;
     }*/

    public bool Player_recognizes()
    {
        return player_recognizes;
    }


}
