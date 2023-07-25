using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//오브젝트를 습득한 경우
//인벤토리네에 오브젝트의 이미지를 넣어주고 갯수를 중첩해주는 코드이다.
public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    public int X_START;
    public int Y_START;

    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;

    //아이템 슬롯 값과 게임 오브젝트의 값을 가지는 Dictionary
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        //오류
        for(int i =0; i < inventory.Container.Count; i++)
        {
            if(itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }
    }

    //이미지가 인벤토리에 없는 경우 생성해주는 코드
    public void CreateDisplay()
    {

        for(int i = 0; i < inventory.Container.Count; i++)
        {
            //아이템의 프리펩을 생성
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");

        }
    }
    public Vector3 GetPosition(int i)
    {
        //이미지의 위치를 잡아주는 코드
        return new Vector3(X_START+(X_SPACE_BETWEEN_ITEM * (i %NUMBER_OF_COLUMN)), Y_START + ( - Y_SPACE_BETWEEN_ITEM *(i/NUMBER_OF_COLUMN)), 0f);
    }
}
