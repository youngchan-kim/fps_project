using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //카메라의 포지션에 케릭터의 포지션을 넣으면
    //카메라는 케릭터와  같은 포지션을 유지하게됨
    public Transform cameraPosition;
    public Transform Orientation;
    Transform Head;
    Vector3 pos;
    private void Start()
    {
        Head = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0);
    }
    // Update is called once per frame
    public void Update()
    {
        //Debug.Log(Head.name);
        /*pos = Head.position + (Vector3.up * 0.1f) + (Vector3.forward * 0.1f);
        cameraPosition.position = pos;*/
        cameraPosition.position = Head.position + (Vector3.up * 0.1f) + (Vector3.forward * 0.1f);
       
    }
}
