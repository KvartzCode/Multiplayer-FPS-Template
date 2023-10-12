using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlterunaFPS
{
    public partial class PlayerController
    {
        [field: Header("Stats")]

        [field: SerializeField]
        public ushort ID { get; private set; } = ushort.MaxValue;

        [field: SerializeField]
        public int Score { get; private set; }

        [field: SerializeField]
        public int Kills { get; private set; }

        [field: SerializeField]
        public int Deaths { get; private set; }

        [field: SerializeField]
        public float Ping { get; private set; }

        private PlayerStats _stats;


        public void AddScore(int score)
        {
            _stats.Score += score;
            UpdateValues();
        }

        public void AddKills(int kills = 1)
        {
            _stats.Kills += kills;
            UpdateValues();
        }

        public void AddDeaths(int death = 1)
        {
            _stats.Deaths += death;
            UpdateValues();
        }

        private void InitializeStats()
        {
            _stats = new PlayerStats(Avatar.Owner);
            UpdateValues();
        }

        private void UpdateValues()
        {
            ID = _stats.ID;
            Score = _stats.Score;
            Deaths = _stats.Deaths;
            Ping = _stats.Ping; //TODO: Update this value in intervals from somewhere. 
            SyncStats();
        }

        private void SyncStats()
        {
            ScoreBoard.Instance.UpdatePlayerStats(_stats);
        }
    }
}
