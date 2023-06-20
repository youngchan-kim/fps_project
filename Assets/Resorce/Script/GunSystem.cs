
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
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

    //Reference
    public Camera fpsCam;
    //공격할지점
    public Transform attackPoint;
    //공격한 곳
    public RaycastHit rayHit;
    //언떤 것을 공격했는지
    public LayerMask whatIsEnemy;


    private void Awake()
    {
        //탄창사이즈 만큼 남은 탄을 채워준다.
        bulletsLeft = magazineSize;
        //쏠 수 있는 상태
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        //버튼 눌림 체크가 false이면
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

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

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Spread될 방향을 계산
        //normal direction+ spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        //1인칭 기준으로 만들기 때문
        //접촉한 단일 개체의 정보를 얻어오기 위함
        //특정 위치에서 일정한 방향으로 광선을 발사

        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }
        bulletsLeft--;
        bulletsShot--;
        //총 쏘면서 바뀐 속성 리셋
        Invoke("ResetShot", timeBetweenShooting);

        //
        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
