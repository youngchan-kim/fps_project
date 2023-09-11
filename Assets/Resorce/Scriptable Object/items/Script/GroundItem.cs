using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;
    //데이터 바뀌도록할것
    public int amount=-1;
    Transform PlayerObject;
    private void Start()
    {
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
/*        if(amount==-1)
        {
            amount = item.data.amount;
        }*/
        if (PlayerObject.name == other.name)
        {
            item.data.item_object = gameObject;
            if (amount == -1)
            {
                amount = item.data.amount;

            }
            GameMgr.Instance.player.GetComponent<Player>().On_The_Ground_Item(item/*.data*/);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (PlayerObject.name == other.name) 
        {
            amount = item.objectamount;
            GameMgr.Instance.player.GetComponent<Player>().On_The_Ground_Item_Removed(item);
        }
    }
}
