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

    private void OnDisable()
    {
        consoleUI.ClearAllContent();
    }
}