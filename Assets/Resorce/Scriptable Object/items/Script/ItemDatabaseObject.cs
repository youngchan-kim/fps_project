using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ISerializationCallbackReceiver
/// 직렬화 및 역 직렬화 시 콜백으 수신하기 위한 인터페이스 입니다.
/// 
/// 콜백 내에서 개체가 직접 소유한 필드만 처리햐여 처리 부감을 가능한 낮추는 것이 좋다.
/// 
/// 클래스 내부에서만 작동한다. 구조체에서는 작동하지 않는다.
/// 
/// OnBeforeSerialize 콜백이 참조된 개체에 의해 구현되기 전에 호출된다
/// 
/// OnAfterDeserialize는 참조된 모든 개체에서 호출된 후에 호출
/// 
/// 개체가 생성전에 OnBeforeSerialize를 이용하여 초기화작업
/// 개체 생성후 OnAfterDeserialize데이터 채워 넣기
/// </summary>

[CreateAssetMenu(fileName = "New Item Database", menuName = " Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject,ISerializationCallbackReceiver
{
    public ItemObject[] Items;
    public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<ItemObject, int>();
        GetItem = new Dictionary<int, ItemObject>();
        for (int i =0; i < Items.Length; i++)
        {
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}
