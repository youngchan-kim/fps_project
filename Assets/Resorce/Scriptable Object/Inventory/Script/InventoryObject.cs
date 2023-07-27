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
    public void AddItem(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            //Container.Items[i].item의 구조체의 값들과_item의 값이 같은데 오류가 남 값이 같지 않다고 뜸
            //원래 코드 if(Container.Items[i].item. == _item)
            if (Container.Items[i].item.Id == _item.Id)
            {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }

        Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));
    }

    //인벤토리 인스펙터 창에서 저장하기위해 ContextMenu에 노출
    [ContextMenu("Save")]
    public void Save()
    {
        /*string saveData = JsonUtility.ToJson(this, true);
        //설명
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();*/
        //설명
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create,FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();

    }
    //
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Container = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }
}

[System.Serializable]
public class Inventory
{
    //Items명의 List생성 타입은 InventorySlot
    public List<InventorySlot> Items = new List<InventorySlot>();
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    public int amount;
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}