using System;
using System.Collections.Generic;
using mazing.common.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectButtonStateFix : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect   scrollRect;
    public  List<Button> buttons = new List<Button>();

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData _EventData)
    {
        // foreach (var button in buttons)
        // {
        //     button.OnDeselect(_EventData);
        //     // button.animator.ResetTrigger("Normal");
        // }
    }

    public void OnEndDrag(PointerEventData _EventData)
    {
        // foreach (var button in buttons)
        // {
        //     button.OnPointerUp(_EventData);
        // }
    }
}