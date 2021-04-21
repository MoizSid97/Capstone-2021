﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMeshSocket : MonoBehaviour
{
    public enum SocketID
    {
        Spine,
        RightHand
    }

    Dictionary<SocketID, MeshSocket> socketMap = new Dictionary<SocketID, MeshSocket>();

    // Start is called before the first frame update
    void Start()
    {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
        foreach(var socket in sockets)
        {
            socketMap[socket.socketID] = socket;
        }
    }

    public void Attach(Transform objTransform, SocketID id)
    {
        socketMap[id].Attach(objTransform);
    }
}