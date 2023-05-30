using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] protected ConsoleUI consoleUI;

    protected virtual void Awake()
    {
        backButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        consoleUI.ClearAllContent();
    }
}