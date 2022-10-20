using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealthBar : MonoBehaviour
{
    public Canvas canvas;
    public Image foregroundImg;
    public Image backgroundImge;
    private Transform target;

    void Start()
    {
        canvas.worldCamera = Camera.main;
        target = Camera.main.transform;
    }


    void LateUpdate()
    {
        if (target)
        {
            this.transform.rotation = target.transform.rotation;
        }
    }

    public void SetHealthBarPercentage(float percentage)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        foregroundImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    public void Deactive()
    {
        this.gameObject.SetActive(false);
    }
}
