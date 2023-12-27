using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 바라보는 방향을 카메라가 바라보게 하기위함
//플레이어의 앞면을 가리키는 플레이어.foward 변수를 통해서 케릭터가 바라보는 방향을 알 수 있다. 
public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    //
    public Transform orientation;

    public Transform player;

    public Transform playerObj;

    public Rigidbody rb;

    public float rotationSpeed;
/*
    public Transform RightLookAt;
    public Transform LeftLookAt;*/

    public GameObject TPC;
/*    public GameObject rightWaveCam;
    public GameObject leftWaveCam;*/

    protected CameraStyle curentStyle;
    public enum CameraStyle
    {
        Basic
    }

    private void Start()
    {
        TPC = GameObject.FindWithTag("TPC_Cam");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //카메라에서 플레이어까지의 방향을 계산하여 전방이 어디인지 알아내기
        Vector3 viewDir = player.position - new Vector3(TPC.transform.position.x, player.position.y, TPC.transform.position.z);

        //카메라가 바라보는 방향은viewDir.normalized값을쓴다.
        orientation.forward = viewDir.normalized;

        // rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //플레이어 오브젝트의 정면을 입력받은 방향으로 선형 보간사용으로 부드럽게  회전시킴 
        if (inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

    }
}
