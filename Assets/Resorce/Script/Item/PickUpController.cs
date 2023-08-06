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

    //오브젝트와 플레이어의 거리
    private void Update()
    {

    }

    public virtual void PickUp()
    {

        //무기transform을 초기화 한뒤 건컨데이터의 자식으로 만든다
        //transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }

    protected Vector3 Drop()
    {

        //무기의 부모를 초기화한다.
        //transform.SetParent(ItemObject);
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 100f);
        hitPos = rayHit.point;
        hitPos.y += 0.001f;
        transform.localPosition = hitPos;
        return hitPos;
        //transform.localRotation = Quaternion.Euler(90,0,0);
    }
}
