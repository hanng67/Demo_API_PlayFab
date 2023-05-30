using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsoleUI : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;
    [SerializeField] private Transform TextTemplateTransform;
    [SerializeField] private Button clearButton;
    [SerializeField] private ScrollRect scrollRect;

    private void Awake()
    {
        clearButton.onClick.AddListener(() =>
        {
            ClearAllContent();
        });
    }

    private void Start()
    {
        TextTemplateTransform.gameObject.SetActive(false);
    }

    public void Write(string text)
    {
        Transform contentTextTransform = Instantiate(TextTemplateTransform, contentTransform);
        TextMeshProUGUI textMeshProUGUI = contentTextTransform.GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = text;
        contentTextTransform.gameObject.SetActive(true);
        ScrollContentToBottom();
        Debug.Log(text);
    }

    public void WriteLine(string text)
    {
        text = text + "\n------------------------------------------------------";
        Write(text);
    }

    public void ClearAllContent()
    {
        foreach (Transform child in contentTransform)
        {
            if (child == TextTemplateTransform) continue;

            Destroy(child.gameObject);
        }
    }

    private void ScrollContentToBottom()
    {
        Canvas.ForceUpdateCanvases();

        scrollRect.verticalNormalizedPosition = 0;
    }
}
