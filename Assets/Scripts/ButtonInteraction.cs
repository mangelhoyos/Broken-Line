using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInteraction : MonoBehaviour
{
    private RectTransform rectTransform;

    private float initialWidth;
    private float initialPos;

    private bool mouseOverButton;

    private const byte velocity = 25;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialWidth = rectTransform.sizeDelta.x;
        initialPos = rectTransform.position.x;
    }

    private void Update()
    {
        CheckMouseActivity();
        ChangeButtonSize();
    }

    private void CheckMouseActivity()
    {
        if (EventSystem.current.IsPointerOverGameObject() && ReturnUIMouseObject() != null && ReturnUIMouseObject().gameObject.name == gameObject.name)
        {
            mouseOverButton = true;
        }else
        {
            mouseOverButton = false;
        }
    }

    private void ChangeButtonSize()
    {
        if (mouseOverButton)
        {
            rectTransform.localScale = new Vector3(Mathf.SmoothStep(rectTransform.localScale.x, 0.13f,velocity * Time.deltaTime), 
                Mathf.SmoothStep(rectTransform.localScale.y, 0.13f,velocity * Time.deltaTime),1);
        }
        else
        {
            rectTransform.localScale =
                new Vector3(Mathf.SmoothStep(rectTransform.localScale.x, 0.1f,velocity * Time.deltaTime),
                    Mathf.SmoothStep(rectTransform.localScale.y, 0.1f,velocity * Time.deltaTime),1);
        }
    }

    public GameObject ReturnUIMouseObject()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        GameObject gameObj = null;
        if (raycastResults.Count > 0)
        {
            foreach (var go in raycastResults)
            {
                if (go.gameObject.CompareTag("Button"))
                {
                    gameObj = go.gameObject;
                    break;
                }
            }
        }

        return gameObj;
    }
}