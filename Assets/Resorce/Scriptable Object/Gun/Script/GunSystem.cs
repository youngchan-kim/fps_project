
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunSystem : MonoBehaviour
{
    public Gun_Scriptable Gun_property;

    //GunPickUp pickup = null;

    //공격할지점
    [SerializeField]
    public AimShaker firePosition;
    [SerializeField]
    public Transform attackPoint;

    //총이 맞는 곳의 이펙트
    [SerializeField]
    public GameObject bulletHoleGraphic;

    //남은 탄 표시
    [SerializeField]
    public TextMeshProUGUI text;

    //사격 이펙트
    [SerializeField]
    public ParticleSystem muzzleFlashparticle;

    //남은 탄창, 쏠수 있는 탄
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;
    bool shoot;

    //공격한 곳
    public RaycastHit rayHit;
 
    public int Equipped_parts_Scopes;
    //총구가 가리키는 포지션
    Vector3 target_position;
    //촐구가 바라보는 방향
    Vector3 direction;

    public ItemObject bullet;
    float total_reloadTime;
    int total_magazineSize;
    float total_timeBetweenShooting;

    Player player;
    private void Awake()
    { 
        player = GameMgr.Instance.player.GetComponent<Player>();
        //pickup = GetComponent<GunPickUp>();
        //탄창사이즈 만큼 남은 탄을 채워준다.
        bulletsLeft = 0;
        //쏠 수 있는 상태
        readyToShoot = true;
    }
    public int CheckGunParts()
    {
        return Equipped_parts_Scopes;
    }

    private void Update()
    {
        //Debug.Log(GameMgr.Instance.player.GetComponent<Player>().GetEquipped());
        //Debug.Log(GameMgr.Instance.player.GetComponent<Player>().GetGunslotFull());
        if (GameMgr.Instance.player.GetComponent<Player>().GetGunSlotEmpty())
        {
            transform.LookAt(attackPoint.transform.position);
            Debug.DrawLine(firePosition.transform.position, attackPoint.transform.position, Color.red);
            direction = transform.forward;
            firePosition.transform.forward = direction;

            MyInput();
            //SetText
            text.SetText(bulletsLeft + "/" + player.GetInven_Find_Item(bullet));
        }
    }


    private void MyInput()
    {
        //연사와 단발을 B키를 통해 조작할 수 있다.
        if (Input.GetKeyDown(KeyCode.B))
            if (Gun_property.allowButtonHold)
                Gun_property.allowButtonHold = false;
            else Gun_property.allowButtonHold = true;
        /*if (!Input.GetKeyUp(KeyCode.Mouse0))*/
        //버튼 눌림 체크가 false이면 shooting은 눌렸을 때 ture;
        //연사 가능 일때
        if (Gun_property.allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
            if (Input.GetKey(KeyCode.Mouse0) && shoot)
            {
                StartCoroutine(firePosition.AimShake(Gun_property.spread));
                muzzleFlashparticle.Play();
                shoot = false;
            }
        }
        //연사 불가는 일때
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
            if (Input.GetKeyDown(KeyCode.Mouse0) && shoot)
            {
                StartCoroutine(firePosition.AimShake(Gun_property.spread));
                muzzleFlashparticle.Play();
                shoot = false;
            }

        }

        //R키가 눌렸고 남은 탄이 탄창보다 작고 리로드 중이 아닐때 호출한다.
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < Gun_property.magazineSize && !reloading) Reload();

        //Shoot
        //쏠준비됨 혹은 슈팅중이고 재장전중이지 않으며 장전된 탄의 수가 0보다 클때
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = Gun_property.bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //RayCast
        //1인칭 기준으로 만들기 때문
        //접촉한 단일 개체의 정보를 얻어오기 위함
        //특정 위치에서 일정한 방향으로 광선을 발사
        Vector3 firedirection = attackPoint.transform.position - firePosition.transform.position;
        if (Physics.Raycast(firePosition.transform.position, firedirection, out rayHit, Gun_property.range, Gun_property.what_can_Shoot))
        {
            //Debug.Log(rayHit.collider.name);
            if (rayHit.collider.GetComponent<ShootingAi>())

                rayHit.collider.GetComponent<ShootingAi>().TakeDamage(Gun_property.damage);

            var t = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));

        }

        //Debug.DrawLine(firePosition.transform.position, rayHit.point, Color.blue);


        /*Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);*/
        //Instantiate(muzzleFlashparticle, attackPoint.transform.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        //총 쏘면서 바뀐 속성 리셋
        total_timeBetweenShooting = Gun_property.timeBetweenShooting + 0;
        Invoke("ResetShot", total_timeBetweenShooting);

        //총을 쏠때 연사시간
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", Gun_property.timeBetweenShots);
        }
        shoot = false;
    }


    private void ResetShot()
    {
        readyToShoot = true;
        shoot = true;
    }


    private void Reload()
    {
        reloading = true;
        total_reloadTime = Gun_property.reloadTime + 0;
        Invoke("ReloadFinished", total_reloadTime);
    }


    private void ReloadFinished()
    {
        total_magazineSize = Gun_property.magazineSize + 0;
        int haveBullet = player.GetInven_Find_Item(bullet);
        int addBullet = 0;
        addBullet = total_magazineSize - bulletsLeft;

        if (haveBullet <= addBullet)
        {
            bulletsLeft = bulletsLeft + haveBullet;
            player.SetInven_Find_Item(bullet, 0);
        }
        else
        {
            bulletsLeft = addBullet;
            player.SetInven_Find_Item(bullet, haveBullet- addBullet);
        }
        reloading = false;
        player.InventRefresh();
    }

    public Sprite GetSprite()
    {
        return Gun_property.sprites;
    }

}
