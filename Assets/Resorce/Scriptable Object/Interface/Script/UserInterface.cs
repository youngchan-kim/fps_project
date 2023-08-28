using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

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
        //만들어야 하는것 해당키가 없는 경우의 예외 처리 만들것
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
        {
            Debug.Log("오브젝트를 허공에 뒀을 때");

            if (slotsOnInterface[obj].GetInventoryID() != 0)
            {
                player.ClickItem(slotsOnInterface[obj].item);
                player.ItemDropSystem();
                slotsOnInterface[obj].RemoveItem();
                if(slotsOnInterface[obj].GetInventoryID() == 3)
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
            //Debug.Log("원래 있던 곳"+slotsOnInterface[obj].GetInventoryID());
            //Debug.Log("두는곳"+mouseHoverSlotData.GetInventoryID());
            Debug.Log("찾는것" + slotsOnInterface[obj].GetInventoryID());
            Debug.Log(slotsOnInterface[obj].AllowedItems);
            Debug.Log(slotsOnInterface[obj].item.GetType());
            Debug.Log("땅에 있던 오브젝트"+slotsOnInterface[obj].item.groundobject);
            Debug.Log("오브젝트를 둔 곳" + mouseHoverSlotData.GetInventoryID());
            Debug.Log("오브젝트를 둔 곳은 " + mouseHoverSlotData.parent.inventory.name);
            Debug.Log("오브젝트를 둔 곳은 " + mouseHoverSlotData.parent.inventory.ItemSlotNum(mouseHoverSlotData) + " 번째");
            Debug.Log("오브젝트를 가져온 곳은 " + slotsOnInterface[obj].parent.inventory.ItemSlotNum(slotsOnInterface[obj] )+ " 번째");

            switch (mouseHoverSlotData.GetInventoryID())
            {
                case 0:
                    player.ClickItem(slotsOnInterface[obj].item);
                    if (slotsOnInterface[obj].GetInventoryID() != 0)
                    {
                        player.ItemDropSystem();
                        player.RemoveGun(slotsOnInterface[obj].parent.inventory.ItemSlotNum(slotsOnInterface[obj]));
                        slotsOnInterface[obj].RemoveItem();
                    }    
                    break;
                case 1:
                    if (mouseHoverSlotData.CanPlaceInSlot(slotsOnInterface[obj].ItemObject))
                    {
                        if (slotsOnInterface[obj].GetInventoryID() != 1)
                        {
                            player.ItemSlotNum(mouseHoverSlotData.parent.inventory.ItemSlotNum(mouseHoverSlotData));
                            player.ItemPickup_inventory_System(slotsOnInterface[obj].ItemObject);
<<<<<<< Updated upstream
                            player.Add_Item(mouseHoverSlotData.GetInventoryID(), slotsOnInterface[obj].ItemObject_Data(), slotsOnInterface[obj].item.groundobject);

=======
                            player.Add_Item(mouseHoverSlotData.GetInventoryID(), slotsOnInterface[obj].item, slotsOnInterface[obj].item.groundobject);
                            
>>>>>>> Stashed changes
                            //inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
                            slotsOnInterface[obj].RemoveItem();

                        }
                        else if (slotsOnInterface[obj].GetInventoryID() == 1)
                        {
                            player.GunSlot_swap(slotsOnInterface[obj].parent.inventory.ItemSlotNum(slotsOnInterface[obj])
                                , mouseHoverSlotData.parent.inventory.ItemSlotNum(mouseHoverSlotData));
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
                        if (slotsOnInterface[obj].GetInventoryID() != 3)
                        {
                            player.ItemSlotNum(mouseHoverSlotData.parent.inventory.ItemSlotNum(mouseHoverSlotData));
                            player.ItemPickup_inventory_System(slotsOnInterface[obj].ItemObject); 
                        }
                        inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
                        if (slotsOnInterface[obj].GetInventoryID()==3)
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
            if (_slot.Value.item.Id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                //슬롯 아이템의 갯수가 1이면 숫자 표시 X 0이 아니면 숫자 표시
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount != 0 ? _slot.Value.amount.ToString("n0"): "";
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}