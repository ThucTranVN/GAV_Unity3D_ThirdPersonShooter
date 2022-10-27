using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadCastEvent : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ListenerManager.HasInstance)
            {
                int sendValue = 1;
                ListenerManager.Instance.BroadCast(ListenType.UPDATE_COUNT_TEXT, sendValue);
            }  
        }
    }
}
