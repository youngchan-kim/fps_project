using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //카메라의 포지션에 케릭터의 포지션을 넣으면
    //카메라는 케릭터와  같은 포지션을 유지하게됨
    public Transform cameraPosition;

    // Update is called once per frame
    public void Update()
    {
        transform.position = cameraPosition.position;
   }
}
