using System;
using UnityEngine;

public class ProfileUI : MonoBehaviour
{
    [Header("Instances")] 
    public GameObject WeaponGo;
    public GameObject BoostGo;
    
    public void TookBoost()
    {
        BoostGo.SetActive(true);
    }

    public void UseBoost()
    {
        BoostGo.SetActive(false);
    }

    public void TookWeapon()
    {
        WeaponGo.SetActive(true);
    }

    public void UseWeapon()
    {
        WeaponGo.SetActive(false);
    }
}
