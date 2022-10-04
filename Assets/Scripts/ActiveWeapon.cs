using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1
    }

    public Transform crossHairTarget;
    public Animator rigController;
    public Transform[] weaponSlots;
    public CinemachineFreeLook playerCamera;
    public AmmoUI ammoUI;

    private Weapon[] equipedWeapons = new Weapon[2];
    private int activeWeaponIndex;
    private bool isHolstered = false;

    void Start()
    {
        Weapon existWeapon = GetComponentInChildren<Weapon>();
        if (existWeapon)
        {
            EquipWeapon(existWeapon);
        }
    }

    public bool IsFiring()
    {
        Weapon currentWeapon = GetActiveWeapon();
        if (!currentWeapon)
        {
            return false;
        }
        return currentWeapon.isFiring;
    }

    public Weapon GetActiveWeapon()
    {
        return GetWeapon(activeWeaponIndex);
    }

    private Weapon GetWeapon(int index)
    {
        if (index < 0 || index >= equipedWeapons.Length)
        {
            return null;
        }
        return equipedWeapons[index];
    }

    private void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);
        if (weapon && !isHolstered)
        {
            weapon.UpdateWeapon(Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleActiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(WeaponSlot.Primary);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(WeaponSlot.Secondary);
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.raycastDestination = crossHairTarget;
        weapon.weaponRecoil.playerCamera = playerCamera;
        weapon.weaponRecoil.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex],false);
        equipedWeapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponSlot);
        ammoUI.Refresh(weapon.ammoCount);
    }

    private void ToggleActiveWeapon()
    {
        bool isHolsterd = rigController.GetBool("holster_pistol");
        if (isHolsterd)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
    }

    private void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;

        if(holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }

        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }

    IEnumerator HolsterWeapon(int index)
    {
        isHolstered = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_pistol", true);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    IEnumerator ActivateWeapon(int index)
    {
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_pistol", false);
            rigController.Play("equip_" + weapon.weaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
            ammoUI.Refresh(weapon.ammoCount);
        }
    }
}
