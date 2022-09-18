using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab;

    void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if (activeWeapon)
        {
            Weapon newWeapon = Instantiate(weaponPrefab);
            activeWeapon.EquipWeapon(newWeapon);
        }
    }
}
