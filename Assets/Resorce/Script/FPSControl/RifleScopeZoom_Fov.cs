/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RifleScopeZoom_Fov : MonoBehaviour
{

    [SerializeField] private Camera playerCamera;

    private void Awake()
    {
        Player_RifleScopeZoom_Base playerRifleScopeZoomBase = GetComponent<Player_RifleScopeZoom_Base>();
        playerRifleScopeZoomBase.OnRifleDown += playerRifleScopeZoomBase_OnRifleDown;
        playerRifleScopeZoomBase.OnRifleUp += playerRifleScopeZoomBase_OnRifleUp;
        playerRifleScopeZoomBase.OnZoomIn += playerRifleScopeZoomBase_OnZoomIn;
        playerRifleScopeZoomBase.OnZoomOut += playerRifleScopeZoomBase_OnZoomOut;
    }
    private void Update()
    {
        float speed = 10f;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFieldOfView, Time.deltaTime * speed);
    }
    private void playerRifleScopeZoomBase_OnRifleUp(object sender, System.EventArgs e)
    {
        playerCamera.fieldOfView = 30;
    }
    private void playerRifleScopeZoomBase_OnRifleDown(object sender, System.EventArgs e)
    {
        playerCamera.fieldOfView = 50;
    }
    private void playerRifleScopeZoomBase_OnZoomOut(object sender, System.EventArgs e)
    {
        playerCamera.fieldOfView = 30;
    }
    private void playerRifleScopeZoomBase_OnZoomIn(object sender, System.EventArgs e)
    {
        playerCamera.fieldOfView = 20;
    }
}
*/