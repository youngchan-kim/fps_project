using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;
using static UnityEditor.Progress;

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

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
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
        GameObject Item_coupling_object = obj.GetComponent<ItemPrefabObject>().GetObject();
        //만들어야 하는것 해당키가 없는 경우의 예외 처리 만들것
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
        {
            Debug.Log("오브젝트를 허공에 뒀을 때");
            if (slotsOnInterface[obj].GetInventoryID() != 0)
            {
                player.ItemDropSystem(slotsOnInterface[obj]);
                slotsOnInterface[obj].RemoveItem();
                Debug.Log(slotsOnInterface[obj].GetInventoryID());
                if (slotsOnInterface[obj].GetInventoryID() == 3)
                    player.RemoveGun(slotsOnInterface[obj].parent.inventory.ItemSlotNum(slotsOnInterface[obj]));
            }
            
            DragEnd = false;
            //Debug.Log(name);
            /*if (mouseHoverSlotData.GetInventoryID() != 0)
            {
                //Debug.Log(name);
                GameMgr.Instance.player.GetComponent<Player>().ItemPickup_inventory_System();
            }*/
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            Debug.Log("찾는것" + slotsOnInterface[obj].GetInventoryID());
            //Debug.Log("땅에 있던 오브젝트의 이름" + obj.GetComponent<ItemPrefabObject>().GetObject().name);
            int startInventoryID = slotsOnInterface[obj].GetInventoryID();
            int endInventoryID = mouseHoverSlotData.GetInventoryID();
            switch (endInventoryID)
            {
                case 0:                    
                    if (startInventoryID != 0)
                    {
                        player.ItemDropSystem(slotsOnInterface[obj]);
                        if (startInventoryID == 3)
                            player.RemoveGun(slotsOnInterface[obj].parent.inventory.ItemSlotNum(slotsOnInterface[obj]));
                        slotsOnInterface[obj].RemoveItem();
                        
                    }    
                    break;
                case 1:
                    if (mouseHoverSlotData.CanPlaceInSlot(slotsOnInterface[obj].ItemObject))
                    {
                        
                        if (startInventoryID == 1)
                        {
                            player.GunSlot_swap(slotsOnInterface[obj].parent.inventory.ItemSlotNum(slotsOnInterface[obj])
                                , mouseHoverSlotData.parent.inventory.ItemSlotNum(mouseHoverSlotData));
                        }
                        else
                        {
                            Debug.Log("다른 인벤에서 캐릭터 인벤으로 들어옴");
                            Debug.Log(slotsOnInterface[obj].slot_item_object);
                            player.ItemSlotNum(mouseHoverSlotData.parent.inventory.ItemSlotNum(mouseHoverSlotData));
                            player.ItemPickup_inventory_System(slotsOnInterface[obj].ItemObject, slotsOnInterface[obj].slot_item_object);
                            Debug.Log("찾는것" + slotsOnInterface[obj].GetInventoryID());
                            player.Add_Item(mouseHoverSlotData.GetInventoryID(), slotsOnInterface[obj].item);
                            player.Item_Removed(slotsOnInterface[obj].parent.inventory, endInventoryID, slotsOnInterface[obj].item);

                        }
                        
                        //inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
                    }
                    break;
                case 2:
  /*                  if (mouseHoverSlotData.CanPlaceInSlot(slotsOnInterface[obj].ItemObject))
                    {

                    }*/
                    break;
                case 3:
                    if (mouseHoverSlotData.CanPlaceInSlot(slotsOnInterface[obj].ItemObject))
                    {
                        if (startInventoryID != 3)
                        {
                            player.ItemSlotNum(mouseHoverSlotData.parent.inventory.ItemSlotNum(mouseHoverSlotData));
                            player.ItemPickup_inventory_System(slotsOnInterface[obj].ItemObject, slotsOnInterface[obj].slot_item_object); 
                        }
                        inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
                        if (startInventoryID == 3)
                        {
                            player.GunSlot_swap(slotsOnInterface[obj].parent.inventory.ItemSlotNum(slotsOnInterface[obj])
                                , mouseHoverSlotData.parent.inventory.ItemSlotNum(mouseHoverSlotData));
                        }
                    }
                    break;

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
            if(clickItem == inventory.Container.Items[i])
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