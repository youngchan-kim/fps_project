using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 이벤트트리거를 컴포넌트로 가진 오브젝트가 스트롤뷰 하위로 들어가면
/// 해당 오브젝트 위에서는 스트롤기능이 작동하지 않는다.
/// 해당 기능을 사용하기 위해 만든 스트립트
/// </summary>
public class InventoryScroll : MonoBehaviour, IScrollHandler
{
    private ScrollRect parentSR;

    private void Awake()
    {
        // 부모 계층에서 ScrollRect 자동 검색
        parentSR = GetComponentInParent<ScrollRect>();

        if (parentSR == null)
        {
            Debug.LogWarning(
                $"{gameObject.name}: 부모에서 ScrollRect를 찾지 못했습니다."
            );
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (parentSR == null)
            return;

        parentSR.OnScroll(eventData);
    }
}
