using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    private static ScoreBoard _instance;
    public static ScoreBoard Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField, Tooltip("The prefab to spawn as a new row in the score board")]
    private GameObject _scoreRowPrefab;
    [SerializeField, Tooltip("The parent to spawn new rows under")]
    private Transform _scoreRowParent;

    private List<PlayerStats> _playerStats = new List<PlayerStats>();
    private List<ScoreBoardRow> _rows = new List<ScoreBoardRow>();

    private CanvasGroup canvasGroup; // Used to toggle visibility without disabling the GameObject


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void ShowScoreBoard()
    {
        canvasGroup.alpha = 1;
    }

    public void HideScoreBoard()
    {
        canvasGroup.alpha = 0;
    }

    public void UpdatePlayerStats(PlayerStats playerStats)
    {
        PlayerStats stats = _playerStats.FirstOrDefault(p => p.ID == playerStats.ID);

        if (stats != null)
        {
            stats = playerStats;
            _rows.FirstOrDefault(r => r.ID == stats.ID).UpdateStats(stats);
        }
        else
        {
            _playerStats.Add(playerStats);
            var row = Instantiate(_scoreRowPrefab, _scoreRowParent).GetComponent<ScoreBoardRow>();
            row.SetID(playerStats.ID);
            _rows.Add(row);
        }
    }
}
