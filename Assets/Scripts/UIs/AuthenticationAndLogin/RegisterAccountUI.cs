using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class RegisterAccountUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button registerAccountButton;
    [SerializeField] private ConsoleUI consoleUI;

    private void Awake()
    {
        registerAccountButton.onClick.AddListener(() =>
        {
            RegisterAccount();
        });
    }

    private void RegisterAccount()
    {
        consoleUI.Write(@$"Registering account: {usernameInputField.text}, with Email: {emailInputField.text}");

        var registerRequest = new RegisterPlayFabUserRequest()
        {
            Email = emailInputField.text,
            Password = passwordInputField.text,
            Username = usernameInputField.text,
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRequestFailure);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        consoleUI.Write("Register Success:");
        consoleUI.Write($"- PlayFabID: {result.PlayFabId}");
        consoleUI.Write($"- Username: {result.Username}");
        consoleUI.WriteLine($"- SessionTicket: {result.SessionTicket}");
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }
}
