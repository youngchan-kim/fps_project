using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GunSystem gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam, ItemObject;

    //오브젝트를 얻을 수 있는 범위
    public float pickUpRange;
    //내려놓는 힘과 잡는 힘
    public float dropForwardForce, dropUpwardForce;
    //장착 여부 체크
    public bool equipped;
    //이미 총을 들고 있는지 확인
    //모든 스크립트에서 변경하기 위함
    public static bool slotFull;


    public bool GetEquipped()
    {
        return equipped;
    }
    private void Start()
    {
        //Setup
        switch(equipped)
        {
            case false:
                gunScript.enabled = false;
                rb.isKinematic = false;
                coll.isTrigger = false;
                break;

            case true:
                gunScript.enabled = true;
                rb.isKinematic = true;
                coll.isTrigger = true;
                slotFull = true;
                break;
        }
    }

    private void Update()
    {
        //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();

        //플레이어가 총을 가지고 있는지 체크와 G키를 통해 내려놓음
        if (equipped && Input.GetKeyDown(KeyCode.G)) Drop();
        
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //무기를 카메라의 자식으로 만든다
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //충돌에 대한 트리거 활성화
        //물리 운동 활성화
        rb.isKinematic = true;
        coll.isTrigger = true;

        //총의 스크립스 활성화
        gunScript.enabled = true;
    }

    private void Drop()
    { 
        equipped = false;
        slotFull = false;

        //무기의 부모를 초기화한다.
        transform.SetParent(ItemObject);

        //충돌에 대한 트리거 비활성화
        //물리 운동 비활성화
        rb.isKinematic = false;
        coll.isTrigger = false;

        //총을 버릴때의 버려지는 곳
        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        //총의 스크립스 비활성화
        gunScript.enabled = false;
    }
}
