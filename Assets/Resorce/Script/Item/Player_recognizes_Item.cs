using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_recognizes_Item : MonoBehaviour
{
    private GroundInterface Ground_item;
    private DynamicInterface Inventory_item;
    private StaticInterface Equip_item;
    private StaticInterface Gun_item;
    public InventoryObject Groundinventory, inventory, Equipinventory, Guninventory;
    private bool player_recognizes = false;
    Item crruntitem;
    GroundItem groundItem;
    public bool pickup;
    private void Update()
    {
        Player_recognizes();
    }
    /*    public void OnTriggerEnter(Collider other)
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
    */
    
    public void Swap_Item()
    {
        if (Groundinventory.FindItemOnInventory(crruntitem) != null)
        {
            if (groundItem.name == "Gun")
            {
                Debug.Log("아이템 스왑");
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(crruntitem), Guninventory.GetEmptySlot());
            }
            else if (groundItem.name == "Helmat" || groundItem.name == "Bag" || groundItem.name == "Amor")
            {
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(crruntitem), Equipinventory.GetEmptySlot());
            }
            else
            {
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(crruntitem), inventory.GetEmptySlot());
            }
        }
    }

        //추가 코드
        // 오브젝트 사이의 접촉이 일어난 순간 호출
    public void On_The_Ground_Item(Item item)
    {
        Debug.Log("주변아이템 목록에 추가");
        Add_Item(Groundinventory, item);
        crruntitem = item;
    }

    public void On_The_Ground_Item_Removed(Item item)
    {
        Debug.Log("주변아이템 목록에서 삭제");
        Item_Removed(Groundinventory, item);
        crruntitem = null;
    }

    public void Add_Item(InventoryObject inven, Item item)
    {

        if (inven.AddItem(item, 1))
        {
            crruntitem = item;
            pickup = true;
        }
    }
    
    public void Item_Removed(InventoryObject inven, Item item)
    {
        inven.ClearItem(item);
        pickup = false;
    }
    public void item_move(InventoryObject inven1, InventoryObject inven2, Item item)
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        groundItem = other.GetComponent<GroundItem>();
        Debug.Log(name);
    }
    public void OnTriggerExit(Collider other)
    {
        groundItem = null;
    }

    public GroundItem GetPickupItem()
    {
        Debug.Log(name);
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
