using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvent animationEvent;
    public ActiveWeapon activeWeapon;
    public Transform leftHand;
    public AmmoUI ammoUI;
    public bool isReloading;

    private GameObject magazineHand;

    void Start()
    {
        animationEvent.WeaponAnimEvent.AddListener(OnAnimationEvent);
    }

    void Update()
    {
        Weapon weapon = activeWeapon.GetActiveWeapon();
        if (weapon)
        {
            if (Input.GetKeyDown(KeyCode.R) || weapon.ammoCount <=0)
            {
                isReloading = true;
                rigController.SetTrigger("reload_weapon");
            }
            if (weapon.isFiring)
            {
                ammoUI.Refresh(weapon.ammoCount);
            }
        }
        
    }

    void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "detach_magazine":
                DetachMagazine();
                break;
            case "drop_magazine":
                DropMagazine();
                break;
            case "refill_magazine":
                RefillMagazine();
                break;
            case "attatch_magazine":
                AttatchMagazine();
                break;
        }
    }

    private void DetachMagazine()
    {
        Weapon weapon = activeWeapon.GetActiveWeapon();
        magazineHand = Instantiate(weapon.weaponMagazine, leftHand, true);
        weapon.weaponMagazine.SetActive(false);

    }

    private void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        droppedMagazine.transform.localScale = new Vector3(1, 1, 1);
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        magazineHand.SetActive(false);
    }

    private void RefillMagazine()
    {
        magazineHand.SetActive(true);
    }

    private void AttatchMagazine()
    {
        Weapon weapon = activeWeapon.GetActiveWeapon();
        weapon.weaponMagazine.SetActive(true);
        Destroy(magazineHand);
        weapon.ammoCount = weapon.ammoSize;
        rigController.ResetTrigger("reload_weapon");
        ammoUI.Refresh(weapon.ammoCount);
        isReloading = false;
    }
}
