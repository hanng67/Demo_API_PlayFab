using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CouponSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI couponCodeText;
    [SerializeField] private Button couponButton;

    private Action onCouponClickAction;

    private float doubleClickTime = 0.3f;
    private float lastClickTime = 0f;

    private void Awake()
    {
        couponButton.onClick.AddListener(() =>
        {
            CouponClickAction();
        });

        if (transform.GetSiblingIndex() == 1)
        {
            couponButton.Select();
        }
    }

    public void UpdateVisual(string couponCode, Action onCouponClickAction)
    {
        couponCodeText.text = couponCode;
        this.onCouponClickAction = onCouponClickAction;
    }

    private void CouponClickAction()
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick <= doubleClickTime)
        {
            onCouponClickAction();
        }
        lastClickTime = Time.time;
    }
}
