using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : Health
{
    public PostProcessVolume postProcessing;
    [Range(0,1)]
    public float maxValue;
    private Ragdoll ragdoll;
    private ActiveWeapon activeWeapon;
    private CharacterAiming aiming;

    protected override void OnStart()
    {
        ragdoll = GetComponent<Ragdoll>();
        activeWeapon = GetComponent<ActiveWeapon>();
        aiming = GetComponent<CharacterAiming>();
    }

    protected override void OnDeath(Vector3 direction)
    {
        ragdoll.ActiveRagdoll();
        direction.y = 0;
        ragdoll.ApplyForce(direction);
        activeWeapon.DropWeapon();
        aiming.enabled = false;
        CameraManager.Instance.EnableKillCam();
    }

    protected override void OnDamage(Vector3 direction)
    {
        if (postProcessing.profile.TryGetSettings(out Vignette vignette))
        {
            float percent = 1.0f - (currentHealth / maxHealth);
            vignette.intensity.value = percent * maxValue;
        }
    }
}
