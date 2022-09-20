using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public Transform crossHairTarget;
    public Transform weaponParent;
    public Animator rigController;

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

            if (Input.GetKeyDown(KeyCode.X))
            {
                bool isHolsterd = rigController.GetBool("holster_pistol");
                rigController.SetBool("holster_pistol", !isHolsterd); //toggle
            }
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

        rigController.Play("equip_" + weapon.weaponName);
    }
}
