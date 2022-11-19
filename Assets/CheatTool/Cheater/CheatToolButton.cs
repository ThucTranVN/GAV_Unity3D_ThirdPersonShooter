using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class CheatToolButton : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject pCheatPopup;
    private RectTransform popupTransform;

    // Dimensions of the popup divided by 2
    private Vector2 halfSize;
    
    private bool isPopupBeingDragged = false;

    // Coroutines for simple code-based animations
    private IEnumerator moveToPosCoroutine = null;

    void Awake()
    {
        popupTransform = (RectTransform) transform;
        pCheatPopup.SetActive(false);
    }

    void Start()
    {
        halfSize = popupTransform.sizeDelta * 0.5f * popupTransform.root.localScale.x;
    }

    public void OnViewportDimensionsChanged()
    {
        if (!gameObject.activeSelf)
            return;

        halfSize = popupTransform.sizeDelta * 0.5f * popupTransform.root.localScale.x;
        OnEndDrag(null);
    }

    // A simple smooth movement animation
    private IEnumerator MoveToPosAnimation(Vector3 targetPos)
    {
        float modifier = 0f;
        Vector3 initialPos = popupTransform.position;

        while (modifier < 1f)
        {
            modifier += 4f * Time.unscaledDeltaTime;
            popupTransform.position = Vector3.Lerp(initialPos, targetPos, modifier);

            yield return null;
        }
    }

    // Popup is clicked
    public void OnPointerClick(PointerEventData data)
    {
        // Hide the popup and show the log window
        if (!isPopupBeingDragged)
            pCheatPopup.SetActive(true);
    }

    // Hides the log window and shows the popup
    public void Show()
    {
        // Update position in case resolution changed while hidden
        OnViewportDimensionsChanged();
    }

    // Hide the popup
    public void Hide()
    {
        isPopupBeingDragged = false;
    }

    public void OnBeginDrag(PointerEventData data)
    {
        isPopupBeingDragged = true;

        // If a smooth movement animation is in progress, cancel it
        if (moveToPosCoroutine != null)
        {
            StopCoroutine(moveToPosCoroutine);
            moveToPosCoroutine = null;
        }
    }

    // Reposition the popup
    public void OnDrag(PointerEventData data)
    {
        popupTransform.position = data.position;
    }

    // Smoothly translate the popup to the nearest edge
    public void OnEndDrag(PointerEventData data)
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        Vector3 pos = popupTransform.position;

        // Find distances to all four edges
        float distToLeft = pos.x;
        float distToRight = Mathf.Abs(pos.x - screenWidth);

        float distToBottom = Mathf.Abs(pos.y);
        float distToTop = Mathf.Abs(pos.y - screenHeight);

        float horDistance = Mathf.Min(distToLeft, distToRight);
        float vertDistance = Mathf.Min(distToBottom, distToTop);

        // Find the nearest edge's coordinates
        if (horDistance < vertDistance)
        {
            if (distToLeft < distToRight)
                pos = new Vector3(halfSize.x, pos.y, 0f);
            else
                pos = new Vector3(screenWidth - halfSize.x, pos.y, 0f);

            pos.y = Mathf.Clamp(pos.y, halfSize.y, screenHeight - halfSize.y);
        }
        else
        {
            if (distToBottom < distToTop)
                pos = new Vector3(pos.x, halfSize.y, 0f);
            else
                pos = new Vector3(pos.x, screenHeight - halfSize.y, 0f);

            pos.x = Mathf.Clamp(pos.x, halfSize.x, screenWidth - halfSize.x);
        }

        // If another smooth movement animation is in progress, cancel it
        if (moveToPosCoroutine != null)
            StopCoroutine(moveToPosCoroutine);

        // Smoothly translate the popup to the specified position
        moveToPosCoroutine = MoveToPosAnimation(pos);
        StartCoroutine(moveToPosCoroutine);

        isPopupBeingDragged = false;
    }
}