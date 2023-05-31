using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class InventoryPanelUI : BasePanelUI
{
    [SerializeField] private TextMeshProUGUI loggedInText;
    [SerializeField] private Transform containerItemInventoryTransform;
    [SerializeField] private Transform itemInventoryTemplateTransform;
    [SerializeField] private Transform containerTextBalanceTransform;
    [SerializeField] private Transform textBalanceTemplateTransform;

    private Dictionary<string, ItemSO> catalogItems = new Dictionary<string, ItemSO>();
    private List<ItemSO> inventoryItems = new List<ItemSO>();
    private Dictionary<string, int> virtualCurrency = new Dictionary<string, int>();

    private void Start()
    {
        itemInventoryTemplateTransform.gameObject.SetActive(false);
        textBalanceTemplateTransform.gameObject.SetActive(false);
    }

    public override void Init()
    {
        consoleUI.WriteLine("Inventory");
        loggedInText.text = $"Logged in User ID: {ProjectManager.Instance.userID}";
        GetCatalogItems();
        base.Init();
    }

    protected override void OnDisable()
    {
        catalogItems.Clear();
        inventoryItems.Clear();
        ClearAllItemInventoryUI();
        ClearAllTextBalance();
        base.OnDisable();
    }

    private void GetCatalogItems()
    {
        consoleUI.Write("Getting Catalog Items");
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogItemsSuccess, OnRequestFailure);
    }

    private void OnGetCatalogItemsSuccess(GetCatalogItemsResult result)
    {
        consoleUI.WriteLine($"Catalog Items: " + result.ToJson());
        foreach (CatalogItem catalogItem in result.Catalog)
        {
            catalogItems.Add(catalogItem.ItemId, new ItemSO(catalogItem));
        }
        GetUserInventory();
    }

    private void GetUserInventory()
    {
        // if (isEnableLog)
        {
            consoleUI.Write("Getting User Inventory");
        }
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnRequestFailure);
    }

    private void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        // if (isEnableLog)
        {
            consoleUI.WriteLine($"User Inventory: " + result.ToJson());
        }

        foreach (ItemInstance item in result.Inventory)
        {
            inventoryItems.Add(new ItemSO(catalogItems[item.ItemId], item));
        }
        virtualCurrency = result.VirtualCurrency;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        UpdateVisualItemInventory();
        UpdateVisualTextBalance();
    }

    private void UpdateVisualItemInventory()
    {
        ClearAllItemInventoryUI();
        foreach (var item in inventoryItems)
        {
            Transform itemTransform = Instantiate(itemInventoryTemplateTransform, containerItemInventoryTransform);
            itemTransform.gameObject.SetActive(true);
            itemTransform.GetComponent<ItemInventoryUI>()
                         .SetItemInventoryUIInfo(item, consoleUI);
        }
    }

    private void UpdateVisualTextBalance()
    {
        ClearAllTextBalance();
        foreach (var vc in virtualCurrency)
        {
            Transform textTransform = Instantiate(textBalanceTemplateTransform, containerTextBalanceTransform);
            textTransform.gameObject.SetActive(true);
            textTransform.GetComponent<TextMeshProUGUI>().text = $"{vc.Key} = {vc.Value}";
        }
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }

    private void ClearAllItemInventoryUI()
    {
        foreach (Transform child in containerItemInventoryTransform)
        {
            if (child == itemInventoryTemplateTransform) continue;
            Destroy(child.gameObject);
        }
    }

    private void ClearAllTextBalance()
    {
        foreach (Transform child in containerTextBalanceTransform)
        {
            if (child == textBalanceTemplateTransform) continue;
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
        UpdateVisualTextBalance();
    }
}
