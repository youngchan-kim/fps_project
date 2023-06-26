using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimShaker : MonoBehaviour
{
    private PlayerCam camShake;
    public IEnumerator AimShake(float duration, float magnitude)
    {

/*        //에임의 기본 위치에서
        Vector3 originalPos = transform.localPosition;*/

        //호출 될 때 마다 초기화 되어야함
        float elapsed = 0.0f;

        //시간 값이 들어오면
        //시간 값보다 경과 시간이 작으면 계속 반복
        while (elapsed < duration)
        {
            //정해진 크기만큼 랜덤하게 x,y축을 변형
            camShake.SetAimXMove(Random.Range(-0.1f, 0.1f) * magnitude);
            camShake.SetAimYMove(Random.Range(0f, 2f) * magnitude);
/*            //카메라의 로컬위치에 해당 값을 대입
            transform.localPosition = new Vector3(x, y, originalPos.z);*/

            //경과시간 은 실제지나간 시간을 더한 값으로 경과시간을 체크
            elapsed += Time.deltaTime;

            yield return null;
        }
/*        //예제에서는 복귀 하지만 복귀할 필요가 없음
        //처음 위치로 에임의 위치를 복귀
        transform.localPosition = originalPos;*/

    }
}
