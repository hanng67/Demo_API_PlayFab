using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LoginWithUsernameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button loginWithUsernameButton;
    [SerializeField] private ConsoleUI consoleUI;

    private void Awake()
    {
        loginWithUsernameButton.onClick.AddListener(() =>
        {
            LoginWithUsername();
        });
    }

    private void LoginWithUsername()
    {
        consoleUI.Write(@$"Logging in with Username: {usernameInputField.text}");

        var loginRequest = new LoginWithPlayFabRequest()
        {
            Username = usernameInputField.text,
            Password = passwordInputField.text,
        };
        PlayFabClientAPI.LoginWithPlayFab(loginRequest, OnLoginSuccess, OnRequestFailure);
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
