using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PickUpController
{
    //public MouseItem mouseItem = new MouseItem();
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;

    public GameObject playercam;
    //public GameObject Inventory;
    private bool mode_chage;

    private bool life;

    RaycastHit rayHit;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
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
        Debug.DrawRay(GameMgr.Instance.player.transform.localPosition, GameMgr.Instance.player.transform.GetChild(1).forward * 5f, Color.black, 0.2f);
        //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
        if (Physics.Raycast(GameMgr.Instance.player.transform.localPosition, GameMgr.Instance.player.transform.GetChild(1).forward, out rayHit,  4f, Item_Mask))
        {
            Debug.Log("보고있는 아이템");
            Debug.Log(rayHit.transform.parent.GetComponent<GroundItem>());
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickUp(rayHit);
            }
        }
        //itemDatabase save & load test code
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
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
        healthBar.SetHealth(currentHealth);
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
