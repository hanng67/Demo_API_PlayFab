using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LoginWithCustomIDUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField customIDInputField;
    [SerializeField] private Button loginWithCustomIDButton;
    [SerializeField] private ConsoleUI consoleUI;

    private void Awake()
    {
        loginWithCustomIDButton.onClick.AddListener(() =>
        {
            LoginWithCustomID();
        });
    }

    private void LoginWithCustomID()
    {
        consoleUI.Write(@$"Logging in with CustomID: {customIDInputField.text}");

        var loginRequest = new LoginWithCustomIDRequest()
        {
            CustomId = customIDInputField.text,
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(loginRequest, OnLoginSuccess, OnRequestFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        ProjectManager.Instance.SetUserID(result.PlayFabId);
        consoleUI.Write("Login Success:");
        consoleUI.Write($"- PlayFabID: {result.PlayFabId}");
        consoleUI.WriteLine($"- SessionTicket: {result.SessionTicket}");
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }
}
