using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class PlayerStats
{
    public ushort ID { get; private set; }
    public int Score { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public float Ping { get => _user.Latency; }

    private User _user;


    public PlayerStats(User user, int score = 0, int kills = 0, int deaths = 0)
    {
        _user = user;

        ID = user.Index;
        Score = score;
        Kills = kills;
        Deaths = deaths;
    }
}
