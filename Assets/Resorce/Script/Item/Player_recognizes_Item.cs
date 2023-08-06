using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_recognizes_Item : MonoBehaviour
{
    private bool player_recognizes = false;
    private void Update()
    {
        Player_recognizes();
    }

    //실행안됨
   /* public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            player_recognizes = true;
            //물체와 충돌하게 되면 인벤토리에 들어가게 만듦
            player.Groundinventory.AddItem(new Item(GetComponent<GroundItem>().item), 1);
        }
    }*/
    //실행안됨
   /* public void OnTriggerExit(Collider other)
    {
        player_recognizes = false;
    }*/
    
    public bool Player_recognizes()
    {
        return player_recognizes;
    }


}
