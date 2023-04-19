using UnityEngine;

/// <summary>
/// ProfileUI is used in every ProfilePrefab to get the references and functions to modify the visual.
/// The profile prefabs are used to display for each player is he has a boost or a weapon he can use.
/// </summary>

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

    public void ResetProfile()
    {
        UseBoost();
        UseWeapon();
    }
}
