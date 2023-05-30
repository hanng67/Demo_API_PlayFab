using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class BasePanelUI : MonoBehaviour
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

    public virtual void Init()
    {
        gameObject.SetActive(true);
    }

    protected virtual void OnDisable()
    {
        consoleUI.ClearAllContent();
    }
}