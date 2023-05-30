using UnityEngine;
using System.Collections.Generic;
using PlayFab.ClientModels;

public class ItemSO : ScriptableObject
{
    public string itemId;
    public string displayName;
    public string description;
    public string itemImageUrl;
    public Sprite itemSprite;
    public string vcText;
    public int vcValue;

    public ItemSO() : this(null) { }

    public ItemSO(CatalogItem catalogItem)
    {
        itemId = catalogItem.ItemId;
        displayName = catalogItem.DisplayName;
        description = catalogItem.Description;
        itemImageUrl = catalogItem.ItemImageUrl;
        foreach (var vc in catalogItem.VirtualCurrencyPrices)
        {
            vcText = vc.Key;
            vcValue = (int)vc.Value;
        }
    }
}