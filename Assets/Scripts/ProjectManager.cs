using System.Collections;
using System.Collections.Generic;
using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

public class ProjectManager : MonoBehaviour
{
    public static ProjectManager Instance { get; private set; }

    public UnityEvent<string> OnLoginSuccessEvent;

    public string userID;

    private void Awake()
    {
        Instance = this;

        OnLoginSuccessEvent.AddListener((userID) =>
        {
            this.userID = userID;
        });
    }

#if UNITY_EDITOR
    private void Start()
    {
        LoginPlayFabWithCustomID();
    }

    private void LoginPlayFabWithCustomID()
    {
        var loginRequest = new LoginWithCustomIDRequest()
        {
            CreateAccount = false,
            CustomId = "DD2DC53706826D62",
        };
        PlayFabClientAPI.LoginWithCustomID(loginRequest,
                                           result =>
                                           {
                                               OnLoginSuccessEvent.Invoke(result.PlayFabId);
                                               Debug.Log("Login Success");
                                           },
                                           error => { Debug.LogError(error.GenerateErrorReport()); });
    }
#endif
}
