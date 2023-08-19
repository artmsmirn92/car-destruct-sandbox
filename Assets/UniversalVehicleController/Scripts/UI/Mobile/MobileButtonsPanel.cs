using System.Collections;
using System.Collections.Generic;
using mazing.common.Runtime.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PG
{
    public class MobileButtonsPanel :MonoBehaviour
    {

#pragma warning disable 0649

        [SerializeField] private Button buttonPause;
[SerializeField] private Button buttonSlowDownTime;

public MobileButtonsPanel SetButtonPauseAction(UnityAction _Action)
{
    buttonPause.SetOnClick(_Action);
    return this;
}

public MobileButtonsPanel SetButtonSlowdownTimeAction(UnityAction _Action)
{
    buttonSlowDownTime.SetOnClick(_Action);
    return this;
}
        
        [SerializeField] float CloseY;
        [SerializeField] float OpenY;
        [SerializeField] float OpenCloseSpeed = 1000;
        [SerializeField] Button OpenButton;
        [SerializeField] Button CloseButton;

#pragma warning restore 0649

        RectTransform RectTR;
        float TargetY;

        bool ButtonsPanelIsOpen
        {
            set
            {
                PlayerPrefs.SetInt ("ButtonsPanelIsOpen", value? 1: 0);
            }
            get
            {
                return PlayerPrefs.GetInt ("ButtonsPanelIsOpen", 0) == 1;
            }
        }

        void Start ()
        {
            OpenButton.SetActive (true);
            CloseButton.SetActive (false);

            RectTR = GetComponent<RectTransform> ();
            RectTR.SetAnchoredY (CloseY);

            OpenButton.onClick.AddListener (() => 
            { 
                TargetY = OpenY;
                OpenButton.SetActive (false);
                CloseButton.SetActive (true);
                ButtonsPanelIsOpen = true;
            });

            CloseButton.onClick.AddListener (() =>
            {
                TargetY = CloseY;
                OpenButton.SetActive (true);
                CloseButton.SetActive (false);
                ButtonsPanelIsOpen = false;
            });

            if (ButtonsPanelIsOpen)
            {
                TargetY = OpenY;
                OpenButton.SetActive (false);
                CloseButton.SetActive (true);
                RectTR.SetAnchoredY (TargetY);
            }
        }

        // Update is called once per frame
        void Update ()
        {
            if (RectTR.anchoredPosition.y != TargetY)
            {
                RectTR.SetAnchoredY (Mathf.MoveTowards(RectTR.anchoredPosition.y, TargetY, OpenCloseSpeed * Time.deltaTime));
            }
        }
    }
}

// [SerializeField] private Button buttonPause;
// [SerializeField] private Button buttonSlowDownTime;
//
// public MobileButtonsPanel SetButtonPauseAction(UnityAction _Action)
// {
//     buttonPause.SetOnClick(_Action);
//     return this;
// }
//
// public MobileButtonsPanel SetButtonSlowdownTimeAction(UnityAction _Action)
// {
//     buttonSlowDownTime.SetOnClick(_Action);
//     return this;
// }
