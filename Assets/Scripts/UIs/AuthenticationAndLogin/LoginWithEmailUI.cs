using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LoginWithEmailUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button loginWithEmailButton;
    [SerializeField] private ConsoleUI consoleUI;

    private void Awake()
    {
        loginWithEmailButton.onClick.AddListener(() =>
        {
            LoginWithEmail();
        });
    }

    private void LoginWithEmail()
    {
        consoleUI.Write(@$"Logging in with Email: {emailInputField.text}");

        var loginRequest = new LoginWithEmailAddressRequest()
        {
            Email = emailInputField.text,
            Password = passwordInputField.text,
        };
        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, OnLoginSuccess, OnRequestFailure);
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
