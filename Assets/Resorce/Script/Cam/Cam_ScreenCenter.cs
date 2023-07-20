using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_ScreenCenter : MonoBehaviour
{
    private Vector3 ScreenCenter;
    public RaycastHit Aimrayhit;
    Ray ray;

    private Vector3 aimpoint;
    [Header("Aim")]
    [SerializeField]
    private GameObject aim;

    private void Start()
    {
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(ScreenCenter);
        //화면상의 중심값
        if (Physics.Raycast(ray.origin, Camera.main.transform.forward, out RaycastHit Aimrayhit))
        {
            Debug.DrawLine(ray.origin, Aimrayhit.point, Color.red);
        }
        aimpoint = Aimrayhit.point;
        aim.transform.position = aimpoint;
    }
}
