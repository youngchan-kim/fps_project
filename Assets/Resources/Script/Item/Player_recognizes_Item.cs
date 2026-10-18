using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;*/
/*using static UnityEngine.Rendering.VolumeComponent;*/


public enum InventoryType
{
    Ground,
    other
}

public class Player_recognizes_Item : MonoBehaviour
{
    [SerializeField]
    public InventoryObject Groundinventory, inventory, Equipinventory, Guninventory;
    //private bool player_recognizes = false;
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
    [SerializeField]
    private Transform gunEquipPoint;
    //아이템을 두기 위한 위치
    //월드 포지션을 가지기 위함
    private Vector3 hitPos;

    int slotnum;
    public LayerMask Dropzon_layermask;

    private void Update()
    {
        if( Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 100f, Dropzon_layermask))
        {
            Debug.Log(rayHit.point);
        }
    }

    //키보드로 먹을때
    //기존에는 획득 후 ItemPickup_inventory_System()까지 호출하면서 중복 처리 가능성이 있었어.
    //이제 아이템 추가에 성공했을 때만 바닥 아이템을 제거
    public void Pickup_Swap_Item(Collider Coll)
    {
        if (Coll == null)
            return;

        GroundItem ground = Coll.GetComponentInParent<GroundItem>();

        if (ground == null || ground.item == null)

            return;
        ItemObject itemObject = ground.item;
        Item item = itemObject.data;
        int amount = ground.amount;

        InventoryObject targetInventory = null;

        switch (itemObject.type)
        {
            case ItemType.Bullet:
            case ItemType.Medical:
                targetInventory = inventory;
                break;

            case ItemType.Helmet:
            case ItemType.Bag:
            case ItemType.Armor:
                targetInventory = Equipinventory;
                break;

            default:
                Debug.LogWarning("아직 획득 처리를 구현하지 않은 아이템: "
                                 + itemObject.type);
                return;
        }

        if (targetInventory == null)
            return;

        bool success = targetInventory.AddItem(
            item, InventoryType.other, amount);

        if (!success)
        {
            Debug.Log("인벤토리에 공간이 없습니다.");
            return;
        }

        // 월드 오브젝트를 제거하기 전에 바닥 UI 데이터도 제거
        if (Groundinventory != null)
        {
            InventorySlot groundSlot =
                Groundinventory.FindItemOnInventory(item);

            if (groundSlot != null)
                groundSlot.RemoveItem();
        }

        Destroy(ground.gameObject);

        pickup = true;

        Debug.Log(item.Name + " 획득: " + amount);
    }

    //드래그엔드일때 사용
    public void ItemPickup_inventory_System(ItemObject _itemObject, GameObject ground_Item)
    {
        Item _item = _itemObject.data;
        switch (_itemObject.type)
        {
            case ItemType.Gun:
                {
                    if (_itemObject.equippedGunPrefab == null)
                    {
                        Debug.LogError("장착용 총기 프리팹이 없습니다.");
                        break;
                    }

                    if (gunEquipPoint == null)
                    {
                        Debug.LogError("총기 장착 위치가 없습니다.");
                        break;
                    }
                    if (slotnum < 0 || slotnum >= Gunslot.Length)
                    {
                        Debug.LogWarning("잘못된 총기 슬롯 번호: " + slotnum);
                        break;
                    }

                    int targetSlot = slotnum;
                    // 총기 슬롯에 오브젝트 등록
                    // 이미 등록된 총기가 있다면 중복 생성 방지
                    if (Gunslot[targetSlot] != null)
                    {
                        Debug.LogWarning("이미 총기가 있는 슬롯입니다.");
                        break;
                    }

                    GameObject newGun = Instantiate(_itemObject.equippedGunPrefab,gunEquipPoint); 
                    GunSystem newGunSystem = newGun.GetComponent<GunSystem>();

                    if (newGunSystem == null)
                    {
                        Debug.LogError("생성한 총기에 GunSystem이 없습니다.");
                        Destroy(newGun);
                        break;
                    }

                    // 기존 Player_Action에서 사용하는 조준 지점
                    Transform point = transform.GetChild(1);

                    newGunSystem.SetAttackPoint(point);
                    Gunslot[targetSlot] = newGun;
                    // 등록한 총기만 활성화
                    GunActive(targetSlot);

                    slotnum = -1;

                    Debug.Log("총기 생성 및 장착 완료: " + newGun.name);
                    break;
                } 
            case ItemType.Bullet:
                //아이템 슬롯에 아이템 넣어야함
                //디스트로이하기 때문에 필요 없다.
                //ground_Item.SetActive(false);
                //총알은 플레이어에게서 버려질때를 제외하면 부모 오브젝트가 없다.
                //오브젝트 비활성화가 불가능하다.
                break;
            case ItemType.Medical:
                //아이템 슬롯에 아이템 넣어야함

                //힐템은 해당 아이템 상호작용전까지는 비활성화 상태이다.
                break;
        }
        //클릭한아이템을 Items에서 제거해야함
        //_item.groundobject.SetActive(false);
    }

    //프리팹이 없거나 바닥을 찾지 못했을 때 인벤토리 아이템이 사라지는 문제를 막을 수 있
    public bool ItemDropSystem(InventorySlot slot)
    {
        if (slot == null || slot.item == null)
            return false;

        if (slot.item.Id < 0 || slot.totalamount <= 0)
            return false;

        GameObject prefab = slot.item.item_object_Prefab;

        if (prefab == null)
        {
            Debug.LogError("바닥 아이템 프리팹이 없습니다.");
            return false;
        }

        if (!Physics.Raycast(
            transform.position + Vector3.up,
            Vector3.down,
            out RaycastHit hit,
            10f,
            Dropzon_layermask))
        {
            Debug.LogWarning("아이템을 놓을 바닥을 찾지 못했습니다.");
            return false;
        }

        Vector3 dropPosition = hit.point + Vector3.up * 0.1f;

        GameObject droppedItem = Instantiate(
            prefab,
            dropPosition,
            Quaternion.identity);

        GroundItem ground = droppedItem.GetComponent<GroundItem>();

        if (ground == null)
            ground = droppedItem.GetComponentInChildren<GroundItem>();

        if (ground == null)
        {
            Debug.LogError("프리팹에 GroundItem 컴포넌트가 없습니다.");
            Destroy(droppedItem);
            return false;
        }

        ground.amount = slot.totalamount;

        Debug.Log("아이템 버리기 성공: " + slot.item.Name);

        return true;
    }


    //추가 코드
    // 오브젝트 사이의 접촉이 일어난 순간 호출
    public void On_The_Ground_Item(ItemObject item, int itemamount)
    {
        Groundinventory.AddGroundItem(item.data, itemamount);
    }

    public void On_The_Ground_Item_Removed(ItemObject item)
    {
        Item_Removed(Groundinventory, -1, item.data);
        Groundinventory.Sort();
    }

    public void Add_Item(int invenID, Item item, int itemamount)
    {
        switch (invenID)
        {
            case 1:
                if (inventory.AddItem(item, InventoryType.other, itemamount))
                {
                    pickup = true;
                }
                break;
            case 2:
                if (Equipinventory.AddItem(item, InventoryType.other, itemamount))
                {
                    pickup = true;
                }
                break;
            case 3:
                if (Guninventory.AddItem(item, InventoryType.other, itemamount))
                {
                    pickup = true;
                }
                break;
        }
    }


    public void Item_Removed(InventoryObject item_out_inven, int inven_num, Item item )
    {
        //Debug.Log("반응 확인");
        item_out_inven.ClearItem(item.Id, inven_num);
        pickup = false;
    }


    /*    //목록체크
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
    }*/


    /*    //활성화된 오브젝트 리턴
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
        }*/

    public int GetInven_Find_Item(ItemObject _item)
    {
        if (_item == null || inventory == null || _item.data == null)
            return 0;

        if (inventory.Container == null || inventory.Container.Items == null)
            return 0;

        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            if (slot == null || slot.item == null)
                continue;

            if (slot.item.Id == _item.data.Id)
            {
                return slot.totalamount;
            }
        }

        return 0;
    }
    public void SetInven_Find_Item(ItemObject _item, int num)
    {
        if (_item == null || inventory == null || _item.data == null)
            return;

        if (inventory.Container == null || inventory.Container.Items == null)
            return;

        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            if (slot == null || slot.item == null)
                continue;

            if (slot.item.Id == _item.data.Id)
            {
                slot.totalamount = Mathf.Max(0, num);
                return;
            }
        }
    }
    public void InventRefresh()
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            if (inventory.Container.Items[i].totalamount == 0)
            {
                inventory.Container.Items[i].RemoveItem();
            }
        }
    }
    public void Inventsort(int invenID)
    {
        switch (invenID)
        {
            case 0:
                Groundinventory.Sort();
                break;
            case 1:
               inventory.Sort();
                break;
        }
        
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
    void Additem(GameObject[] Itemslot, GameObject item)
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
    //Gunslot[i]가 null이면 SetActive(false)에서 NullReferenceException이 발생해서 null인경우를 if처리로 예외발생을 막음
    public void RemoveGun(int num)
    {
        if (num < 0 || num >= Gunslot.Length)
        {
            Debug.LogWarning("잘못된 Gun 슬롯 번호: " + num);
            return;
        }

        GameObject gun = Gunslot[num];

        if (gun == null)
        {
            Debug.LogWarning(
                "Gun 슬롯 " + num + "에 등록된 총기가 없습니다.");
            return;
        }

        // 슬롯 참조 먼저 제거
        Gunslot[num] = null;

        // 장착용 총기 오브젝트 삭제
        Destroy(gun);

        Debug.Log("장착용 총기 삭제: " + gun.name);
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
            if ((sellect) != i && Gunslot[i] != null)
                Gunslot[i].gameObject.SetActive(false);
        }
    }
    //아이템 id 찾기
    //수정 사항 
    // 복잡한 GetChild() 경로 제거
    //자식 인덱스 오류 방지
    //비활성화된 총기까지 검색
    //총기 ID를 기준으로 오브젝트 검색
    GameObject Gun_Item_ID_Check(int Id)
    {
        GunSystem[] guns = GetComponentsInChildren<GunSystem>(true);
        Debug.Log($"[총기 검색] 요청 ID: {Id}, 검색된 총기 수: {guns.Length}");
        foreach (GunSystem gun in guns)
        {
            if (gun.Gun_property == null)
            {
                Debug.LogWarning($"[총기 검색] {gun.name}: Gun_property 없음");
                continue;
            }
            Debug.Log($"[총기 검색] {gun.name}, 총기 ID: {gun.Gun_property.Id}");

            if (gun.Gun_property.Id == Id)
            {
                Debug.Log("총기 검색 성공: " + gun.name);
                return gun.gameObject;
            }
        }

        Debug.LogError($"[총기 검색 실패] ID {Id}에 해당하는 총기가 없음");

        return null;
    }

}
