using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanelUI : MonoBehaviour
{
    [SerializeField] private Button authenticationAndLoginButton;
    [SerializeField] private Button storeButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button leaderboardButton;

    [Space]
    [SerializeField] private Transform authenticationAndLoginPanelTransform;
    [SerializeField] private Transform storePanelTransform;
    [SerializeField] private Transform inventoryPanelTransform;
    [SerializeField] private Transform leaderboardPanelTransform;

    [Space]
    [Header("Setting")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color unEnableColor;

    private bool isLoggedIn = false;

    private void Awake()
    {
        authenticationAndLoginButton.onClick.AddListener(() =>
        {
            authenticationAndLoginPanelTransform.GetComponent<AuthenticationAndLoginPanelUI>().Init();
        });
        ChangeColorButton(unEnableColor);
    }

    private void Start()
    {
        ProjectManager.Instance.OnLoginSuccessEvent.AddListener(EnableAllAction);
    }

    private void EnableAllAction()
    {
        if (isLoggedIn) return;
        isLoggedIn = true;

        ChangeColorButton(defaultColor);

        storeButton.onClick.AddListener(() =>
        {
            storePanelTransform.GetComponent<StorePanelUI>().Init();
        });
        inventoryButton.onClick.AddListener(() =>
        {
            inventoryPanelTransform.GetComponent<InventoryPanelUI>().Init();
        });
        leaderboardButton.onClick.AddListener(() =>
        {
            leaderboardPanelTransform.GetComponent<LeaderboardPanelUI>().Init();
        });
    }

    private void ChangeColorButton(Color color)
    {
        storeButton.GetComponent<Image>().color = color;
        inventoryButton.GetComponent<Image>().color = color;
        leaderboardButton.GetComponent<Image>().color = color;
    }
}
