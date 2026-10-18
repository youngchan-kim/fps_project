using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text ammoText;

    private void Update()
    {
        if (player == null || ammoText == null)
            return;

        GunSystem gun = player.GetGunSystem();

        if (gun == null)
        {
            ammoText.text = "-- / --";
            return;
        }

        ammoText.text =
    gun.GetCurrentAmmo() + " / " + gun.GetReserveAmmo();
    }
}