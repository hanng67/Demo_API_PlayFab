using UnityEngine;
using System.Collections.Generic;
using PlayFab.ClientModels;

public class ItemSO
{
    public string itemId;
    public string displayName;
    public string description;
    public string itemImageUrl;
    public Sprite itemSprite;
    public string vcKey;
    public int vcValue;
    public string itemInstanceId;
    public int? remainingUses;

    public ItemSO() : this(catalogItem: null) { }

    public ItemSO(CatalogItem catalogItem)
    {
        itemId = catalogItem.ItemId;
        displayName = catalogItem.DisplayName;
        description = catalogItem.Description;
        itemImageUrl = catalogItem.ItemImageUrl;
        foreach (var vc in catalogItem.VirtualCurrencyPrices)
        {
            vcKey = vc.Key;
            vcValue = (int)vc.Value;
        }
    }

    public ItemSO(ItemSO itemSO, ItemInstance itemInstance)
    {
        itemId = itemSO.itemId;
        displayName = itemSO.displayName;
        description = itemSO.description;
        itemImageUrl = itemSO.itemImageUrl;
        itemSprite = itemSO.itemSprite;
        vcKey = itemSO.vcKey;
        vcValue = itemSO.vcValue;
        itemInstanceId = itemInstance.ItemInstanceId;
        remainingUses = itemInstance.RemainingUses;
    }
}