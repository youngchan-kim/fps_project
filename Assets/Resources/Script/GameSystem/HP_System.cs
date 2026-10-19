using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_System : MonoBehaviour
{
    public float MaxHp;
    public float CurrentHp;
    float BasicHealvolume;
    bool HP_life;

    Coroutine coroutine = null;
    public void Initialize(float MaxHealth, float MaxHealvolume, bool life)
    {
        MaxHp = MaxHealth;
        CurrentHp = 50;
        BasicHealvolume = MaxHealvolume;
        HP_life = life;
    }

    public float GetCurHp()
    {
        return CurrentHp;
    }
    public void SetCurHp(float hp)
    {
        CurrentHp = hp;
    }
    public float GetBasicHealvolume()
    {
        return BasicHealvolume;
    }

    public void Lifecheck()
    {
        if (CurrentHp <= 0) HP_life = false;
    }
    public bool GetLife()
    {
        return HP_life;
    }

    public void Dead()
    {

    }
    public void OnHpAnimtion(InventorySlot slot)
    {
//        HP가 최대치여도 아이템을 소모할 수 있음
//        수량이 0인지 확인하지 않음
//        회복 중 다시 사용하면 이전 코루틴을 중단함
//        이전 코루틴 중단 시 현재 슬롯의 수량을 다시 증가시키는 방식이 부정확함
/*        if (null != coroutine) { StopCoroutine(coroutine); Slot.totalamount++; }
        coroutine = StartCoroutine(HpSliderAnimation(Slot.item.addHeal));
        Slot.totalamount--;*/

        if (slot == null || slot.item == null)
            return;

        if (slot.totalamount <= 0)
            return;

        if (CurrentHp >= MaxHp)
        {
            Debug.Log("HP가 최대치입니다.");
            return;
        }

        if (coroutine != null)
        {
            Debug.Log("이미 회복 중입니다.");
            return;
        }

        float healAmount = slot.item.addHeal;

        slot.totalamount--;

        coroutine = StartCoroutine(HpSliderAnimation(healAmount));
    }

    //힐 이터멀레이터
    IEnumerator HpSliderAnimation(float heal)
    {
        float remainingHeal = heal;
        float healSpeed = 30f;

        while (remainingHeal > 0f)
        {
            float healStep = Mathf.Min(
                healSpeed * Time.deltaTime,
                remainingHeal
            );

            float actualHeal = Mathf.Min(
                healStep,
                MaxHp - CurrentHp
            );

            if (actualHeal <= 0f)
                break;

            CurrentHp += actualHeal;
            remainingHeal -= actualHeal;

            yield return null;
        }

        coroutine = null;

        Debug.Log("회복 완료, 현재 HP: " + CurrentHp);
    }
}

