using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

//오브젝트를 습득한 경우
//인벤토리네에 오브젝트의 이미지를 넣어주고 갯수를 중첩해주는 코드이다.
public abstract class UserInterface : MonoBehaviour
{
    public Player player;

    public InventoryObject inventory;

    //아이템 슬롯 값과 게임 오브젝트의 값을 가지는 Dictionary
    //슬롯을 키로 사용하고 게임 오브젝트의 개체를 값으로 사용했지만 
    //반대로 슬롯을 값으로 게임 오브젝트를 키로 사용할 것
    /// <summary>
    /// 이렇게 하면 디스플레이에 생성한 프리펩을 클릭했을 때
    /// 슬롯에 있는 게임 개체를 전환한다면 
    /// 해당 인벤토리 슬롯의 실제 데이터 표현에 대한 좋은 링크를 제공하므로
    /// 좋은 방법
    /// 프리펩이 가진 슬롯이 무엇인지 알기 편하다. 그리고 슬롯을 변경하기 용의하다.
    /// </summary>
    /// 디스플레이 코드와 인벤토리 코드를 완전히 분리하기 위함

    //디스플레이 코드는 백엔드에서 일어나는 일을 시각적으로 보여만 주기 위함
    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parent = this;
        }
        CreateSlots();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }
    //이미지가 인벤토리에 없는 경우 생성해주는 코드
    public abstract void CreateSlots();
    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {

            //항목이 있는 경우
            if (_slot.Value.ID >= 0)
            {

                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            //없는 경우
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";

            }
        }
    }


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
        player.mouseItem.hoverobj = obj;

        if (itemsDisplayed.ContainsKey(obj))
        {
            player.mouseItem.hoverItem = itemsDisplayed[obj];
        }

    }
    public void OnExit(GameObject obj)
    {
        player.mouseItem.hoverobj = null;
        player.mouseItem.hoverItem = null;
    }
    public void OnDragStart(GameObject obj)
    {
        //
        var mouseObject = new GameObject();
        //마우스 오브젝트의 Transform
        var rt = mouseObject.AddComponent<RectTransform>();
        //마우스 오브젝트로 잡은 이미지의 크기
        rt.sizeDelta = new Vector2(200, 100);
        mouseObject.transform.SetParent(transform.parent);
        //클릭한 오브젝트의 슬롯의 아이디가 0이 아니면
        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }
        //마우스가 선택한 오브젝트
        //mouseObject는 마우스아이템의 오브젝트
        player.mouseItem.obj = mouseObject;
        //마우스가 선택한 오브젝트의 아이템
        player.mouseItem.item = itemsDisplayed[obj];
    }
    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = player.mouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHoverObj = itemOnMouse.hoverobj;
        var GetItemObject = inventory.database.GetItem;

        //아이템끼리의 위치를 교환
        if (player.mouseItem.hoverobj)
        {
            inventory.MoveItem(itemsDisplayed[obj], mouseHoverItem.parent.itemsDisplayed[itemOnMouse.hoverobj]);
        }
        else
        {
            //inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        Debug.Log(player.mouseItem.item.ID + "마우스가 놓은 곳의 아이디");
        Destroy(itemOnMouse.obj);
        itemOnMouse.item = null;
    }
    public void OnDrag(GameObject obj)
    {
        if (player.mouseItem.obj != null)
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
    }

   
}

public class MouseItem
{
    //오브젝트
    public GameObject obj;
    //아이템
    public InventorySlot item;
    //마우스가 위에 있는  아이템
    public InventorySlot hoverItem;
    //마우스가 위에 있는 오브젝트
    public GameObject hoverobj;
}