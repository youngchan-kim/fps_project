using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Scope : MonoBehaviour
{
    enum Scopes
    {
        scope,
        holo,
        reddot
    };
    public Animator animator;

    public GameObject scopeOverlay;
    //public GameObject weaponcamera;
    public Camera mainCamera;
    public GameObject site;
    private CinemachineVirtualCamera Cam;

    //public GunSystem checkparts;

    public float scopedFOV;
    private float normalFOV;
    private bool isScoped = false;
    /*private int PartsNum = -1;*/
    private void Start()
    {
        Cam = GetComponent<CinemachineVirtualCamera>();
    }
    void Update()
    {
        
        //마우스 오른 버튼이 눌리면 
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //PartsNum = checkparts.CheckGunParts();
            //bool값이 반다로 바뀐다.
            isScoped = !isScoped;
            animator.SetBool("is Scoped", isScoped);
            if (isScoped)
                StartCoroutine(OnScoped());
            else
                OnUnScoped();
        }
    }

    void OnUnScoped()
    {
        site.SetActive(false);
        scopeOverlay.SetActive(false);
       // weaponcamera.SetActive(true);

        mainCamera.fieldOfView = normalFOV;
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);
        site.SetActive(true);
        normalFOV = mainCamera.fieldOfView;
        scopedFOV = 15f;
        //scopeOverlay.SetActive(true);
        /*switch (PartsNum)
        {
            case 0:
                scopedFOV = 15f;
                virtualcamera.Follow = site;
                scopeOverlay.SetActive(true);
                break;
           *//* case (int)Scopes.holo:
                scopedFOV = 10;
                break;
            case (int)Scopes.reddot:
                scopedFOV = 10;
                break;*//*

            default:
                scopedFOV = 10;
                scopeOverlay.SetActive(false);
                break;

        }*/

        //weaponcamera.SetActive(true);

        mainCamera.fieldOfView = scopedFOV;
    }

}
