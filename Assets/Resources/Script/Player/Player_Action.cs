using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour
{

    public float horizontalInput;
    public float verticalInput;

    float cur_hori;
    float cur_vert;
    // Start is called before the first frame update

    public Animator anim;
    float moveAction =0;

    public void AnimStart()
    {
        Debug.Log(transform.GetChild(0).name);
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    public void Anim()
    {
        switch(horizontalInput)
        {
            case -1:

                break;

            case 0:
                break;

            case 1:
                break;
        }
        if (horizontalInput ==-1)
            anim.SetFloat("B&F", -1);
        else if (horizontalInput > cur_hori)
            anim.SetFloat("B&F", 1);
        else
            anim.SetFloat("B&F", 0);


        if (verticalInput < cur_vert)
            anim.SetFloat("Dir", -1);
        else if (verticalInput > cur_vert)
            anim.SetFloat("Dir", 1);
        else
            anim.SetFloat("Dir", 0);


        if (cur_hori != horizontalInput)
            cur_hori = horizontalInput;
        if (cur_vert != verticalInput)
            cur_vert = verticalInput;
    }
}
