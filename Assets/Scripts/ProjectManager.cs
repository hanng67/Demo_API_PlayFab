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

    public UnityEvent OnLoginSuccessEvent;

    public string userID = "";

    private void Awake()
    {
        Instance = this;
    }

    public void SetUserID(string userID)
    {
        this.userID = userID;
        OnLoginSuccessEvent.Invoke();
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
                                               SetUserID(result.PlayFabId);
                                               Debug.Log("Login Success");
                                           },
                                           error => { Debug.LogError(error.GenerateErrorReport()); });
    }
#endif
}
