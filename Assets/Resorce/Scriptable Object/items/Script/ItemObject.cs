using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{

    //장착아이템 test용
    Equipment,
    //방탄복
    Armor,
    //가방
    Bag,
    //치료아이템
    Medical,    
    //총알
    Bullet,
    //itemtest용
    Food,
    Helmet,
    Weapon,
    Shield,
    Boots,
    Chest,
    //기타
    Default
}

//당장은 쓰지 않는 버프 종류
//도핑시스템에 사용됨
//스피드업
//도핑치유
public enum Attributes
{ 
    Dopping
}
public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite uiDisplay;
    //아이템의 오브젝트
    //public GameObject prefab;
    //아이템 타입
    public ItemType type;

    [TextArea(15, 20)]
    public string description;
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    //public ItemBuff[] buffs;
    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;

/*        buffs = new ItemBuff[item.buffs.Length];
        for(int i =0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max)
            {
                Attribute = item.buffs[i].attribute
            };
        }
*/
    }
}
