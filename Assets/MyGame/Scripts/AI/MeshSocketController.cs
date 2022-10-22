using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocketController : MonoBehaviour
{
    public enum SocketID
    {
        RightLeg,
        RightHand
    }

    private Dictionary<SocketID, MeshSocket> socketMap = new Dictionary<SocketID, MeshSocket>();

    void Start()
    {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
        foreach (var socket in sockets)
        {
            socketMap[socket.socketID] = socket;
        }
    }

    public void Attach(Transform objectTransform, SocketID socketID)
    {
        socketMap[socketID].Attach(objectTransform);
    }
}
