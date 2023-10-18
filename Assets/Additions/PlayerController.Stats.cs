//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace AlterunaFPS
//{
//    public partial class PlayerController
//    {
//        [field: Header("Stats")]

//        [field: SerializeField]
//        public ushort ID { get; private set; } = ushort.MaxValue;

//        [field: SerializeField]
//        public int Score { get; private set; }

//        [field: SerializeField]
//        public int Kills { get; private set; }

//        [field: SerializeField]
//        public int Deaths { get; private set; }

//        [field: SerializeField]
//        public float Ping { get; private set; }

//        private PlayerStats _stats;


//        public void AddScore(int score)
//        {
//            _stats.Score += score;
//            Score = _stats.Score;
//            //UpdateValues();
//            RefreshValues();
//        }

//        public void AddKills(int kills = 1)
//        {
//            _stats.Kills += kills;
//            Kills = _stats.Kills;
//            //UpdateValues();
//            RefreshValues();
//        }

//        public void AddDeaths(int death = 1)
//        {
//            _stats.Deaths += death;
//            Deaths = _stats.Deaths;
//            //UpdateValues();
//            RefreshValues();
//        }

//        private void InitializeStats()
//        {
//            _stats = new PlayerStats(Avatar.Owner);
//            ID = _stats.ID;
//            //UpdateValues();
//            RefreshValues();
//        }

//        private void RefreshValues()
//        {
//            //SyncStats(Avatar.IsOwner)
//            if (Avatar.IsOwner /*&& Avatar.Possessor.IsHost*/)
//                BroadcastRemoteMethod(nameof(RefreshValuesRemote));
//        }

//        [SynchronizableMethod]
//        private void RefreshValuesRemote()
//        {
//            Score = _stats.Score;
//            Kills = _stats.Kills;
//            Deaths = _stats.Deaths;
//            Ping = _stats.Ping;

//            ScoreBoard.Instance.UpdatePlayerStats(_stats, true);
//        }

//        private void UpdateValues()
//        {
//            Score = _stats.Score;
//            Kills = _stats.Kills;
//            Deaths = _stats.Deaths;
//            Ping = _stats.Ping;
//            SyncStats();
//        }

//        private void UpdatePing()
//        {
//            Ping = _stats.Ping;
//        }

//        private void SyncStats()
//        {
//            //if (Avatar.IsOwner)
//            ScoreBoard.Instance.UpdatePlayerStats(_stats, Avatar.IsOwner);
//        }
//    }
//}
