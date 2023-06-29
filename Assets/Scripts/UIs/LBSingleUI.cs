using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LBSingleUI : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Awake()
    {
        backgroundImage.enabled = false;
    }

    public void UpdateVisual(string userName, int highScore, bool isCurrentUser = false)
    {
        userNameText.text = userName;
        highScoreText.text = highScore.ToString();

        if (isCurrentUser)
        {
            backgroundImage.enabled = true;
        }
    }
}
