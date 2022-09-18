using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActiveWeapon : MonoBehaviour
{
    public Transform crossHairTarget;
    public Rig handIK;
    public Transform weaponParent;

    private Weapon weapon;

    void Start()
    {
        Weapon existWeapon = GetComponentInChildren<Weapon>();
        if (existWeapon)
        {
            EquipWeapon(existWeapon);
        }
    }

    private void Update()
    {
        if (weapon)
        {
            if (Input.GetButton("Fire1"))
            {
                weapon.StartFiring();
            }

            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }

            weapon.UpdateBullet(Time.deltaTime);

            if (Input.GetButton("Fire1"))
            {
                weapon.StopFiring();
            }
        }
        else
        {
            handIK.weight = 0.0f;
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.raycastDestination = crossHairTarget;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        handIK.weight = 1.0f;
    }
}
