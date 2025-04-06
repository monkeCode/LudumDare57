using UnityEngine;

using TMPro;


public class AmmoPlayer : MonoBehaviour
{
    public TMP_Text ammoText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = $"{Player.Player.Instance.WeaponHandler.Weapon.CurrentAmmo}/{Player.Player.Instance.WeaponHandler.Weapon.MagazineSize}";
    }
}

