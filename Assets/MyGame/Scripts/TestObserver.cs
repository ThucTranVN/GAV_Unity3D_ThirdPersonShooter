using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestObserver : MonoBehaviour
{
    public TextMeshProUGUI countText;
    private int Count = 0;

    void Start()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_COUNT_TEXT, OnListenUpdateCountText);
        }
    }

    void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_COUNT_TEXT, OnListenUpdateCountText);
        }
    }

    private void OnListenUpdateCountText(object v)
    {
        if(v != null)
        {
            if(v is int value)
            {
                Count += value;
                countText.text = "Count: " + Count;
            }
        }
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
}
