using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Animator animator;

    public GameObject scopeOverlay;
    private bool isScoped = false;
    void Update()
    {
        //마우스 오른 버튼이 눌리면 
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            //bool값이 반다로 바뀐다.
            isScoped = !isScoped;
            animator.SetBool("is Scoped", isScoped);
            scopeOverlay.SetActive(isScoped);
        }
    }
}
