using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class ProjectManager : MonoBehaviour
{

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
                                           result => { Debug.Log("Login Success"); },
                                           error => { Debug.LogError(error.GenerateErrorReport()); });
    }
#endif
}
