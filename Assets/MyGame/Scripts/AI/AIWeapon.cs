using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{
    private Weapon currentWeapon;
    private Animator animator;
    private MeshSocketController socketController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        socketController = GetComponent<MeshSocketController>();
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        socketController.Attach(weapon.transform, MeshSocketController.SocketID.RightLeg);
    }

    public void ActivateWeapon()
    {
        animator.SetBool("Equip", true);
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            currentWeapon = null;
        }
    }

    public void OnAnimationEvent(string eventName)
    {
        if (eventName.Equals("equipWeapon"))
        {
            socketController.Attach(currentWeapon.transform, MeshSocketController.SocketID.RightHand);
        }
    }
}
