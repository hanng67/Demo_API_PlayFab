using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanelUI : MonoBehaviour
{
    [SerializeField] private Button authenticationAndLoginButton;
    [SerializeField] private Button storeButton;
    [SerializeField] private Button inventoryButton;

    [SerializeField] private Transform authenticationAndLoginPanelTransform;
    [SerializeField] private Transform storePanelTransform;
    [SerializeField] private Transform inventoryPanelTransform;

    private void Awake()
    {
        authenticationAndLoginButton.onClick.AddListener(() =>
        {
            authenticationAndLoginPanelTransform.GetComponent<AuthenticationAndLoginPanelUI>().Init();
        });
        storeButton.onClick.AddListener(() =>
        {
            storePanelTransform.GetComponent<StorePanelUI>().Init();
        });
        inventoryButton.onClick.AddListener(() =>
        {
            inventoryPanelTransform.GetComponent<InventoryPanelUI>().Init();
        });
    }
}
