using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : UnityEvent<string>
{

}

public class WeaponAnimationEvent : MonoBehaviour
{
    public AnimEvent WeaponAnimEvent = new AnimEvent();

    public void OnAnimationEvent(string eventName)
    {
        WeaponAnimEvent.Invoke(eventName);
    }
}
