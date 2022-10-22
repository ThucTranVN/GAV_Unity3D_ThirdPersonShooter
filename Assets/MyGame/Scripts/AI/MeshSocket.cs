using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSocketController.SocketID socketID;
    private Transform attatchPoint;

    void Start()
    {
        attatchPoint = transform.GetChild(0);
    }

    public void Attach(Transform objectTransform)
    {
        objectTransform.SetParent(attatchPoint, false);
    }
}