using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LoginWithDeviceIDUI : MonoBehaviour
{
    [SerializeField] private Button loginWithDeviceIDButton;
    [SerializeField] private ConsoleUI consoleUI;

    private void Awake()
    {
#if UNITY_ANDROID
        loginWithDeviceIDButton.onClick.AddListener(() =>
        {
            LoginWithDeviceID();
        });
#else
        loginWithDeviceIDButton.gameObject.GetComponent<Image>().color = Color.gray;
#endif
    }

    private void LoginWithDeviceID()
    {
        consoleUI.Write(@$"Logging in with DeviceID: {SystemInfo.deviceUniqueIdentifier}");

        var loginRequest = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(loginRequest, OnLoginSuccess, OnRequestFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        consoleUI.WriteLine("Login Success: " + result.ToJson());
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }
}
