using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Data.SkyjoData;

namespace Data.PlayersData
{
    public class GameModel : INotifyPropertyChanged
    {
        #region Enum
        #endregion

        #region Constants
        public const int DEFAULT_HEIGHT_MATRIX = 3;
        public const int DEFAULT_WIDTH_MATRIX = 4;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void SetField<T>(ref T field, T value, string propertyName)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Field et Properties
        private List<Player> _players;
        /// <summary>
        /// List of players
        /// </summary>
        public List<Player> Players
        {
            get { return _players; }
            set { SetField(ref _players, value, nameof(Players)); }
        }

        private Deck _currentDeck;
        /// <summary>
        /// The current deck
        /// </summary>
        public Deck CurrentDeck
        {
            get { return _currentDeck; }
            set { SetField(ref _currentDeck, value, nameof(CurrentDeck)); }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public GameModel()
        {
            _players = new List<Player>();
            _currentDeck = new Deck(); ;
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        /// <summary>
        /// Cleard Hand of Player
        /// </summary>
        private void ClearHandOfPlayers()
        {
            foreach (var player in Players)
            {
                player.PlayerGrid.Clear();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Draw cards to players
        /// </summary>
        public void DrawCardToPlayers()
        {
            if (Players.Count != 0)
            {
                Players[0].IsPlayerTurn = true;
                ClearHandOfPlayers();
                for (int i = 0; i < DEFAULT_HEIGHT_MATRIX; i++)
                {
                    for (int y=0; y< DEFAULT_WIDTH_MATRIX; y++)
                    {
                        foreach (var player in Players)
                        {
                            var c = CurrentDeck.DrawCard();
                            if (c!= null)
                                player.PlayerGrid.SetCard(i, y, c);
                            else
                                Logger.Instance.Log(Logger.ELevelMessage.Error, "Impossible de distribuer:  problème de pioche");
                        }
                    }
                    
                }
            }
            else
            {
                Logger.Instance.Log(Logger.ELevelMessage.Error, "Impossible de distribuer les cartes : la liste de joueurs est vide");
            }
            
        }
        #endregion

        #endregion
    }
}
