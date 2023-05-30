using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class StorePanelUI : BasePanelUI
{
    [SerializeField] private StoreUI storeUI;
    [SerializeField] private UserBalanceUI userBalanceUI;
    [SerializeField] private AddCurrencyUI addCurrencyUI;
    [SerializeField] private SubtractCurrencyUI subtractCurrencyUI;

    public override void Init()
    {
        consoleUI.WriteLine("Store");
        storeUI.Init();
        userBalanceUI.Init();
        addCurrencyUI.Init();
        subtractCurrencyUI.Init();
        base.Init();
    }
}
