
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ProfilesModels;
using UnityEngine;

public class APIPlayfabContent
{
    public static void GetTitleNews()
    {
        SetProfileLanguage(GameDefine.LANGUAGE_ENGLISH);
        PlayFabClientAPI.GetTitleNews(new GetTitleNewsRequest(), GetTitleDataResult, OnRequestFailure);
    }

    public static void GetTitleDataResult(GetTitleNewsResult result)
    {
        // ConsoleUI.Instance.LogText("GetTitleNews Success");
        foreach (TitleNewsItem newsItem in result.News)
        {
            string jsonNews = newsItem.ToJson();
            Debug.Log(jsonNews);
            // ConsoleUI.Instance.LogText($"{newsItem.Title}: " + newsItem.Body);
        }
    }

    public static void PushNotification(string title, string message, int? timestamp = null)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "PushNotification",
            FunctionParameter = new Dictionary<string, object>() {{ "Title", title },
                                                                { "Message", message },
                                                                { "Timestamp", timestamp } }
        }, result =>
        {
            // ConsoleUI.Instance.LogText("PushNotification Success");
        }, OnRequestFailure);
    }

    private static void SetProfileLanguage(string language)
    {
        var request = new SetProfileLanguageRequest
        {
            Language = language
        };
        PlayFabProfilesAPI.SetProfileLanguage(request, SetProfileLanguageResult, OnRequestFailure);
    }

    private static void SetProfileLanguageResult(SetProfileLanguageResponse result)
    {
        // ConsoleUI.Instance.LogText("SetProfileLanguage Success");
    }

    private static void OnRequestFailure(PlayFabError error)
    {
        // ConsoleUI.Instance.LogText("Call API Failure: " + error.GenerateErrorReport());
    }
}