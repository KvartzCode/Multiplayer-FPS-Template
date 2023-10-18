using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alteruna;
using UnityEngine;

public class ScoreBoard : AttributesSync
{
    private static ScoreBoard _instance;
    public static ScoreBoard Instance
    {
        get
        {
            return _instance;
        }
    }

    [Range(1, float.MaxValue)]
    public float pingRefreshRate = 5;

    [SerializeField, Tooltip("The prefab to spawn as a new row in the score board")]
    private GameObject _scoreRowPrefab;
    [SerializeField, Tooltip("The parent to spawn new rows under")]
    private Transform _scoreRowParent;

    //private List<PlayerStats> _playerStats = new List<PlayerStats>();
    private List<ScoreBoardRow> _rows = new List<ScoreBoardRow>();

    private CanvasGroup canvasGroup; // Used to toggle visibility without disabling the GameObject


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        //InvokeRepeating(nameof(RefreshPing), pingRefreshRate, pingRefreshRate);
    }

    private void Start()
    {
        //if (Multiplayer.Me.IsHost)
        //Multiplayer.ForceSynced.AddListener(OtherUserJoined);
        Multiplayer.OtherUserJoined.AddListener(OtherUserJoined);
        InvokeRepeating(nameof(UpdatePing), pingRefreshRate, pingRefreshRate);
    }


    private void OtherUserJoined(Multiplayer multiplayer, User user)
    {
        if (!Multiplayer.GetUser().IsHost)
            return;

        var row = GetOrAddRow(user);
        List<ushort> users = Multiplayer.GetUsers().Where(u => u.Index != user.Index && u.Index != Multiplayer.GetUser().Index).Select(u => u.Index).ToList();
        InvokeRemoteMethod(nameof(AddRowForClients), users, row.Stats);

        //RefreshValues(user);
        var players = _rows.Select(r => r.Stats).ToList();
        InvokeRemoteMethod(nameof(GetPlayerList), user.Index, players);
    }

    [SynchronizableMethod]
    public void GetPlayerList(List<PlayerStats> playerList)
    {
        if (_rows.Count > 0)
        {
            for (int i = _rows.Count - 1; i >= 0; i--)
            {
                Destroy(_rows[i].gameObject);
            }
            _rows.Clear();
        }

        foreach (var player in playerList)
        {
            GetOrAddRow(player);
        }
    }

    [SynchronizableMethod]
    private void AddRowForClients(PlayerStats player)
    {
        GetOrAddRow(player);
    }

    public ScoreBoardRow GetOrAddRow(User user)
    {
        var player = new PlayerStats(user.Index, user.Name);
        return GetOrAddRow(player);
    }

    public ScoreBoardRow GetOrAddRow(PlayerStats player)
    {
        ScoreBoardRow row = _rows.FirstOrDefault(r => r.Stats.ID == player.ID);
        if (row)
            return row;

        row = Instantiate(_scoreRowPrefab, _scoreRowParent).GetComponent<ScoreBoardRow>();
        row.Initialize(player);
        _rows.Add(row);
        return row;
    }

    public void AddScore(User user, int amount)
    {
        if (Multiplayer.GetUser().IsHost)
        {
            GetOrAddRow(user).Stats.Score += amount;
            RefreshValues(user);
        }
    }

    public void AddKills(User user, int amount)
    {
        if (Multiplayer.GetUser().IsHost)
        {
            GetOrAddRow(user).Stats.Kills += amount;
            RefreshValues(user);
        }
    }

    private void RefreshValues(User user)
    {
        if (Multiplayer.GetUser().IsHost)
        {
            PlayerStats player = _rows.FirstOrDefault(r => r.Stats.ID == user.Index).Stats;
            BroadcastRemoteMethod(nameof(RefreshValuesRemote), player);
        }
    }

    [SynchronizableMethod]
    private void RefreshValuesRemote(PlayerStats player)
    {
        _rows.FirstOrDefault(r => r.Stats == player).UpdateStats(player);
    }

    //[SynchronizableMethod]
    //private void RefreshValuesRemote()
    //{
    //    foreach (ScoreBoardRow row in _rows)
    //    {
    //        row.RefreshStats();
    //    }
    //}

    //private void RefreshPing()
    //{
    //    BroadcastRemoteMethod(nameof(RefreshPingRemote));
    //}

    //[SynchronizableMethod]
    //private void RefreshPingRemote()
    //{
    //    foreach (ScoreBoardRow row in _rows)
    //    {
    //        row.RefreshPing();
    //    }
    //}

    private void UpdatePing()
    {
        List<User> users = Multiplayer.GetUsers();
        foreach (var user in users)
        {
            var row = _rows.FirstOrDefault(r => r.Stats.ID == user.Index);
            row.UpdatePing(user.Latency);
        }
        //foreach (ScoreBoardRow row in _rows)
        //{
        //    User user = Multiplayer.GetUser(row.Stats.ID);
        //    row.UpdatePing(user.Latency);
        //}
    }

    public void ShowScoreBoard()
    {
        canvasGroup.alpha = 1;
    }

    public void HideScoreBoard()
    {
        canvasGroup.alpha = 0;
    }
}
