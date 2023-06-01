using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardPanelUI : BasePanelUI
{
    [SerializeField] private TextMeshProUGUI loggedInText;
    [SerializeField] private GamePlay gamePlay;

    private void Start()
    {
        gamePlay.Show();
    }

    public override void Init()
    {
        consoleUI.WriteLine("Leaderboard");
        loggedInText.text = $"Logged in User ID: {ProjectManager.Instance.userID}";
        gamePlay.Show();
        base.Init();
    }
}
