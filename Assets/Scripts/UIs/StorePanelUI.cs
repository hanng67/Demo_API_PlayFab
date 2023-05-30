using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class StorePanelUI : BasePanelUI
{
    [SerializeField] private StoreUI storeUI;

    public override void Init()
    {
        consoleUI.WriteLine("Store");
        storeUI.Init();
        base.Init();
    }
}
