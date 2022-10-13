using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerManager : SingletonMonoBehaviour<ListenerManager>
{
    public Dictionary<ListenType, ListenerGroup> listeners = new Dictionary<ListenType, ListenerGroup>();

    public void Register(ListenType type, Action<object> action)
    {
        if (!listeners.ContainsKey(type))
        {
            listeners.Add(type, new ListenerGroup());
        }
        if (listeners[type] != null)
        {
            listeners[type].Attach(action);
        }
    }

    public void Unregister(ListenType type, Action<object> action)
    {
        if(listeners.ContainsKey(type) && listeners[type] != null)
        {
            listeners[type].Detach(action);
        }
    }

    public void UnresgisterAll(Action<object> action)
    {
        foreach (ListenType key in listeners.Keys)
        {
            Unregister(key, action);
        }
    }

    public void BroadCast(ListenType type, object value = null)
    {
        if (listeners.ContainsKey(type) && listeners[type] != null)
        {
            listeners[type].Broadcast(value);
        }
    }

}
