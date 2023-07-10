using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMouse : MonoBehaviour
{
    public GameObject point;
    private Vector3 pointposition;
    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        pointposition = point.transform.localPosition;
        pointposition.z  =  3f;
        this.transform.localPosition = pointposition;
    }
}
