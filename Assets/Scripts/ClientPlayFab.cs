using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class ClientPlayFab : MonoBehaviour
{
    private void Start()
    {
        LoginPlayFabWithCustomID();
    }

    private void LoginPlayFabWithCustomID()
    {
        var loginRequest = new LoginWithCustomIDRequest()
        {
            CreateAccount = false,
            CustomId = "J5F4PRZMKZCGR7CI",
        };
        PlayFabClientAPI.LoginWithCustomID(loginRequest, OnLoginSuccess, OnRequestFailure);
    }

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }

    private void OnLoginSuccess(LoginResult result)
    {
        // ConsoleUI.Instance.LogText("Login Success: " + result.ToJson());
        // APIPlayfabEconomy.GetStoreItems(GameDefine.CATALOG_ID, GameDefine.STORE_ID_WEAPON);
        APIPlayFabPlayerInventory.AddInventoryItems(result);
    }

    private void OnRequestFailure(PlayFabError error)
    {
        // ConsoleUI.Instance.LogText("Call API Failure: " + error.GenerateErrorReport());
    }
}
