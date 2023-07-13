using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Gun
{
    Empty,
    AK,
    Sniper
};

public class PickUpController : MonoBehaviour
{
    public GunSystem gunScript;
    public Scope scopeScript;
    public Transform player, gunContainer, fpsCam, ItemObject;
    //내려지는 레이어
    public LayerMask whatIsGround;
    //오브젝트를 얻을 수 있는 범위
    public float pickUpRange;
    //내려놓는 힘과 잡는 힘
    //public float dropForwardForce, dropUpwardForce;
    //장착 여부 체크
    public bool equipped;
    //이미 총을 들고 있는지 확인
    //모든 스크립트에서 변경하기 위함
    public static bool slotFull;
    
    private Vector3 hitPos;


    public Image Equit_icon;
    public Sprite nomal_icon;
    Sprite after_object;

    public bool GetEquipped()
    {
        return equipped;
    }
    private void Start()
    {
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

    private void Update()
    {       
        //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();

        //플레이어가 총을 가지고 있는지 체크와 G키를 통해 내려놓음
        if (equipped && Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //무기transform을 초기화 한뒤   건컨데이터의 자식으로 만든다
 
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        //총의 스크립스 활성화
        gunScript.enabled = true;
        scopeScript.enabled = true;
        Equit_icon.sprite = gunScript.GetSprite();
    }

    private void Drop()
    { 
        equipped = false;
        slotFull = false;

        //무기의 부모를 초기화한다.
        transform.SetParent(ItemObject);
        bool ground = Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit rayHit, 100f, whatIsGround);
        if(ground) Debug.Log("땅");
        hitPos = rayHit.point;
        hitPos.y = 0.0f;
        transform.localPosition = hitPos;
        transform.localRotation = Quaternion.Euler(90,0,0);

        //총의 스크립스 비활성화
        gunScript.enabled = false;
        scopeScript.OnUnScoped();
        scopeScript.enabled = false;
        Equit_icon.sprite = nomal_icon;
    }

    void name()
    {
        if (this.gameObject.name == "AK")
        {
            Gun.AK;
        }
        else if (this.gameObject.name == "AK")
        {
            Gun.Sniper;
        }
        else
            Gun.Empty;
    }
}
