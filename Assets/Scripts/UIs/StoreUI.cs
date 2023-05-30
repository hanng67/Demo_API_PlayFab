using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : BaseUI
{
    [SerializeField] private Transform containerItemStoreTransform;
    [SerializeField] private Transform itemStoreTemplateTransform;

    private Dictionary<string, ItemSO> catalogItems = new Dictionary<string, ItemSO>();

    private void Start()
    {
        catalogItems = new Dictionary<string, ItemSO>();

        itemStoreTemplateTransform.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        consoleUI.WriteLine("Store");
        GetCatalogItems("Store_default");
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        catalogItems.Clear();
        ClearAllItemStoreUI();
    }

    private void GetCatalogItems(string storeId)
    {
        consoleUI.WriteLine("Getting Catalog Items");
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
        consoleUI.WriteLine("Getting Store Items ID");
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
        {
            StoreId = storeId
        }, OnGetStoreItemsSuccess, OnRequestFailure);
    }

    private void OnGetStoreItemsSuccess(GetStoreItemsResult result)
    {
        consoleUI.WriteLine($"Store Items: " + result.ToJson());
        foreach (StoreItem StoreItem in result.Store)
        {
            catalogItems[StoreItem.ItemId].vcValue = (int)StoreItem.VirtualCurrencyPrices[catalogItems[StoreItem.ItemId].vcText];
            AddItemStoreUI(catalogItems[StoreItem.ItemId]);
        }
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
        itemStoreUI.SetItemSO(itemSO, consoleUI);
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
