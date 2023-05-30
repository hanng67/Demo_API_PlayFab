using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using PlayFab;
using PlayFab.ClientModels;

public class ItemStoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Button purchaseButton;

    private ItemSO itemSO;
    private ConsoleUI consoleUI;

    private void Awake()
    {
        purchaseButton.onClick.AddListener(() => PurchaseItem());
    }

    public void SetItemSO(ItemSO itemSO, ConsoleUI consoleUI)
    {
        this.itemSO = itemSO;
        this.consoleUI = consoleUI;

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        displayNameText.text = itemSO.displayName;
        iconImage.enabled = false;
        StartCoroutine(LoadImageFromURL(itemSO.itemImageUrl));
        valueText.text = $"{itemSO.vcValue} ({itemSO.vcText})";
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

    private void PurchaseItem()
    {
        consoleUI.Write($"Purchasing Item {itemSO.displayName}");
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
        {
            ItemId = itemSO.itemId,
            VirtualCurrency = itemSO.vcText,
            Price = itemSO.vcValue
        }, OnPurchaseItemSuccess, OnRequestFailure);
    }

    private void OnPurchaseItemSuccess(PurchaseItemResult result)
    {
        consoleUI.WriteLine($"Purchased Item {result.Items[0].DisplayName} Successfully");
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }
}
