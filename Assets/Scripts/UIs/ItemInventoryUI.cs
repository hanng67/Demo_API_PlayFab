using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using PlayFab;
using PlayFab.ClientModels;

public class ItemInventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI textButton;

    private ItemSO itemSO;
    private Action onButtonSuccessAction;
    private ConsoleUI consoleUI;

    private void Awake()
    {
        button.onClick.AddListener(() => ButtonAction());
    }

    public void SetItemInventoryUIInfo(ItemSO itemSO, Action onButtonSuccessAction, ConsoleUI consoleUI)
    {
        this.itemSO = itemSO;
        this.onButtonSuccessAction = onButtonSuccessAction;
        this.consoleUI = consoleUI;

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        string displayName = itemSO.displayName;
        if (itemSO.remainingUses != null)
        {
            displayName += $"(x{itemSO.remainingUses})";
            textButton.text = "Consume";
        }
        else
        {
            textButton.text = "Discard";
        }
        displayNameText.text = displayName;
        iconImage.enabled = false;
        StartCoroutine(LoadImageFromURL(itemSO.itemImageUrl));
    }

    public IEnumerator LoadImageFromURL(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));
            iconImage.sprite = sprite;
            iconImage.enabled = true;
        }
    }

    private void ButtonAction()
    {
        if (itemSO.remainingUses == null)
        {
            DiscardItem();
        }
        else
        {
            ConsumeItem();
        }
    }

    private void DiscardItem()
    {
        consoleUI.Write($"Discarding Item {itemSO.displayName}");
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "RevokeInventoryItem",
            FunctionParameter = new { itemInstanceId = itemSO.itemInstanceId },
        }, OnRevolkItemSuccess, OnRequestFailure);
    }

    private void OnRevolkItemSuccess(ExecuteCloudScriptResult result)
    {
        if (result.FunctionResult != null)
        {
            consoleUI.WriteLine($"Revoked Item {itemSO.displayName} Failed");
            onButtonSuccessAction();
        }
        else
        {
            consoleUI.WriteLine("Error: " + result.ToJson());
        }
    }

    private void ConsumeItem()
    {
        consoleUI.Write($"Consuming Item {itemSO.displayName}");
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest()
        {
            ConsumeCount = 1,
            ItemInstanceId = itemSO.itemInstanceId,
        }, OnConsumeItemSuccess, OnRequestFailure);
    }

    private void OnConsumeItemSuccess(ConsumeItemResult result)
    {
        consoleUI.WriteLine($"Consumed Item {itemSO.displayName} Successfully");
        onButtonSuccessAction();
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }
}
