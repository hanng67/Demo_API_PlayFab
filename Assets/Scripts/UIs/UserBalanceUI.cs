using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;

public class UserBalanceUI : MonoBehaviour
{
    [SerializeField] private Transform containerTransform;
    [SerializeField] private Transform textTemplateTransform;
    [SerializeField] private ConsoleUI consoleUI;

    private Dictionary<string, int> virtualCurrency = new Dictionary<string, int>();

    private bool isEnableLog = true;

    private void Start()
    {
        textTemplateTransform.gameObject.SetActive(false);
    }

    public void Init(bool isEnableLog = true)
    {
        this.isEnableLog = isEnableLog;
        GetUserInventory();
    }

    private void OnDisable()
    {
        ClearAllVirtualCurrencyText();
    }

    private void GetUserInventory()
    {
        if (isEnableLog)
        {
            consoleUI.Write("Getting User Inventory");
        }
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnRequestFailure);
    }

    private void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        if (isEnableLog)
        {
            consoleUI.WriteLine($"User Balance: " + result.ToJson());
        }
        virtualCurrency = result.VirtualCurrency;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        ClearAllVirtualCurrencyText();
        foreach (var vc in virtualCurrency)
        {
            Transform textTransform = Instantiate(textTemplateTransform, containerTransform);
            textTransform.gameObject.SetActive(true);
            textTransform.GetComponent<TextMeshProUGUI>().text = $"{vc.Key} = {vc.Value}";
        }
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }

    private void ClearAllVirtualCurrencyText()
    {
        foreach (Transform child in containerTransform)
        {
            if (child == textTemplateTransform) continue;
            Destroy(child.gameObject);
        }
    }

    public void UpdateVirtualCurrency(string key, int value, bool isReplace = false)
    {
        virtualCurrency[key] += value;
        if (isReplace)
        {
            virtualCurrency[key] = value;
        }
        UpdateVisual();
    }
}
