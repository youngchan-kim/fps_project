using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum Gun
{
    Empty,
    AK,
    Sniper
};

public class PickUpController : MonoBehaviour
{
    // public GunSystem gunScript;
    //public Scope scopeScript;
    public Transform player;
    //내려지는 레이어
    public LayerMask whatIsGround;
    //오브젝트를 얻을 수 있는 범위
    public float pickUpRange;
    //내려놓는 힘과 잡는 힘
    //public float dropForwardForce, dropUpwardForce;
    //장착 여부 체크
    //public bool equipped;
    //이미 총을 들고 있는지 확인
    //모든 스크립트에서 변경하기 위함
    //public static bool slotFull;

    //아이템을 두기 위한 위치
    //월드 포지션을 가지기 위함
    private Vector3 hitPos;

    //public Image Equit_icon;
    //public Sprite nomal_icon;
    //Sprite after_object;

    /*public bool GetEquipped()
    {
        return equipped;
    }*/

    //오브젝트와 플레이어의 거리
    /*   private void Update()
       {
           //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
           Vector3 distanceToPlayer = player.position - transform.position;
       }*/

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

    /*void name()
    {
        if (this.gameObject.name == "AK")
        {
            //Gun.AK;
        }
        else if (this.gameObject.name == "AK")
        {
            //Gun.Sniper;
        }
        else
        {
            //Gun.Empty;
        }
    }*/
}
