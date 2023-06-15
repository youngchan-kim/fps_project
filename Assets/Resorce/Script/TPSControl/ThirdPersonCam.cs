using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 바라보는 방향을 카메라가 바라보게 하기위함
//플레이어의 앞면을 가리키는 플레이어.foward 변수를 통해서 케릭터가 바라보는 방향을 알 수 있다. 
public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;

    public CameraStyle curentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //카메라 스타일 바꾸기
        if (Input.GetKeyDown(KeyCode.V) && curentStyle == CameraStyle.Combat) SwitchCameraStyle(CameraStyle.Basic);
        else if (Input.GetKeyDown(KeyCode.V) && curentStyle == CameraStyle.Basic) SwitchCameraStyle(CameraStyle.Combat);
        //카메라에서 플레이어까지의 방향을 계산하여 전방이 어디인지 알아내기
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);

        //카메라가 바라보는 방향은viewDir.normalized값을쓴다.
        orientation.forward = viewDir.normalized;

        // rotate player object

        if (curentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        else if (curentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) { thirdPersonCam.SetActive(true); combatCam.SetActive(false); }
        else if (newStyle == CameraStyle.Combat) { combatCam.SetActive(true); thirdPersonCam.SetActive(false); }
        curentStyle = newStyle;
    }
}
