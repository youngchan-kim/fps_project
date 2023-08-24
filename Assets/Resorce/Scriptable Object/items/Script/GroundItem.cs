using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;

    Transform PlayerObject;
    private void Start()
    {
        
        //Debug.Log("지금 찾는거" +GameMgr.Instance.player.transform.Find("PlayerObject"));
        PlayerObject = GameMgr.Instance.player.transform.Find("PlayerObject");
    }
    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        //GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        //EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
    }

    // 오브젝트 사이의 접촉이 일어난 순간 호출
    public void OnTriggerEnter(Collider other)
    {
        if (PlayerObject.name == other.name)
        {
            Item thisitem = new Item(item);
            GameMgr.Instance.player.GetComponent<Player>().On_The_Ground_Item(thisitem, gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (PlayerObject.name == other.name) 
        {
            Item thisitem = new Item(item);
            GameMgr.Instance.player.GetComponent<Player>().On_The_Ground_Item_Removed(thisitem);

        }
    }
    public ItemObject This_Item_info()
    {
        return item;
    }

}
