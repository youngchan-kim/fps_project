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
    public Camera mainCamera;
    public GameObject scope_parts;
    private CinemachineVirtualCamera Cam;

    public float scopedFOV;
    private float normalFOV;
    private bool isScoped = false;
    private void Start()
    {
        Cam = GetComponent<CinemachineVirtualCamera>();
    }
    void Update()
    {

        //마우스 오른 버튼이 눌리면 
        if (Input.GetMouseButton(1))
        {
            isScoped = true;

            StartCoroutine(OnScoped());
        }
        else
        {
            isScoped = false;
            OnUnScoped();
        }
        animator.SetBool("is Scoped", isScoped);
    }
    public void OnUnScoped()
    {
        scope_parts.SetActive(false);
        scopeOverlay.SetActive(false);

        mainCamera.fieldOfView = normalFOV;
    }

    IEnumerator OnScoped()
    {
        
        yield return new WaitForSeconds(.25f);
        scope_parts.SetActive(true);
        normalFOV = mainCamera.fieldOfView;
        scopedFOV = 15f;
        if (scope_parts.GetComponentInChildren<Transform>().Find("scope"))
            scopeOverlay.SetActive(true); 
        else
            scopeOverlay.SetActive(false);


        mainCamera.fieldOfView = scopedFOV;
    }
}
