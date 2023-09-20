using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : PickUpController
{
    //public MouseItem mouseItem = new MouseItem();
    public float maxHealth = 100f;
    public float currentHealth;
    float healvolume = 30;
    float MaxHealvolume = 80;

    public HPBar hpBar;


    public GameObject playercam;
    //public GameObject Inventory;
    private bool mode_chage;

    private bool life;

    RaycastHit rayHit;
    // Start is called before the first frame update
    //Player body Object
    //[HideInInspector] 
    public GameObject floor;

    Coroutine coroutine = null;
    void Start()
    {
        hpBar.SetMaxHealth(maxHealth);
    }

    public void Initialize()
    {
        life = true;
        currentHealth = maxHealth;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    private void Update()
    {
        //Debug.DrawRay(GameMgr.Instance.player.transform.localPosition, GameMgr.Instance.player.transform.GetChild(1).forward * 5f, Color.black, 0.2f);
        //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
        if (Physics.Raycast(GameMgr.Instance.player.transform.localPosition, GameMgr.Instance.player.transform.GetChild(1).forward, out rayHit,  4f, Item_Mask))
        {
            /*Debug.Log("보고있는 아이템");
            Debug.Log(rayHit.transform.parent.GetComponent<GroundItem>());*/
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickUp(rayHit);
            }
        }

        //itemDatabase save & load test code
        /*if (Input.GetKeyDown(KeyCode.End))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
        }*/
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1번눌림" );
            GunActive(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GunActive(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GunActive(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GunActive(3);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playercam.gameObject.SetActive(mode_chage);
            //Inventory.SetActive(!mode_chage);
            mode_chage = !mode_chage;  
        }

        if (Getlife())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(15);
            }
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                life = false;
            }
        }

        CuserControl(mode_chage);
        
    }
    void CuserControl(bool mode_chage)
    {
        //커서를 움직이지 않도록 하고 보이지 않도록 해야함.
        if (mode_chage == false)
        { 
            Cursor.lockState = CursorLockMode.Locked; 
        }

        else
        { 
            Cursor.lockState = CursorLockMode.Confined; 
        }
        Cursor.visible = mode_chage;
    }
    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        hpBar.SetHealth(currentHealth);
    }

    IEnumerator HpSliderAnimation(float _heal)
    {
        //최대 힐량
        float heal = _heal;
        //현재 체력을 Ui에 표시
        //hpBar.slider. = currentHealth;

        float t = 0.0f;
        //최대로 채울 수 있는 값 분에 3.0을 채우는 시간
        float elipsed = 1.0f / MaxHealvolume;   
        //현재 채력이 최소 회복가능한 값보다 작을 때
        while (currentHealth < MaxHealvolume)
        {
            //heal이 남은 량이 0보다 크다면
            if(heal > 0)
            {
                //
                if (heal > elipsed)
                {
                    heal -= elipsed;
                    t += elipsed;
                }
                else
                { 
                    t += heal;
                    heal = 0;
                }

            }
            //hpBar.GetComponent<HealthBar>().value = Mathf.Lerp(currentHealth, MaxHealvolume, t);
            yield return new WaitForSeconds(elipsed);
        }
        //hpBar.slider.value = currentHealth;
        Debug.Log("힐이 들어간 값 : "+ t);
        coroutine = null;
    }

    public void OnHpAnimtion(InventorySlot Slot)
    {
        if (null != coroutine) { StopCoroutine(coroutine); Slot.totalamount++; }
        coroutine = StartCoroutine(HpSliderAnimation(Slot.item.addHeal));
        Slot.totalamount--;


    }
    public bool Getlife()
    {
        return life;
    }

    //데이터 관리 클래스
    public void OnApplicationQuit()
    {
        //@슬롯
        Groundinventory.Container.Clear();
        inventory.Container.Clear();
        Equipinventory.Container.Clear();
        Guninventory.Container.Clear();
    }

}
