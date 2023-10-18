using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;

public class ScoreBoardRow : MonoBehaviour
{
    //[Tooltip("The ID of player to display stats for")]
    //public ushort ID { get; private set; }

    //[field: SerializeReference]
    private PlayerStats _stats;
    public PlayerStats Stats { get => _stats; }

    [SerializeField, Tooltip("Reference to the name text-object")]
    private TMP_Text Name;
    [SerializeField, Tooltip("Reference to the score text-object")]
    private TMP_Text Score;
    [SerializeField, Tooltip("Reference to the kills text-object")]
    private TMP_Text Kills;
    [SerializeField, Tooltip("Reference to the deaths text-object")]
    private TMP_Text Deaths;
    [SerializeField, Tooltip("Reference to the ping text-object")]
    private TMP_Text Ping;


    /// <summary>
    /// Sets ID and Name of row. Should only be called once after being instantiated.
    /// </summary>
    public void Initialize(PlayerStats stats)
    {
        _stats = stats;
        Name.text = _stats.Name;
        //_stats.SetName(stats.Name);
    }

    ///// <summary>
    ///// Only call this when instantiated.
    ///// </summary>
    ///// <param name="ID">The ID of a <see cref="User"/> class.</param>
    //public void SetID(ushort ID)
    //{
    //    this.ID = ID;
    //}

    //public void UpdateStats(PlayerStats playerStats, int ping)
    //{
    //    UpdateStats(playerStats);
    //}

    //private void RefreshStats()
    //{
    //    Name.text = Stats.Name.ToString();
    //    Score.text = Stats.Score.ToString();
    //    Kills.text = Stats.Kills.ToString();
    //    Deaths.text = Stats.Deaths.ToString();
    //    Ping.text = Stats.Ping.ToString();
    //}

    public void UpdateStats(PlayerStats playerStats)
    {
        _stats.Update(playerStats);
        //Name.text = playerStats.Name.ToString();
        Score.text = _stats.Score.ToString();
        Kills.text = _stats.Kills.ToString();
        Deaths.text = _stats.Deaths.ToString();
        //Ping.text = playerStats.Ping.ToString();
    }

    public void UpdateScore(int amount)
    {
        _stats.Score += amount;
        Score.text = _stats.Score.ToString();
    }

    public void UpdateKills(int amount)
    {
        _stats.Kills += amount;
        Score.text = _stats.Kills.ToString();
    }

    public void UpdateDeaths(int amount)
    {
        _stats.Deaths += amount;
        Score.text = _stats.Deaths.ToString();
    }

    public void UpdatePing(float ping)
    {
        //Debug.Log("PING = " + ping);
        Stats.Ping = ping;
        Ping.text = /*Mathf.Floor*/(ping).ToString();
    }

    //public void RefreshPing(float ping)
    //{
    //    Stats.Ping = ping;
    //    Ping.text = Mathf.FloorToInt(ping).ToString();
    //}
}
