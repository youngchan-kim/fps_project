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
    GameObject haveitem;
    //오브젝트와 플레이어의 거리
    private void Update()
    {

    }

    public virtual void PickUp()
    {
        Debug.Log(transform.GetChild(0).GetChild(0).GetChild(0).name); 
        haveitem = GetPickupItem().gameObject;
        itemname = haveitem.name.ToString();
        
        //아이템의 이름과 같은 이름의 자식을 찾아라
        if(transform.GetChild(0).GetChild(0).GetChild(0).name == itemname)
            transform.GetChild(0).GetChild(0).GetChild(0).Find(haveitem.transform.GetChild(0).name).gameObject.SetActive(true); 
               
        //충돌한 아이템을 비활성화한다.
        haveitem.SetActive(false);
       
    }

    protected Vector3 Drop()
    {
        transform.Find(itemname).gameObject.SetActive(false);
        haveitem.SetActive(true);
        Physics.Raycast(haveitem.transform.position, Vector3.down, out RaycastHit rayHit, 100f);
        hitPos = rayHit.point;
        hitPos.y += 0.001f;
        haveitem.transform.localPosition = hitPos;
        return hitPos;
    }
}
