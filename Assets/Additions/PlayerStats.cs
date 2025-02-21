using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.PlayerLoop;

[Serializable]
public class PlayerStats : IEquatable<PlayerStats>
{
    public ushort ID { get; private set; }
    public string Name { get; private set; }
    public int Score { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public float Ping { get; set; }


    public PlayerStats(ushort ID, string name, int score = 0, int kills = 0, int deaths = 0)
    {
        //User user = Multiplayer.Instance.GetUser(ID);
        Debug.LogWarning($"name = {name}");

        this.ID = ID;
        Name = name;
        Score = score;
        Kills = kills;
        Deaths = deaths;
        //Ping = user.Latency;
    }

    //public void SetName(string name)
    //{
    //    Name = name;
    //}


    /// <summary>
    /// Copies Score, Kills, and Deaths.
    /// </summary>
    public void Update(PlayerStats stats)
    {
        Score = stats.Score;
        Kills = stats.Kills;
        Deaths = stats.Deaths;
    }


    #region operator overloads

    /// <summary>
    /// Returns true if both PlayerStats share the same ID.
    /// </summary>
    public static bool operator ==(PlayerStats lhs, PlayerStats rhs)
    {
        if (lhs is null || rhs is null)
            return false; // We are not comparing types so return false if any is null

        return lhs.Equals(rhs);
    }

    /// <summary>
    /// Returns true if both PlayerStats have different IDs.
    /// </summary>
    public static bool operator !=(PlayerStats lhs, PlayerStats rhs) => !(lhs == rhs);

    /// <summary>
    /// Returns true if the same reference or if sharing the same ID.
    /// </summary>
    public bool Equals(PlayerStats other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true; // If comparing against self, then ID is the same.

        return ID == other.ID;
    }

    public override bool Equals(object obj) => Equals(obj as PlayerStats);

    public override int GetHashCode() => base.GetHashCode();

    #endregion
}
