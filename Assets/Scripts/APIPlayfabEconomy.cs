
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class APIPlayfabEconomy
{
    public static void GetCatalogItems(string catalogVersion)
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
        {
            CatalogVersion = catalogVersion
        }, OnGetCatalogItemsSuccess, OnRequestFailure);
    }

    public static void OnGetCatalogItemsSuccess(GetCatalogItemsResult result)
    {
        foreach (CatalogItem catalogItem in result.Catalog)
        {
            string jsonCatalog = catalogItem.ToJson();
            Debug.Log(jsonCatalog);
            // ConsoleUI.Instance.LogText($"Store Item {catalogItem.ItemId}: " + jsonCatalog);
        }
    }

    public static void GetStoreItems(string catalogVersion, string storeId)
    {
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
        {
            CatalogVersion = catalogVersion,
            StoreId = storeId
        }, OnGetCatalogItemsSuccess, OnRequestFailure);
    }

    public static void OnGetCatalogItemsSuccess(GetStoreItemsResult result)
    {
        foreach (StoreItem StoreItem in result.Store)
        {
            string jsonCatalog = StoreItem.ToJson();
            Debug.Log(jsonCatalog);
            // ConsoleUI.Instance.LogText($"Catalog Item {StoreItem.ItemId}: " + jsonCatalog);
        }
    }

    private static void OnRequestFailure(PlayFabError error)
    {
        // ConsoleUI.Instance.LogText("Call API Failure: " + error.GenerateErrorReport());
    }
}