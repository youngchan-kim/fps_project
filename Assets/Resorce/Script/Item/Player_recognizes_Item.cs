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

    string itemname;
    GroundItem haveitem;
    GameObject Activeitem;
    GameObject ItemList;
    //object의 장착 여부
    public bool equipped;
    //모든 스크립트에서 변경하기 위함
    public static bool GunslotFull;

    //아이템을 두기 위한 위치
    //월드 포지션을 가지기 위함
    private Vector3 hitPos;

    private void Update()
    {
        Player_recognizes();
    }

    //키보드로 먹을때
    public void Pickup_Swap_Item(GroundItem ground_Item, Item see_the_item)
    {
        if (Groundinventory.FindItemOnInventory(see_the_item) != null)
        {
            if (ground_Item.name == "Gun")
            {
                Debug.Log("아이템 픽업");
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(see_the_item), Guninventory.GetEmptySlot());
            }
            else if (ground_Item.name == "Helmat" || ground_Item.name == "Bag" || ground_Item.name == "Armor")
            {
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(see_the_item), Equipinventory.GetEmptySlot());
            }
            else
            {
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(see_the_item), inventory.GetEmptySlot());
            }
            groundItem = ground_Item;
            ItemPickup_inventory_System();
        }
    }
    //드래그로 먹을때
    public void Pickup_Swap_Item()
    {
        if (Groundinventory.FindItemOnInventory(crruntitem) != null)
        {
            if (groundItem.name == "Gun")
            {
                Debug.Log("아이템 픽업");
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(crruntitem), Guninventory.GetEmptySlot());
            }
            else if (groundItem.name == "Helmat" || groundItem.name == "Bag" || groundItem.name == "Armor")
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
        Add_Item(Groundinventory, item);
        crruntitem = item;
    }

    public void On_The_Ground_Item_Removed(Item item)
    {
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

    public bool GetPickup()
    {
        return pickup;
    }

    public bool Player_recognizes()
    {
        return player_recognizes;
    }

    //드래그엔드일때 사용
    public void ItemPickup_inventory_System()
    {
        //총인경우 체크
        if (Activeitem = Gun_Item_ID_Check(crruntitem.Id))
        {
            
            //플레이어가 총을 장착 여부와 슬롯이 차있는지 여부 확인
            if (!equipped && !GunslotFull)
            {
                Activeitem.transform.gameObject.SetActive(true);
                equipped = true;
                GunslotFull = true;
            }
        }
        else
        {
            Activeitem.transform.gameObject.SetActive(true);
        }

        //클릭한아이템을 Items에서 제거해야함
        //충돌한 아이템을 비활성화한다.
        haveitem.gameObject.SetActive(false);
    }

    public void ItemDropSystem()
    {
        if (haveitem.name == "Gun")
        {
            //플레이어가 총을 장착 여부와 슬롯이 차있는지 여부 확인
            if (equipped)
            {
                equipped = false;
                GunslotFull = false;
                ItemActiveCheck().SetActive(false);
            }
        }
        else
        {
            ItemActiveCheck().SetActive(false);
        }

        haveitem.gameObject.SetActive(true);
        Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit rayHit, 100f);
        hitPos = rayHit.point;
        hitPos.y += 0.01f;
        haveitem.transform.position = hitPos;
    }

    //목록체크
    GameObject ListActiveCheck(string name)
    {
        int ListListcount = transform.GetChild(1).GetChild(0).childCount;
        //Debug.Log(ListListcount);

        for (int i = 0; i < ListListcount; i++)
        {
            if (transform.GetChild(1).GetChild(0).GetChild(i).gameObject.name == name)
            {
                return transform.GetChild(1).GetChild(0).GetChild(i).gameObject;
            }
        }
        return null;
    }


    //활성화된 오브젝트 리턴
    GameObject ItemActiveCheck()
    {
        int ItemListcount = transform.GetChild(1).GetChild(0).GetChild(0).childCount;
        Debug.Log(ItemListcount);

        for (int i = 0; i < ItemListcount; i++)
        {
            if (transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject.activeSelf == true)
            {

                return transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject;
            }
        }
        return null;
    }

    //아이템 id 찾기
    GameObject Gun_Item_ID_Check(int Id)
    {
        int ItemListcount = transform.GetChild(1).GetChild(0).GetChild(0).childCount;
        for (int i = 0; i < ItemListcount; i++)
        {
            if (Id == transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).transform.GetComponent<GunSystem>().Gun_property.Id)
            {
                //Debug.Log(transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject.name);
                return transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject;
            }
        }
        return null;
    }

    public void ClickItem(Item clickitem)
    {
        crruntitem = clickitem;
    }

}
