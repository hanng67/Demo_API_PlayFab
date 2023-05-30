using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;

public class APIPlayFabPlayerInventory
{
    public static void AddInventoryItems(LoginResult loginResult)
    {

        // object Item = new InventoryItemReference
        // {
        //     Id = "weapon_level_1",
        // };

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            // AuthenticationContext = loginResult.AuthenticationContext,
            FunctionName = "AddInventoryItems",

            // FunctionParameter = new Dictionary<string, object>() { { "EntityId", loginResult}, }
        }, result =>
        {
            // ConsoleUI.Instance.LogText("ExecuteCloudScript Success");
            // ConsoleUI.Instance.LogText(result.ToJson());
            // APIPlayFabConsoleUI.Instance.LogText(result.FunctionResult.ToString());
        }, OnRequestFailure);

        // PlayFab.EconomyModels.EntityKey entity = new PlayFab.EconomyModels.EntityKey
        // {
        //     Id = id,
        //     Type = type
        // };

        // var request = new AddInventoryItemsRequest
        // {
        //     Entity = entity,
        //     Amount = 1,
        //     Item = new InventoryItemReference
        //     {
        //         Id = "weapon_level_1",
        //     }
        // };
        // PlayFabEconomyAPI.AddInventoryItems(request, AddInventoryItemsResult, OnRequestFailure);
    }

    private static void AddInventoryItemsResult(AddInventoryItemsResponse result)
    {
        // ConsoleUI.Instance.LogText("SetProfileLanguage Success");
    }

    private static void OnRequestFailure(PlayFabError error)
    {
        // ConsoleUI.Instance.LogText("Call API Failure: " + error.GenerateErrorReport());
    }
}