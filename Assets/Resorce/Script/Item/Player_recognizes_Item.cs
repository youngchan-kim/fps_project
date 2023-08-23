using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InventoryType
{
    Ground,
    other
}

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
    //모든 스크립트에서 변경할 수 있게 하기 위함
    public static GameObject[] Gunslot =  new GameObject[4];
    //아이템의 실제정보를 가지고 있음
    public static GameObject[] Itemslot = new GameObject[4];
    public bool equipped;
    //아이템을 두기 위한 위치
    //월드 포지션을 가지기 위함
    private Vector3 hitPos;

    int slotnum;


    private void Update()
    {
        Player_recognizes();
    }

    //키보드로 먹을때
    public void Pickup_Swap_Item(Transform ground_Item, Item see_the_item)
    {
        if (Groundinventory.FindItemOnInventory(see_the_item) != null)
        {
            if (ground_Item.GetComponent<GroundItem>().name == "Gun")
            {
                Debug.Log("아이템 픽업");
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(see_the_item), Guninventory.GetEmptySlot());
            }
            else if (ground_Item.GetComponent<GroundItem>().name == "Helmat" || ground_Item.GetComponent<GroundItem>().name == "Bag" || ground_Item.GetComponent<GroundItem>().name == "Armor")
            {
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(see_the_item), Equipinventory.GetEmptySlot());
            }
            else
            {
                Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(see_the_item), inventory.GetEmptySlot());
            }
           // groundItem = ground_Item.GetComponent<GroundItem>();
            see_the_item.groundobject = ground_Item.gameObject;
            ItemPickup_inventory_System(see_the_item);
        }
    }

    //드래그엔드일때 사용
    public void ItemPickup_inventory_System(Item _item)
    {
        //총인경우 체크
        if (Activeitem = Gun_Item_ID_Check(_item.Id))
        {
            //플레이어가 총을 장착 여부와 슬롯이 차있는지 여부 확인
            if (GetGunSlotEmpty())
            {
                //해당 부분 오류
                //Activeitem.SetActive(true);
                Additem(Gunslot , Gun_Item_ID_Check(_item.Id));
                GunActive(0);
                
            }
        }
        else
        {
            //Additem(Itemslot, Gun_Item_ID_Check(_item.Id));
            //Activeitem.SetActive(true);
        }

        _item.groundobject.SetActive(false);
        //클릭한아이템을 Items에서 제거해야함
        //충돌한 아이템을 비활성화한다.
    }


    public void GunActive(int sellect)
    {
        if (Gunslot[sellect] != null)
        {
            Gunslot[sellect].SetActive(true);
        }
        GunUnActive(sellect);
    }
    public bool GetGunSlotEmpty()
    {
        for (int i = 0; i < Gunslot.Length; i++)
        {
            if (Gunslot[i] == null)
                return true;
        }
        return false;
    }
    void Additem(GameObject[] Itemslot , GameObject item)
    {
        if (slotnum == -1)
        {
            for (int i = 0; i < Itemslot.Length; i++)
            {
                if (Itemslot[i] != null)
                    Debug.Log(i + "번째 슬롯에 " + Itemslot[i] + " 있다.");
                else
                    Debug.Log(i + "번째 슬롯에 없다.");
                if (Itemslot[i] == null)
                {
                    Itemslot[i] = item;
                    return;
                }
            }
        }
        else
        {
            Itemslot[slotnum] = item;
        }
        slotnum = -1;
    }

    public void ItemSlotNum(int num)
    {
        slotnum = num;
    }
    public void RemoveGun(int num)
    {
        for (int i = 0; i < Gunslot.Length; i++)
        {
            if (i == num)
            {
                Gunslot[i].transform.gameObject.SetActive(false);
                Gunslot[i] = null;
                return;
            }
        }
    }
    public void GunSlot_swap(int slotnum1, int slotnum2)
    {
        GameObject temp = Gunslot[slotnum2];
        Gunslot[slotnum2] = Gunslot[slotnum1];
        Gunslot[slotnum1] = temp;
        GunActive(slotnum2);
    }

    public void GunUnActive(int sellect)
    {
        for (int i = 0; i < Gunslot.Length; i++)
        {
            if((sellect) != i && Gunslot[i] != null)
            Gunslot[i].transform.gameObject.SetActive(false);
        }
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


    public void ItemDropSystem()
    {
        if (crruntitem.groundobject.name == "Gun")
        {
            //플레이어가 총을 장착 여부와 슬롯이 차있는지 여부 확인
            if (equipped)
            {
                equipped = false;
                ItemActiveCheck().SetActive(false);
            }
        }
        else
        {
            //ItemActiveCheck().SetActive(false);
        }

        crruntitem.groundobject.SetActive(true);
        Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit rayHit, 100f);
        hitPos = rayHit.point;
        hitPos.y += 0.01f;
        crruntitem.groundobject.transform.position = hitPos;
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
        //Debug.Log(ItemListcount);

        for (int i = 0; i < ItemListcount; i++)
        {
            if (transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject.activeSelf == true)
            {

                return transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject;
            }
        }
        return null;
    }
    //추가 코드
    // 오브젝트 사이의 접촉이 일어난 순간 호출
    public void On_The_Ground_Item(Item item, GameObject prantObject)
    {
        Add_Item(Groundinventory.InventoryID, item, prantObject);
        crruntitem = item;
    }

    public void On_The_Ground_Item_Removed(Item item)
    {
        Item_Removed(Groundinventory, item);
        crruntitem = null;
    }

    public void Add_Item(int invenID, Item item, GameObject prantObject)
    {
        switch(invenID)
        {
            case 0:
                if (Groundinventory.AddItem(item, 1, prantObject, InventoryType.Ground))
                {
                    crruntitem = item;
                    pickup = true;
                }
                break;
            case 1:
                if (inventory.AddItem(item, 1, prantObject, InventoryType.other))
                {
                    crruntitem = item;
                    pickup = true;
                }
                break;
            case 2:
                if ( Equipinventory.AddItem(item, 1, prantObject, InventoryType.other))
                {
                    crruntitem = item;
                    pickup = true;
                }
                break;
            case 3:
                if (Guninventory.AddItem(item, 1, prantObject, InventoryType.other))
                {
                    crruntitem = item;
                    pickup = true;
                }
                break;
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

    public void ClickItem(Item clickitem)
    {
        crruntitem = clickitem;
    }

    public int GetInven_Find_Item(ItemObject _item)
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        { 
            if(inventory.Container.Items[i].item.Id == _item.data.Id)
            {
                return inventory.Container.Items[i].amount;
            }  
        }
        return 0;
    }
    public void SetInven_Find_Item(ItemObject _item, int num)
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            if (inventory.Container.Items[i].item.Id == _item.data.Id)
            {
                 inventory.Container.Items[i].amount = num;
            }
        }
    }
}
