using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : PickUpController
{
    [Serialize]
    public HP_System hp;
    DamegeSystem damegeSystem;
    //maxHealth, MaxHealvolume을 스크립터블 오브젝트로 만들어 놓기
    float maxHealth = 100f;
    float maxHealvolume = 80f;
    bool life;
    public HPBar hpBar;


    public GameObject playercam;
    public GameObject InventoryUI;
    public bool Hold_contorl_check;
    private bool mode_chage;


    RaycastHit rayHit;
    Transform Gun;
    GunSystem gunSystem;


    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        Gun = transform.GetChild(0).GetChild(0).GetChild(0)
            .GetChild(2).GetChild(0).GetChild(0)
            .GetChild(2).GetChild(0).GetChild(0)
            .GetChild(0).GetChild(4);

        // 게임 시작 시 모든 총기 비활성화
        for (int i = 0; i < Gun.childCount; i++)
        {
            Gun.GetChild(i).gameObject.SetActive(false);
        }

        // 슬롯 등록 정보 초기화
        for (int i = 0; i < Gunslot.Length; i++)
        {
            Gunslot[i] = null;
        }

        GunSystem_chage();

        damegeSystem = GetComponent<DamegeSystem>();
        //Debug.Log(GameMgr.Instance.GUI_Mgr.transform.GetChild(0).GetChild(6).name);
        InventoryUI = GameMgr.Instance.GUI_Mgr.transform.GetChild(0).GetChild(6).gameObject;
        //hpBar.SetMaxHealth(maxHealth);
        Hold_contorl_check = false;
    }

    public void Initialize()
    {
        life = true;
        hp.Initialize(maxHealth, maxHealvolume, life);
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    private void Update()
    {
        /*Debug.Log("케릭터 위치" + transform.position);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 100f, Dropzon_layermask))
        {
            
            Debug.Log("케릭터 바닥 위치" + rayHit.point);
        }*/
        /*Debug.Log(Hold_contorl_check);*/

        //이제 총을 장착하지 않은 상태에서 사격 입력이 들어와도 NullReferenceException이 발생하지 않음
        GunSystem_chage();

        if (Hold_contorl_check == false && gunSystem != null)
        {
            if (gunSystem.HoldButtonUse())
            {
                gunSystem.ClickToShoot(Input.GetKey(KeyCode.Mouse0));
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (gunSystem.HasAmmo() && gunSystem.ReadyToShoot() && gunSystem.FiringIsPossible())
                    {
                        //anim.Shoot();
                        gunSystem.Firing();
                    }
                }
            }
            else
            {
                gunSystem.ClickToShoot(Input.GetKeyDown(KeyCode.Mouse0));
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (gunSystem.HasAmmo() && gunSystem.ReadyToShoot() && gunSystem.FiringIsPossible())
                    {
                        //anim.Shoot();
                        gunSystem.Firing();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.R) && gunSystem.ReloadIsPossible())
                gunSystem.Reload();
        }
        //Debug.DrawRay(GameMgr.Instance.player.transform.localPosition, GameMgr.Instance.player.transform.GetChild(1).forward * 5f, Color.black, 0.2f);
        //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
        if (Physics.Raycast(GameMgr.Instance.player.transform.localPosition, GameMgr.Instance.player.transform.GetChild(1).forward, out rayHit, 4f, Item_Mask))
        {
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
            InventoryUI.SetActive(!mode_chage);
            mode_chage = !mode_chage;
            CuserControl(mode_chage);
        }
    }

    //활성화된 총이 없으면 gunSystem을 null로 설정
    //GunSystem 컴포넌트가 없는 오브젝트를 건너뜀
    public void GunSystem_chage()
    {
        gunSystem = null;

        if (Gun == null)
            return;

        for (int i = 0; i < Gun.childCount; i++)
        {
            Transform gun = Gun.GetChild(i);

            if (!gun.gameObject.activeInHierarchy)
                continue;

            GunSystem currentGun = gun.GetComponent<GunSystem>();

            if (currentGun != null)
            {
                gunSystem = currentGun;
                break;
            }
        }
    }

    public GunSystem GetGunSystem()
    {
        return gunSystem;
    }

    void CuserControl(bool mode_chage)
    {
        //커서를 움직이지 않도록 하고 보이지 않도록 해야함.
        if (mode_chage == false) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = mode_chage;
        Hold_contorl_check = !Hold_contorl_check;
    }



    public bool Getlife()
    {
        return hp.GetLife();
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
