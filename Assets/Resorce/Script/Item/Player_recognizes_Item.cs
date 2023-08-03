using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_recognizes_Item : MonoBehaviour
{
    public Transform player;
    //오브젝트를 얻을 수 있는 범위
    public float pickUpRange;
    private void Update()
    {
        Player_recognizes();
    }
    protected Vector3 Player_recognizes()
    //플레이어가 아이템을 인식할 수 있는 범위
    { 
        Vector3 distanceToPlayer = player.position - transform.position;
        return distanceToPlayer;
    }
    protected bool Player_Acquisition_range()
    {
        //습득가능 범위안이면
        if (Player_recognizes().magnitude <= pickUpRange)
            return true;
        return false;
    }
}
