using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour
{



    float cur_hori;
    float cur_vert;
    // Start is called before the first frame update

    public Animator anim;
    float moveAction =0;

    public void AnimStart()
    {
        Debug.Log(transform.GetChild(0).name);
        anim = transform.GetChild(0).GetComponent<Animator>();
        //anim = transform.GetComponent<Animator>();
    }
    public void MoveAnim(float vertical, float horizon)
    {
       /* Debug.Log("앞 뒤 정지:" + vertical);
        Debug.Log("우 좌 정지:" + horizon);
        Debug.Log("앞뒤" + anim.GetFloat("B&F"));
        Debug.Log("좌우"+ anim.GetFloat("Dir"));*/
        if(vertical!= 0 || horizon!=0)
        {
            Debug.Log("방향이 입력됨");
        }
        switch (vertical)
        {
            case 1:
                anim.SetFloat("B&F", 1);
                break;
            case 0:
                anim.SetFloat("B&F", 0);
                break;
            case -1:
                anim.SetFloat("B&F", -1);
                break;
        }
        switch (horizon)
        {
            case 1:
                anim.SetFloat("Dir", 1);
                break;
            case 0:
                anim.SetFloat("Dir", 0);
                break;
            case -1:
                anim.SetFloat("Dir", -1);
                break;
        }
    }

    public void AimStart()
    {
        anim.SetBool("Aim", true);
    }
    public void AimEnd()
    {
        anim.SetBool("Aim", false);
    }
    public void Aiming()
    {
      /* if(anim.GetBool("Aim"))
       {
            //anim.SetFloat("Aiming")
       }*/

    }
}
