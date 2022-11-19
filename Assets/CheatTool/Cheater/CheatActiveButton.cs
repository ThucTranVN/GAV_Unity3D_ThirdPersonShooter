using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheatActiveButton : MonoBehaviour {

    public bool isHoldDown = false;
    private const float TIME_TO_ACTIVE = 5f;

    private float cacheTime = 0;

    void Awake()
    {
        Reset();
    }

    void Update()
    {
        if(isHoldDown) {
            cacheTime += Time.deltaTime;
            if(cacheTime >= TIME_TO_ACTIVE)
            {
                ActiveCheatButton();
            }
        }
    }


    public void OnPointerDown(BaseEventData data)
    {
        if (isHoldDown)
            return;
        isHoldDown = true;
    }

    public void OnPointerUp(BaseEventData data)
    {
        if (!isHoldDown)
            return;
        Reset();
    }

    private void Reset()
    {
        isHoldDown = false;
        cacheTime = 0;
    }

    private void ActiveCheatButton()
    {
        CheatTool.Instance.ShowCheatButton();
        Reset();
    }


}
