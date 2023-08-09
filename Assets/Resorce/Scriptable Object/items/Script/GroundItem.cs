using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;

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
/*       if( GameMgr.GetCollierPlayer()== other)
        {
            GetCollierPlayer().
        }*/

    }

    public void OnTriggerExit(Collider other)
    {
       /* if (GetCollierPlayer() == other)
        {
            //manager.GetCollierPlayer().
        }*/
    }
}
