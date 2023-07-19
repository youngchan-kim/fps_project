using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_ScreenCenter : MonoBehaviour
{
    private Vector3 ScreenCenter;
    private RaycastHit Aimrayhit;
    private float Distance = 1000f;
    private void Start()
    {

        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }
    private void Update()
    {
        //화면상의 중심값
        if(Physics.Raycast(ScreenCenter, Camera.main.transform.forward, out Aimrayhit, Distance))
        Debug.DrawRay(Aimrayhit.point, Camera.main.transform.forward * 100f, Color.red);

    }
    public RaycastHit GetAimPoint()
    {
        
        return Aimrayhit;
    }
}
