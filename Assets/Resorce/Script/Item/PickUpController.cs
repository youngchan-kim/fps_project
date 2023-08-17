using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : Player_recognizes_Item
{
    // public GunSystem gunScript;
    //public Scope scopeScript;
    //내려지는 레이어
    public LayerMask whatIsGround;

    //아이템을 두기 위한 위치
    //월드 포지션을 가지기 위함
    private Vector3 hitPos;

    string itemname;
    GroundItem haveitem;
    GameObject Activeitem;

    //총

    //object의 장착 여부
    public bool equipped;
    //모든 스크립트에서 변경하기 위함
    public static bool GunslotFull;

    public virtual void PickUp()
    {
        //충돌한 아이템
        ItemPickupSystem();
        Swap_Item();
    }

    public void ItemPickupSystem()
    {
        haveitem = GetPickupItem();
        itemname = haveitem.gameObject.transform.GetChild(0).name;
        Activeitem = ListActiveCheck(haveitem.name);
        //Debug.Log("지금 찾는것" + haveitem.name);
        //총인경우 체크
        if (haveitem.name == "Gun")
        {
            //플레이어가 총을 장착 여부와 슬롯이 차있는지 여부 확인
            if (!equipped && !GunslotFull)
            {
                Activeitem.transform.Find(itemname).gameObject.SetActive(true);
                equipped = true;
                GunslotFull = true;
            }
        }
        else
        {
            Activeitem.transform.Find(itemname).gameObject.SetActive(true);
        }


        //충돌한 아이템을 비활성화한다.
        haveitem.gameObject.SetActive(false);
    }




    protected Vector3 Drop()
    {
        Debug.Log(transform.GetChild(1).GetChild(0).GetChild(0).name);

        if (haveitem.name == "Gun")
        {
            //플레이어가 총을 장착 여부와 슬롯이 차있는지 여부 확인
            if (equipped)
            {
                equipped = false;
                GunslotFull = false;
                ItemActiveCheck().SetActive(false); 
            }
        }
        else
        {
            ItemActiveCheck().SetActive(false);
        }
        


        haveitem.gameObject.SetActive(true);
        Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit rayHit, 100f);
        hitPos = rayHit.point;
        hitPos.y += 0.01f;
        haveitem.transform.position = hitPos;
        return hitPos;
    }



    //활성화된 오브젝트 리턴
    GameObject ItemActiveCheck()
    {
        int ItemListcount = transform.GetChild(1).GetChild(0).GetChild(0).childCount;
        Debug.Log(ItemListcount);

        for(int i =0; i < ItemListcount; i++)
        {
            if (transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject.activeSelf == true)
            {
                return transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject;
            }
        }
        return null;
    }

    //목록체크
    GameObject ListActiveCheck(string name)
    {
        int ListListcount = transform.GetChild(1).GetChild(0).childCount;
        //Debug.Log(ListListcount);

        for (int i = 0; i < ListListcount; i++)
        {
            if (transform.GetChild(1).GetChild(0).GetChild(i).gameObject.name == name)
            {
                return transform.GetChild(1).GetChild(0).GetChild(i).gameObject;
            }
        }
        return null;
    }

    public bool GetEquipped()
    {
        //Debug.Log("장착 : " + equipped);
        return equipped;
    }

    public bool GetGunslotFull()
    {
        Debug.Log("장착 : " + GunslotFull);
        return GunslotFull;
    }

}
