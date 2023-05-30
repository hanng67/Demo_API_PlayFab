using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class SubtractCurrencyUI : MonoBehaviour
{
    private const string SELECT_CURRENCY = "Select Currency";
    [SerializeField] private TMP_Dropdown currencyDropdown;
    [SerializeField] private TMP_InputField amountInputField;
    [SerializeField] private Button subtractButton;
    [SerializeField] private UserBalanceUI userBalanceUI;
    [SerializeField] private ConsoleUI consoleUI;

    private string vcKey;
    private int vcValue;

    private void Awake()
    {
        subtractButton.onClick.AddListener(() =>
        {
            SubtractUserVirtualCurrency();
        });
    }

    public void Init()
    {
        List<string> currencyList = new List<string>();
        currencyList.Add(SELECT_CURRENCY);
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result =>
        {
            foreach (var vc in result.VirtualCurrency)
            {
                currencyList.Add(vc.Key);
            }
            currencyDropdown.ClearOptions();
            currencyDropdown.AddOptions(currencyList);
            currencyDropdown.RefreshShownValue();
        }, OnRequestFailure);
    }

    private void OnDisable()
    {
        currencyDropdown.ClearOptions();
        currencyDropdown.AddOptions(new List<string>() { SELECT_CURRENCY });
        currencyDropdown.RefreshShownValue();
    }

    private void SubtractUserVirtualCurrency()
    {
        vcKey = currencyDropdown.options[currencyDropdown.value].text;
        if (vcKey == SELECT_CURRENCY)
        {
            consoleUI.WriteLine("Please select currency");
            return;
        }
        vcValue = int.Parse(amountInputField.text);

        consoleUI.Write("Sending Data : { \n" +
                        $"VirtualCurrency: {vcKey},\nAmount: {vcValue}" + "\n}");

        PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest()
        {
            Amount = vcValue,
            VirtualCurrency = vcKey
        }, OnSubtractUserVirtualCurrencySuccess, OnRequestFailure);
    }

    private void OnSubtractUserVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        consoleUI.Write("Subtract User Currency Successfully:");
        consoleUI.WriteLine($"BalanceChange: {result.BalanceChange} \n" +
                            $"Balance: {result.Balance}");
        userBalanceUI.UpdateVirtualCurrency(vcKey, (-1) * vcValue);
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }
}
