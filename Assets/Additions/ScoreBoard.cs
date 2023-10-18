using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alteruna;
using UnityEngine;
using UnityEngine.UI;

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

    private List<ScoreBoardRow> _rows = new List<ScoreBoardRow>();

    private CanvasGroup canvasGroup; // Used to toggle visibility without disabling the GameObject.


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    private void Start()
    {
        Multiplayer.OtherUserJoined.AddListener(OtherUserJoined);
        InvokeRepeating(nameof(UpdatePing), pingRefreshRate, pingRefreshRate);
    }

    /// <summary>
    /// Returns if you're not the host.
    /// </summary>
    private void OtherUserJoined(Multiplayer multiplayer, User user)
    {
        if (!Multiplayer.GetUser().IsHost)
            return;

        var row = GetOrAddRow(user);
        //This could be better but basically don't call remote method for host and the new user since they initialize the new user's row themselfes.
        List<ushort> users = Multiplayer.GetUsers().Where(u => u.Index != user.Index && u.Index != Multiplayer.GetUser().Index).Select(u => u.Index).ToList();
        InvokeRemoteMethod(nameof(AddRowForClients), users, row.Stats);

        var players = _rows.Select(r => r.Stats).ToList();
        InvokeRemoteMethod(nameof(GetPlayerList), user.Index, players);
    }

    /// <summary>
    /// Replaces any existing list of rows with new ones sent from the host.
    /// </summary>
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

    /// <summary>
    /// Called for all clients except the client who just joined.
    /// </summary>
    /// <param name="player"></param>
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


    #region Update Stats

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

    public void AddDeaths(User user, int amount)
    {
        if (Multiplayer.GetUser().IsHost)
        {
            GetOrAddRow(user).Stats.Deaths += amount;
            RefreshValues(user);
        }
    }

    /// <summary>
    /// Send updated values of user to all clients. However, if you want to be more effective,
    /// you should only send the values that clients need. Instead of the whole object of data.
    /// </summary>
    private void RefreshValues(User user)
    {
        if (Multiplayer.GetUser().IsHost)
        {
            PlayerStats player = _rows.FirstOrDefault(r => r.Stats.ID == user.Index).Stats;
            BroadcastRemoteMethod(nameof(RefreshValuesRemote), player);
        }
    }

    /// <summary>
    /// Updates all values of row matching the PlayerStats parameter.
    /// </summary>
    [SynchronizableMethod]
    private void RefreshValuesRemote(PlayerStats player)
    {
        _rows.FirstOrDefault(r => r.Stats == player).UpdateStats(player);

        //Would be nice if this worked.
        _rows.OrderBy(r => r.Stats.Score);
        for (int i = 0; i < _rows.Count; i++)
        {
            _rows[i].transform.SetSiblingIndex(i);
        }
    }

    #endregion

    /// <summary>
    /// Doesn't fully work since the only latency available to us is the host's latency.
    /// </summary>
    private void UpdatePing()
    {
        List<User> users = Multiplayer.GetUsers();
        foreach (var user in users)
        {
            var row = _rows.FirstOrDefault(r => r.Stats.ID == user.Index);
            row.UpdatePing(user.Latency);
        }
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
