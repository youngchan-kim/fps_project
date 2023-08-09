using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunPickUp : PickUpController
{
    //총기 시스템을 가지고 있는 object를 연결
    public GunSystem gunScript;
    //스코프 시스템을 가지고 있는 object를 연결
    public Scope scopeScript;
    //object가 있게 될 위치
    public Transform gunContainer;
    //object의 장착 여부
    public bool equipped;
    //이미 총을 들고 있는지 확인
    //모든 스크립트에서 변경하기 위함
    public static bool slotFull;

    public Image Equit_icon;
    public Sprite nomal_icon;
    Sprite after_object;


    public bool GetEquipped()
    {
        return equipped;
    }

    void Start()
    {
        transform.localPosition = Drop();

        //Setup
        switch (equipped)
        {
            case false:
                gunScript.enabled = false;
                scopeScript.enabled = false;

                break;

            case true:

                gunScript.enabled = true;
                scopeScript.enabled = true;
                slotFull = true;
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
        //if (!equipped && Player_recognizes() && Input.GetKeyDown(KeyCode.F) && !slotFull) PickUpItem();

        //플레이어가 아이템을 가지고 있는지 체크와 G키를 통해 내려놓음
        if (equipped && Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }
    }

    private void PickUpItem()
    {

        equipped = true;
        slotFull = true;

        //무기transform을 초기화 한뒤 건컨데이터의 자식으로 만든다

        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        //총의 스크립스 활성화
        gunScript.enabled = true;
        scopeScript.enabled = true;
        Equit_icon.sprite = gunScript.GetSprite();
    }

    private void DropItem()
    {
        equipped = false;
        slotFull = false;

        //무기의 부모를 초기화한다.
        transform.SetParent(null);
        transform.localPosition = Drop();
        transform.localRotation = Quaternion.Euler(90, 0, 0);

        //총의 스크립스 비활성화
        gunScript.enabled = false;
        scopeScript.OnUnScoped();
        scopeScript.enabled = false;

        Equit_icon.sprite = nomal_icon;
    }
}
