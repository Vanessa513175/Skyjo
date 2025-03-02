using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Statistiques
{
    public class PlayerStat
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        #endregion

        #region Field et Properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int BestScore { get; set; }
        public DateTime LastGameDate { get; set; }
        public DateTime BestScoreDate { get; set; }
        #endregion

        #region Constructor
        public PlayerStat()
        {
            GamesPlayed = 0;
            GamesWon = 0;
            BestScore = int.MaxValue;
            LastGameDate = DateTime.MinValue;
            BestScoreDate = DateTime.MinValue;
            Id = Guid.NewGuid();
            Name = String.Empty;
        }
        public PlayerStat(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            GamesPlayed = 0;
            GamesWon = 0;
            BestScore = int.MaxValue;
            LastGameDate = DateTime.MinValue;
            BestScoreDate = DateTime.MinValue;
        }

        public PlayerStat(Guid id, string name, int gamesPlayed, int gamesWon, int bestScore, DateTime lastGameDate, DateTime bestScoreDate)
        {
            Id = id;
            Name = name;
            GamesPlayed = gamesPlayed;
            GamesWon = gamesWon;
            BestScore = bestScore;
            LastGameDate = lastGameDate;
            BestScoreDate = bestScoreDate;
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        #endregion

        #region Public Methods
        public void UpdateStats(int score, bool won)
        {
            GamesPlayed++;
            LastGameDate = DateTime.Now;

            if (won)
            {
                GamesWon++;
            }

            if (score < BestScore)
            {
                BestScore = score;
                BestScoreDate = DateTime.Now;
            }
        }
        #endregion

        #endregion
    }
}
