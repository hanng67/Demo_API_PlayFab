
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private Transform containerItemStoreTransform;
    [SerializeField] private Transform itemStoreTemplateTransform;
    [SerializeField] private UserBalanceUI userBalanceUI;
    [SerializeField] private ConsoleUI consoleUI;

    private Dictionary<string, ItemSO> catalogItems = new Dictionary<string, ItemSO>();

    private void Start()
    {
        catalogItems = new Dictionary<string, ItemSO>();

        itemStoreTemplateTransform.gameObject.SetActive(false);
    }

    public void Init()
    {
        GetCatalogItems("Store_default");
    }

    private void OnDisable()
    {
        catalogItems.Clear();
        ClearAllItemStoreUI();
    }

    private void GetCatalogItems(string storeId)
    {
        consoleUI.Write("Getting Catalog Items");
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(),
                                        result => { OnGetCatalogItemsSuccess(result, storeId); },
                                        OnRequestFailure);
    }

    private void OnGetCatalogItemsSuccess(GetCatalogItemsResult result, string storeId)
    {
        consoleUI.WriteLine($"Catalog Items: " + result.ToJson());
        foreach (CatalogItem catalogItem in result.Catalog)
        {
            catalogItems.Add(catalogItem.ItemId, new ItemSO(catalogItem));
        }
        GetStoreItems(storeId);
    }

    private void GetStoreItems(string storeId)
    {
        consoleUI.Write("Getting Store Items ID");
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
        {
            StoreId = storeId
        }, OnGetStoreItemsSuccess, OnRequestFailure);
    }

    private void OnGetStoreItemsSuccess(GetStoreItemsResult result)
    {
        foreach (StoreItem StoreItem in result.Store)
        {
            string storeItemVCKey = catalogItems[StoreItem.ItemId].vcKey;
            int storeItemVCValue = (int)StoreItem.VirtualCurrencyPrices[storeItemVCKey];
            consoleUI.Write($"Store Item ID: {StoreItem.ItemId}, \nPrices: {storeItemVCValue} ({storeItemVCKey})");
            catalogItems[StoreItem.ItemId].vcValue = storeItemVCValue;
            AddItemStoreUI(catalogItems[StoreItem.ItemId]);
        }
        consoleUI.WriteLine("");
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }

    private void AddItemStoreUI(ItemSO itemSO)
    {
        Transform itemStoreTransform = Instantiate(itemStoreTemplateTransform, containerItemStoreTransform);
        itemStoreTransform.gameObject.SetActive(true);

        ItemStoreUI itemStoreUI = itemStoreTransform.GetComponent<ItemStoreUI>();
        itemStoreUI.SetItemStoreUIInfo(itemSO, () =>
        {
            userBalanceUI.UpdateVirtualCurrency(itemSO.vcKey, (-1) * itemSO.vcValue);
        }, consoleUI);
    }

    private void ClearAllItemStoreUI()
    {
        foreach (Transform child in containerItemStoreTransform)
        {
            if (child == itemStoreTemplateTransform) continue;
            Destroy(child.gameObject);
        }
    }
}
