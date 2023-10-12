using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;

public class ScoreBoardRow : MonoBehaviour
{
    [Tooltip("The ID of player to display stats for")]
    public ushort ID { get; private set; }

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
    /// Only call this when instantiated.
    /// </summary>
    /// <param name="ID">The ID of a <see cref="User"/> class.</param>
    public void SetID(ushort ID)
    {
        this.ID = ID;
    }

    public void UpdateStats(PlayerStats playerStats, int ping)
    {
        UpdateStats(playerStats);
    }

    public void UpdateStats(PlayerStats playerStats)
    {
        Name.text = playerStats.Score.ToString();
        Score.text = playerStats.Score.ToString();
        Kills.text = playerStats.Kills.ToString();
        Deaths.text = playerStats.Deaths.ToString();
        Ping.text = playerStats.Ping.ToString();
    }
}
