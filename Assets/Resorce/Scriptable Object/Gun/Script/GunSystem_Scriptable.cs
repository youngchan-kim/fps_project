
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun System/Gun")]
public class GunSystem_Scriptable : ScriptableObject
{
    GunPickUp pickup = null;
    //GunSystem의 속성
    public int damage;
    //총의 제어권, 확산, 범위, 재장전 시간, 연사속도
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    //탄창의 사이즈, 한번 누를때 발사하는 총알의 수
    public int magazineSize, bulletsPerTap;
    //버튼이 눌렸는지 확인하기 위함
    public bool allowButtonHold;

    //남은 탄창, 쏠수 있는 탄
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;
    bool shoot = true;
    //Reference
    //public Transform fpsCam;
    //공격할지점
    /*public Transform attackPoint;*/
    public AimShaker firePosition;
    public Transform attackPoint;

    //공격한 곳
    public RaycastHit rayHit;
    //언떤 것을 공격했는지
    public LayerMask whatIsEnemy;

    //Graphics
    public GameObject bulletHoleGraphic;
    //public CameraShake camShake;
    //public float ShakeMagnitude, ShakeDuration;
    public TextMeshProUGUI text;
    public ParticleSystem muzzleFlashparticle;
    public int Equipped_parts_Scopes;
    //private bool Equipped_parts_Holo;
    //private bool Equipped_parts_RedDot;

    //총구가 가리키는 포지션
    Vector3 target_position;
    //촐구가 바라보는 방향
    //Vector3 direction;
    public Sprite sprites;
    private void Awake()
    {
        //pickup = GetComponent<GunPickUp>();
        //탄창사이즈 만큼 남은 탄을 채워준다.
        bulletsLeft = magazineSize;
        //쏠 수 있는 상태
        readyToShoot = true;
    }
    public int CheckGunParts()
    {
        return Equipped_parts_Scopes;
    }

    private void Update()
    {
        if (pickup.GetEquipped())
        {
            //transform.LookAt(attackPoint.transform.position);
            Debug.DrawLine(firePosition.transform.position, attackPoint.transform.position, Color.red);
            //direction = transform.forward;
           //firePosition.transform.forward = direction;

            MyInput();
            //SetText
            text.SetText(bulletsLeft + "/" + magazineSize);
        }
        //Target();
    }


    private void MyInput()
    {
        //연사와 단발을 B키를 통해 조작할 수 있다.
        if (Input.GetKeyDown(KeyCode.B))
            if (allowButtonHold)
                allowButtonHold = false;
            else allowButtonHold = true;
        /*if (!Input.GetKeyUp(KeyCode.Mouse0))*/
        //버튼 눌림 체크가 false이면 shooting은 눌렸을 때 ture;
        //연사 가능 일때
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
            if (Input.GetKey(KeyCode.Mouse0) && shoot)
            {
                //StartCoroutine(firePosition.AimShake(spread));
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
                //StartCoroutine(firePosition.AimShake(spread));
                muzzleFlashparticle.Play();
                shoot = false;
            }

        }

        //R키가 눌렸고 남은 탄이 탄창보다 작고 리로드 중이 아닐때 호출한다.
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        //쏠준비됨 혹은 슈팅중이고 재장전중이지 않으며 장전된 탄의 수가 0보다 클때
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
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

        if (Physics.Raycast(firePosition.transform.position, attackPoint.transform.position, out rayHit, range))
        {
            //Debug.Log(rayHit.collider.name);
            if (rayHit.collider.GetComponent<ShootingAi>())
                rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }
        //Debug.DrawLine(firePosition.transform.position, rayHit.point, Color.blue);

        //Graphics
        //프리펩혹은 Hierarchy에 있는 GameObject를 복제(생성)및 삭제 할 수 있다.
        //많은 비용이 들기 때문에 Update()에서 호출하지 않는 것이 좋다.
        //복제하려는 오브젝트는 bulletHoleGraphic이고 만들 위치는 rayHit.point,
        // 만들어진 오브젝트의 회전은 Quaternion.Euler(0, 180, 0)이다.
        //Quaternion.LookRotation()
        var t = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));

        /*Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);*/
        Instantiate(muzzleFlashparticle, attackPoint.transform.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        //총 쏘면서 바뀐 속성 리셋
        //Invoke("ResetShot", timeBetweenShooting);

        //총을 쏠때 연사시간
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            //Invoke("Shoot", timeBetweenShots);
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
        //Invoke("ReloadFinished", reloadTime);
    }


    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public Sprite GetSprite()
    {
        return sprites;
    }
}
