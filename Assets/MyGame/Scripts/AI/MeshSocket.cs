using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSocketController.SocketID socketID;
    public HumanBodyBones bones;
    public Vector3 positon;
    public Vector3 rotation;
    private Transform attatchPoint;

    void Start()
    {
        Animator animator = GetComponentInParent<Animator>();
        attatchPoint = new GameObject("Socket" + socketID).transform;
        attatchPoint.SetParent(animator.GetBoneTransform(bones));
        attatchPoint.localPosition = positon;
        attatchPoint.localRotation = Quaternion.Euler(rotation);
    }

    public void Attach(Transform objectTransform)
    {
        objectTransform.SetParent(attatchPoint, false);
    }
}