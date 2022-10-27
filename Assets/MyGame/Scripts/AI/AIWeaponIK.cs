using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HumanBody
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}

public class AIWeaponIK : MonoBehaviour
{
    public Transform targetTransform;
    public Transform aimTransform;
    public HumanBody[] humanBones;
    public int iterations = 10;
    [Range(0,1)]
    public float weight = 1.0f;
    public float angleLimit = 90.0f;
    public float distanceLimit = 1.5f;
    public Vector3 targetOffset;

    private Animator animator;
    private Transform[] boneTransform;


    void Start()
    {
        animator = GetComponent<Animator>();
        boneTransform = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransform.Length; i++)
        {
            boneTransform[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }

    void LateUpdate()
    {
        if (targetTransform == null || aimTransform == null) return;
        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < iterations; i++)
        {
            for (int b = 0; b < boneTransform.Length; b++)
            {
                Transform bone = boneTransform[b];
                AimAtTarget(bone, targetPosition, weight);
            }
        }
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;

        float blendOut = 0.0f;
        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if(targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if(targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }
        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }
}
