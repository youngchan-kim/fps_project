using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = " Inventory System/Inventory")]

public class InventoryObject : ScriptableObject

{
    public string savePath;
    //public List<ItemObject> Container = new List<ItemObject>(); 에서로 변경
    //private ItemDatabaseObject database;
    public ItemDatabaseObject database;
    //Inventory클래스의 명은 Container
    //Container를 사용하면 List Items를 사용하기위함
    public Inventory Container;
    private void OnEnable()
    {
        //유니티 에디터를 사용하여 데이터 베이스에 접근려할 때 오류에 대한 해결책
        //해당 데이터 베이스의 위치에 로드에셋 매뉴를 사용하고
        /*#if UNITY_EDITOR
                database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
        #else
                database = Resources.Load<ItemDatabaseObject>("Database");
        #endif*/
    }

    //아이템 항목 추가 기능
    public void AddItem(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == _item.Id)
            {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        //넣고 싶은 아이템과 수량
        SetEmptySlot(_item, _amount);
    }
    //내부의 첫 번째 빈 슬롯을 찾는함수
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.Id, _item, _amount);
                return Container.Items[i];
            }
        }
        //인벤토리가 가득 차면 null 리턴
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(Item _item)
    {
        for(int i =0; i<Container.Items.Length; i++)
        {
            if(Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(-1, null, 0);
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
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount);
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
    public InventorySlot[] Items = new InventorySlot[6];
    public void Clear()
    {
        for(int i =0; i<Items.Length; i ++)
        {
            Items[i].UpdateSlot(-1, new Item(), 0);
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    public UserInterface parent;
    public int ID = -1;
    public Item item;
    public int amount;
    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    //생성자와 같은 작업을 수행하는 업데이트 함수
    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }

    //허용된 슬롯만 가능
    public bool CanPlaceInSlot(ItemObject _item)
    {
        if (AllowedItems.Length <= 0)
            return true;

        for(int i =0; i < AllowedItems.Length; i++)
        {
            if (_item.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}