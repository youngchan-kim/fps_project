using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;
/*using static UnityEditor.Progress;*/

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    Player player;
    //임시 드레그가 끝났는지 안끝났는지 
    bool DragEnd = false;

    void Start()
    {
        player = GameMgr.Instance.player.GetComponent<Player>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parent = this;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });

        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }
    // Update is called once per frame
    void Update()
    {
        slotsOnInterface.UpdateSlotDisplay();
    }
    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj, PointerEventData.InputButton inputbutton = 0)
    {
        if (inputbutton == (PointerEventData.InputButton)1)
        {
            //아이템이 사용할 수 있는 아이템인지 사용 못하는 아이템인지 체크
            //사용할 수 있는 아이템이면 사용조건에 따라 사용
            //slotsOnInterface[obj].item
        }
        else
            MouseData.slotHoveredOver = obj;
    }


    public void OnDown(GameObject obj, PointerEventData.InputButton inputbutton = 0)
    {
        if (slotsOnInterface[obj].GetInventoryID() > 0)
        {
            if (slotsOnInterface[obj].item.inven_in_active == true/*아이템의 인벤토리 액티브 유무*/)
                Debug.Log("해당아이템은 우클릭으로 사용할 수 있는 기능이 있습니다.");
            else
                Debug.Log("해당아이템은 사용할 수 있는 기능이 없습니다.");
        }
        else
            return;
    }

    public void OnUp(GameObject obj, PointerEventData.InputButton inputbutton = 0)
    {
        int InventoryID = slotsOnInterface[obj].GetInventoryID();
        if (inputbutton == (PointerEventData.InputButton)1 && slotsOnInterface[obj].item.inven_in_active
            && MouseData.interfaceMouseIsOver.inventory.InventoryID != 0)
        {

            if (slotsOnInterface[obj].item.addHeal != 0 && slotsOnInterface[obj].item.Id == 4)
            {
                float test = slotsOnInterface[obj].item.addHeal;

                player.hp.OnHpAnimtion(slotsOnInterface[obj]);
            }
            //기능 사용
            //slotsOnInterface[obj].item;
            player.Inventsort(InventoryID);
        }
        else
        {
            player.Inventsort(InventoryID);
            return;
        }

    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();

    }
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    public void OnDragStart(GameObject obj)
    {

        MouseData.tempItemBeingDragged = CreateTempItem(obj);
        //클릭된 아이템의 grounditem 스크립트를 알아온다.
    }
    //드래그 중인 아이템에 사용됨
    //임시 아이템 생성 함수
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }
    public void OnDragEnd(GameObject obj)
    {
        Debug.Log("===== OnDragEnd 시작 =====");

        Debug.Log("출발 슬롯: " + obj.name);

        Debug.Log("현재 인터페이스: " +
            (MouseData.interfaceMouseIsOver != null
                ? MouseData.interfaceMouseIsOver.name
                : "NULL"));

        Debug.Log("마우스가 가리키는 슬롯: " +
            (MouseData.slotHoveredOver != null
                ? MouseData.slotHoveredOver.name
                : "NULL"));

        int InventoryID = slotsOnInterface[obj].GetInventoryID();
        //만들어야 하는것 해당키가 없는 경우의 예외 처리 만들것

        Debug.Log("인터페이스 NULL 여부: " +
    (MouseData.interfaceMouseIsOver == null));
        if (MouseData.tempItemBeingDragged == null)
            return;
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
        {
            if (InventoryID != 0)
            {
                if (player.ItemDropSystem(slotsOnInterface[obj]))
                {
                    slotsOnInterface[obj].RemoveItem();
                    Debug.Log(InventoryID);
                }
                if (InventoryID == 3)
                    player.RemoveGun(slotsOnInterface[obj].parent.inventory.ItemSlotNum(slotsOnInterface[obj]));
            }

            player.Inventsort(InventoryID);
            DragEnd = false;
            //Debug.Log(name);
            /*if (mouseHoverSlotData.GetInventoryID() != 0)
            {
                //Debug.Log(name);
                GameMgr.Instance.player.GetComponent<Player>().ItemPickup_inventory_System();
            }*/
            return;
        }

        /*        Debug.Log(MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver].name);
                Debug.Log(MouseData.slotHoveredOver);*/

        if (MouseData.slotHoveredOver)
        {

            //Debug.Log("확인5");
            if (MouseData.interfaceMouseIsOver == null ||
    MouseData.slotHoveredOver == null)
                return;

            if (!MouseData.interfaceMouseIsOver.slotsOnInterface.TryGetValue(
                    MouseData.slotHoveredOver,
                    out InventorySlot mouseHoverSlotData))
            {
                return;
            }
            int endInventoryID = mouseHoverSlotData.GetInventoryID();
            switch (endInventoryID)
            {
                case 0:
                    if (InventoryID != 0)
                    {
                        if (player.ItemDropSystem(slotsOnInterface[obj]))
                        {
                            if (InventoryID == 3)
                                player.RemoveGun(slotsOnInterface[obj].parent.inventory.ItemSlotNum(slotsOnInterface[obj]));
                            slotsOnInterface[obj].RemoveItem();
                        }

                    }
                    break;
                case 1:
                    {
                        InventorySlot sourceSlot = slotsOnInterface[obj];

                        if (!mouseHoverSlotData.CanPlaceInSlot(sourceSlot.ItemObject))
                            break;

                        // 일반 인벤토리 내부 이동은 이번 작업에서 제외
                        if (InventoryID == 1)
                            break;

                        // 바닥 인벤토리 -> 일반 인벤토리
                        if (InventoryID == 0)
                        {
                            ItemObject itemObject = sourceSlot.ItemObject;

                            if (itemObject == null)
                                break;

                            if (itemObject.type != ItemType.Bullet &&
                                itemObject.type != ItemType.Medical)
                                break;

                            int amount = sourceSlot.totalamount;

                            if (amount <= 0)
                                break;

                            // 아이템 추가 성공 여부를 직접 확인
                            bool added = player.inventory.AddItem(
                                sourceSlot.item,
                                InventoryType.other,
                                amount);

                            if (!added)
                                break;

                            // 바닥에 연결된 오브젝트 제거
                            GameObject groundObject = sourceSlot.slot_item_object;

                            if (groundObject != null)
                                Destroy(groundObject);

                            sourceSlot.RemoveItem();
                            player.Groundinventory.Sort();
                        }

                        break;
                    }
                case 2:
                    /*                  if (mouseHoverSlotData.CanPlaceInSlot(slotsOnInterface[obj].ItemObject))
                                      {

                                      }*/
                    break;
                case 3:
                    {
                        InventorySlot sourceSlot = slotsOnInterface[obj];

                        Debug.Log("Gun 슬롯 드래그 시작");
                        Debug.Log("출발 인벤토리 ID: " + InventoryID);
                        Debug.Log("도착 인벤토리 ID: " + endInventoryID);
                        // [수정] Gun 인벤토리(ID 3)는 총기(ItemType.Gun)만 받을 수 있도록 강제 제한.
                        // AllowedItems가 비어 있으면 CanPlaceInSlot()이 모든 아이템을 허용하므로 별도로 검사한다.
                        ItemObject draggedItem = sourceSlot.ItemObject;
                        if (draggedItem == null || draggedItem.type != ItemType.Gun)
                        {
                            Debug.Log("[Gun 슬롯 이동 거부] 총기가 아닌 아이템입니다.");
                            break; // [수정] 장착/슬롯 교환/바닥 오브젝트 제거를 실행하지 않음.
                        }

                        // [수정] 기존 슬롯 허용 타입 검사도 유지해 이중으로 확인한다.
                        bool canPlace = mouseHoverSlotData.CanPlaceInSlot(
                            draggedItem);

                        Debug.Log("Gun 슬롯 배치 가능: " + canPlace);

                        if (!canPlace)
                            break;

                        if (InventoryID == 0)
                        {
                            Debug.Log("Ground -> Gun 이동 시도");

                            // 아이템 이동 전에 실제 바닥 오브젝트 참조 저장
                            GameObject groundObject = sourceSlot.slot_item_object;

                            if (groundObject == null)
                            {
                                Debug.LogWarning("바닥 오브젝트 참조가 없습니다.");
                                break;
                            }

                            player.ItemSlotNum(
                                mouseHoverSlotData.parent.inventory
                                    .ItemSlotNum(mouseHoverSlotData));

                            player.ItemPickup_inventory_System(
                                sourceSlot.ItemObject,
                                groundObject);

                            inventory.SwapItems(sourceSlot, mouseHoverSlotData);

                            // 월드에 있는 바닥 오브젝트 제거
                            Destroy(groundObject);

                            Debug.Log("Ground -> Gun 이동 완료");
                        }
                        else
                        {
                            // 기존 Gun 슬롯 내부 교체 로직 유지
                            if (InventoryID != 3)
                            {
                                player.ItemSlotNum(
                                    mouseHoverSlotData.parent.inventory
                                        .ItemSlotNum(mouseHoverSlotData));

                                player.ItemPickup_inventory_System(
                                    sourceSlot.ItemObject,
                                    sourceSlot.slot_item_object);
                            }

                            inventory.SwapItems(sourceSlot, mouseHoverSlotData);

                            if (InventoryID == 3)
                            {
                                player.GunSlot_swap(
                                    sourceSlot.parent.inventory.ItemSlotNum(sourceSlot),
                                    mouseHoverSlotData.parent.inventory
                                        .ItemSlotNum(mouseHoverSlotData));
                            }
                        }

                        break;
                    }
            }

            DragEnd = true;
        }
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public bool OnDragEnd_End()
    {
        return DragEnd;
    }

    public InventorySlot ClickItem(InventorySlot clickItem)
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            if (clickItem == inventory.Container.Items[i])
                return inventory.Container.Items[i];
        }
        return null;
    }

}


public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

//확장 메서드
public static class ExtensionMethods
{

    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            Image image = _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>();
            TextMeshProUGUI text_GUI = _slot.Key.GetComponentInChildren<TextMeshProUGUI>();

            if (_slot.Value.item.Id >= 0)
            {
                image.sprite = _slot.Value.ItemObject.uiDisplay;
                image.color = new Color(1, 1, 1, 1);
                //text_GUI.text = _slot.Value.item.amount != 0 ? _slot.Value.item.amount.ToString("n0"): "";
                text_GUI.text = _slot.Value.totalamount != 0 ? _slot.Value.totalamount.ToString("n0") : "";
            }
            else
            {
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);
                text_GUI.text = "";
            }
        }
    }
}