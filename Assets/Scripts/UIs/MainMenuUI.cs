using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button authenticationAndLoginButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button inventoryButton;

    [SerializeField] private Transform authenticationAndLoginTransform;
    [SerializeField] private Transform shopTransform;
    [SerializeField] private Transform inventoryTransform;

    private void Awake()
    {
        authenticationAndLoginButton.onClick.AddListener(() =>
        {
            authenticationAndLoginTransform.gameObject.SetActive(true);
        });
        shopButton.onClick.AddListener(() =>
        {
            shopTransform.gameObject.SetActive(true);
        });
        inventoryButton.onClick.AddListener(() =>
        {
            inventoryTransform.gameObject.SetActive(true);
        });
    }
}
