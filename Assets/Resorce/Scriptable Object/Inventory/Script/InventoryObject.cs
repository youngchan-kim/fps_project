using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject
{
    
    public int InventoryID;
    public string savePath;
    //public List<ItemObject> Container = new List<ItemObject>(); 에서로 변경
    //private ItemDatabaseObject database;
    public ItemDatabaseObject database;
    //Inventory클래스의 명은 Container
    //Container를 사용하면 List Items를 사용하기위함
    public Inventory Container;

    //아이템 항목 추가 기능
    public bool AddItem(ItemObject _item, GameObject _gameObject, InventoryType inventoryType )
    {
        if (EmptySlotCount <= 0)
            return false;

        if(inventoryType == InventoryType.Ground)
        {
            SetEmptySlot(_item.data, _item.amount, _gameObject);
            return true;            
        }
        else if (inventoryType == InventoryType.other)
        {
            InventorySlot slot = FindItemOnInventory(_item.data);
            if (!database.Items[_item.data.Id].stackable || slot == null)
            {
                SetEmptySlot(_item.data, _item.amount, _gameObject);
                return true;
            }
            //slot.AddAmount(slot.itemobject_data, _item.amount);
            slot.AddPrantObject(_item.data, _gameObject);

        }
        return true;
    }

    //내부의 첫 번째 빈 슬롯을 찾는함수
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.Id == _item.Id)
            {
                return Container.Items[i];
            }
        }
        return null;
    }

    //빈슬롯에 설정
    public InventorySlot SetEmptySlot(Item _item, int _amount, GameObject _gameObject)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.Id <= -1)
            {
                Container.Items[i].UpdateSlot(_item, _amount, _gameObject);
                return Container.Items[i];
            }
        }
        //set up functionality for full inventory
        return null;
    }


    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.Id == -1)
            {
                return Container.Items[i];
            }
        }
        //full inventory
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        //Debug.Log("지금 찾는아이템인벤토리의 아이디");
        //Debug.Log(item1.GetInventoryID());
        //Debug.Log(item2.GetInventoryID());
        //Debug.Log("지금 찾는아이템");
        //Debug.Log(item1.ItemObject);
        //Debug.Log(item2.ItemObject);

        if (item2.CanPlaceInSlot(item1.ItemObject)&& item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }

    public int ItemSlotNum(InventorySlot sellectslot)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i] == sellectslot)
            {
                return i;
            }
        }
        return -1;
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++) 
        {
            if(Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(null, 0);
            }
        }
    }
    public void ClearItem(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            //문제 //아이디 값이 같은데 걍 넘어감
            if (Container.Items[i].item.Id == _item.Id)
            {
                //Container.Items[i].UpdateSlot(null, 0);
                Container.Items[i].item = new Item();
                //Container.Items[i].amount = 0;
            }
        }
    }

    //인벤토리 인스펙터 창에서 저장하기위해 ContextMenu에 노출
    [ContextMenu("Save")]
    public void Save()
    {
        //설명
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container); 
        stream.Close();
        Debug.Log("인벤토리 저장");
    }
    //
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for(int i =0; i<Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
            Debug.Log("인벤토리 로드");
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
}

[System.Serializable]
public class Inventory
{
    //Items명의 List생성 타입은 InventorySlot
    //List의 경우 게임 실행 중에 쉽게 추가와 제거가 가능하다는것 하지만 
    //public List<InventorySlot> Items = new List<InventorySlot>();
    //배열은 크기를 알아야 해당기능이 가능하다.
    //배열을 사용하려면 초기화때 배열의 크기를 설정해줘야한다.
    //처음에 배열의 크기를 8로 하지만 변경이 가능하다.
    //@슬롯
    public InventorySlot[] Items = new InventorySlot[28];
    public void Clear()
    {
        for(int i =0; i<Items.Length; i ++)
        {
            Items[i].RemoveItem();
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];

    [System.NonSerialized]
    public UserInterface parent;
    public string name;
    public Item item = new Item();
    public int amount;
    public GameObject parent_Object;
    //public ItemObject itemobject_data;


    /*public ItemObject ItemObject_Data()
    {
        if (item.Id >= 0)
        {
            item.groundobject = parent.inventory.database.Items[item.Id].data.groundobject;
            itemobject_data = parent.inventory.database.Items[item.Id];
            itemobject_data.data = item;
            return itemobject_data;
        }
        return null;
    }*/
    public ItemObject ItemObject
    {
        get
        {
            if(item.Id >= 0)
            {
                
                //item.groundobject = parent.inventory.database.Items[item.Id].data.groundobject;
                return parent.inventory.database.Items[item.Id];
            }
            return null;
        }
    }

    public int GetInventoryID()
    {
        return parent.inventory.InventoryID;
    }


    public InventorySlot()
    {
        item = new Item();
        amount = 0;
        //item.groundobject = null;
    }
    //스왑할때 사용
    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    //저장할때 주로 사용
    public void UpdateSlot(Item _item ,int _amount)
    {
        item = _item;
        amount = _amount;
    }
    //생성자와 같은 작업을 수행하는 업데이트 함수
    public void UpdateSlot(Item _item, int _amount, GameObject _grounditemobject)
    { 

        item = _item;
        amount = _amount;
        //item.groundobject = _grounditemobject;
    }
    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
        //item.groundobject = null;
    }
    public void AddAmount(ItemObject _item, int value)
    {
        //itemobject_data.amount += value;
        amount += _item.amount;
    }
    public void AddPrantObject(Item _item, GameObject prant_gameObject)
    {
        //_item.groundobject = prant_gameObject;
    }

    //허용된 슬롯만 가능
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0 || _itemObject == null ||_itemObject.data.Id < 0)
            return true;

        for(int i =0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}