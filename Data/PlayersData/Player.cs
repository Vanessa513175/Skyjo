using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.SkyjoData;

namespace Data.PlayersData
{
    public class Player
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        #endregion

        #region Field et Properties
        private string _name;
        /// <summary>
        /// Name of the player
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Guid _playerId;
        /// <summary>
        /// Player Id
        /// </summary>
        public Guid PlayerId
        {
            get { return _playerId; }
            set { _playerId = value; }
        }

        private bool _isPlayerTurn;
        /// <summary>
        /// Check if is player turn
        /// </summary>
        public bool IsPlayerTurn
        {
            get { return _isPlayerTurn; }
            set { _isPlayerTurn = value; }
        }

        public PlayerGrid PlayerGrid;

        public int CardScore
        {
            get
            {
                return PlayerGrid.GetCurrentScore();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name"></param>
        public Player(string name, int height, int width)
        {
            _playerId = Guid.NewGuid();
            _name = name;
            _isPlayerTurn = false;
            PlayerGrid = new PlayerGrid(height, width);
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        #endregion

        #region Public Methods
        #endregion

        #endregion
    }
}
