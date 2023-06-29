using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class LeaderboardUI : MonoBehaviour
{
    private const string GAME_2048_HIGH_SCORE = "Game_2048_High_Score";

    [SerializeField] private Transform containerTransform;
    [SerializeField] private Transform lbSingleUITransform;
    [SerializeField] private Button updateScoreButton;
    [SerializeField] private ConsoleUI consoleUI;

    private List<LBSingleUI> lbSingleUIList = new List<LBSingleUI>();

    private void Awake()
    {
        lbSingleUITransform.gameObject.SetActive(false);

        updateScoreButton.onClick.AddListener(() =>
        {
            GamePlay.Instance.UpdateLBScore();
        });
    }

    private void Start()
    {
        GamePlay.Instance.OnUpdateScore += GamePlay_OnUpdateScore;
    }

    public void Init()
    {
        GetUserHighScore();
        GetLeaderboardGameHighScore(0, 10);
    }

    private void OnDisable()
    {
        ClearAllLBSingleUI();
    }

    private void GamePlay_OnUpdateScore(object sender, GamePlay.OnUpdateHighScoreEventArgs e)
    {
        consoleUI.WriteLine($"Updating High Score to Leaderboard: {e.highScore}");
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = GAME_2048_HIGH_SCORE,
                    Value = e.highScore
                }
            }
        }, result =>
        {
            if (!gameObject.activeInHierarchy) return;
            StartCoroutine(OnUpdatePlayerStatisticsSuccess(result));
        }, OnRequestFailure);
    }

    private IEnumerator OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        yield return new WaitForSeconds(2f);
        consoleUI.WriteLine("Updated High Score Success");
        ClearAllLBSingleUI();
        GetLeaderboardGameHighScore(0, 10);
    }

    private void GetUserHighScore()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string>
            {
                GAME_2048_HIGH_SCORE
            }
        }, OnGetUserHighScoreSuccess, OnRequestFailure);
    }

    private void OnGetUserHighScoreSuccess(GetPlayerStatisticsResult result)
    {
        consoleUI.Write("Get User High Score Success");
        foreach (StatisticValue statisticValue in result.Statistics)
        {
            if (statisticValue.StatisticName == GAME_2048_HIGH_SCORE)
            {
                GamePlay.Instance.SetHighScore(statisticValue.Value);
                consoleUI.Write($"High Score: {statisticValue.Value}");
            }
        }
        consoleUI.WriteLine("");
    }

    private void GetLeaderboardGameHighScore(int offset, int limit)
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = GAME_2048_HIGH_SCORE,
            StartPosition = offset,
            MaxResultsCount = limit
        }, OnGetLeaderboardGameHighScoreSuccess, OnRequestFailure);
    }

    private void OnGetLeaderboardGameHighScoreSuccess(GetLeaderboardResult result)
    {
        consoleUI.Write("Get Leaderboard Success: ");
        foreach (PlayerLeaderboardEntry playerLeaderboardEntry in result.Leaderboard)
        {
            bool isCurrentUser = playerLeaderboardEntry.PlayFabId == ProjectManager.Instance.userID;

            string name = playerLeaderboardEntry.DisplayName ?? playerLeaderboardEntry.PlayFabId;
            int value = playerLeaderboardEntry.StatValue;

            if (isCurrentUser)
            {
                value = GamePlay.Instance.GetHighScore();
            }

            CreateLBSingleUI(name, value, isCurrentUser);
            consoleUI.Write($"{name}: {playerLeaderboardEntry.StatValue}");
        }
        consoleUI.WriteLine("");
    }

    private void CreateLBSingleUI(string displayName, int highScore, bool isCurrentUser = false)
    {
        GameObject lbSingleUIGameObject = Instantiate(lbSingleUITransform.gameObject, containerTransform);
        lbSingleUIGameObject.SetActive(true);
        LBSingleUI lbSingleUI = lbSingleUIGameObject.GetComponent<LBSingleUI>();
        lbSingleUI.UpdateVisual(displayName, highScore, isCurrentUser);

        lbSingleUIList.Add(lbSingleUI);
    }

    private void OnRequestFailure(PlayFabError error)
    {
        consoleUI.WriteLine(error.GenerateErrorReport());
    }

    private void ClearAllLBSingleUI()
    {
        foreach (LBSingleUI lbSingleUI in lbSingleUIList)
        {
            Destroy(lbSingleUI.gameObject);
        }
        lbSingleUIList.Clear();
    }
}
