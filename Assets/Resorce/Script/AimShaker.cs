using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimShaker : MonoBehaviour
{
    float x = 0;
    float y = 0;
    public IEnumerator AimShake(float spreed)
    {

        //에임의 기본 위치에서
        Vector3 originalPos = transform.localPosition;

        //정해진 크기만큼 랜덤하게 x,y축을 변형
        x = Random.Range(-1.1f, 1.1f) * spreed;
        y = Random.Range(0f, 2f) * spreed;
       /* //에임의 로컬위치에 해당 값을 대입
        transform.localPosition = new Vector3(x, y, originalPos.z);*/
        
        yield return null;
/*        //예제에서는 복귀 하지만 복귀할 필요가 없음
        //처음 위치로 에임의 위치를 복귀
        transform.localPosition = originalPos;*/
        x = 0;
        y = 0;

    }

    public float GetAimX()
    {
        if (x == 0)
            return 0;
        else
            return x;
    }
    public float GetAimY()
    {
        if (y == 0)
            return 0;
        else
            return y;
    }
}
