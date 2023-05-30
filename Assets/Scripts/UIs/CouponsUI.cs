using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class CouponsUI : MonoBehaviour
{
    private const string COUPON_CODES_KEY = "CouponCodes";

    [SerializeField] private Transform containerTransform;
    [SerializeField] private Transform couponTemplateTransform;
    [SerializeField] private TMP_InputField couponInputField;
    [SerializeField] private Button redeemButton;
    [SerializeField] private UserBalanceUI userBalanceUI;
    [SerializeField] private ConsoleUI consoleUI;

    private string couponCodes = "";

    private void Awake()
    {
        redeemButton.onClick.AddListener(() =>
        {
            RedeemCoupon();
        });
    }

    private void Start()
    {
        couponTemplateTransform.gameObject.SetActive(false);
    }

    public void Init()
    {
        GetCouponCodes();
    }

    private void OnDisable()
    {
        couponInputField.text = "";
        ClearAllCouponCodesText();
    }

    private void GetCouponCodes()
    {
        consoleUI.Write("Getting Coupon Codes");
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest()
        {
            Keys = new List<string>() { COUPON_CODES_KEY }
        }, OnGetCouponCodesSuccess, OnRequestFailure);
    }

    private void OnGetCouponCodesSuccess(GetTitleDataResult result)
    {
        consoleUI.WriteLine($"Coupon Codes: " + result.Data[COUPON_CODES_KEY]);
        couponCodes = result.Data[COUPON_CODES_KEY];
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        ClearAllCouponCodesText();
        foreach (var couponCode in couponCodes.Split(','))
        {
            Transform textTransform = Instantiate(couponTemplateTransform, containerTransform);
            textTransform.gameObject.SetActive(true);
            textTransform.GetComponent<CouponSingleUI>().UpdateVisual(couponCode, () =>
            {
                couponInputField.text = couponCode;
            });
        }
    }

    private void RedeemCoupon()
    {
        string couponCode = couponInputField.text;
        if (couponCode == "")
        {
            consoleUI.Write("Please enter a coupon code!");
            return;
        }

        consoleUI.Write($"Redeeming Coupon Code: {couponInputField.text}");
        PlayFabClientAPI.RedeemCoupon(new RedeemCouponRequest()
        {
            CouponCode = couponInputField.text
        }, OnRedeemCouponSuccess, OnRequestFailure);
    }

    private void OnRedeemCouponSuccess(RedeemCouponResult result)
    {
        consoleUI.Write("Redeem Success: ");
        foreach (ItemInstance item in result.GrantedItems)
        {
            consoleUI.Write($"Granted Item: {item.DisplayName}");
            if (item.ItemId == "Redeem_Coupons_VC")
            {
                userBalanceUI.Init(isEnableLog: false);
            }
        }
        consoleUI.WriteLine("");
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }

    private void ClearAllCouponCodesText()
    {
        foreach (Transform child in containerTransform)
        {
            if (child == couponTemplateTransform) continue;
            Destroy(child.gameObject);
        }
    }
}
