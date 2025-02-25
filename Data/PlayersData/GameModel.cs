using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Data.AutoGame;
using Data.SkyjoData;

namespace Data.PlayersData
{
    public class GameModel : INotifyPropertyChanged
    {
        #region Enum
        public enum EGamePhase
        {
            Initialization,
            Playing,          
            End               
        }

        #endregion

        #region Constants
        public const int DEFAULT_HEIGHT_MATRIX = 3;
        public const int DEFAULT_WIDTH_MATRIX = 4;

        private const int TIME_BETWEEN_TWO_PLAYERS = 5000;
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

        public EGamePhase GamePhase;

        public int CurrentPlayerIndex;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public GameModel()
        {
            _players = new List<Player>();
            _currentDeck = new Deck();
            GamePhase = EGamePhase.Initialization;
            CurrentPlayerIndex = 0;
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
        /// Play auto turn (computer player)
        /// </summary>
        public GameModel PlayAutoTurn()
        {
            ComputerPlayer computer = new ComputerPlayer();
            if (GamePhase == EGamePhase.Initialization)
            {
                Players[CurrentPlayerIndex].PlayerGrid = computer.ReturnTwoCards(Players[CurrentPlayerIndex].PlayerGrid);

                Logger.Instance.Log(Logger.ELevelMessage.Info, "Le joueur " + Players[CurrentPlayerIndex].Name + " a retourné deux cartes");
                Thread.Sleep(TIME_BETWEEN_TWO_PLAYERS);
            }
            else if (GamePhase == EGamePhase.Playing)
            {
                Tuple<PlayerGrid, Card> tuple = computer.PlayATurn(Players[CurrentPlayerIndex].PlayerGrid, CurrentDeck.DrawCard());
                Players[CurrentPlayerIndex].PlayerGrid = tuple.Item1;
                CurrentDeck.LastPlayedCard = tuple.Item2;
                
                Logger.Instance.Log(Logger.ELevelMessage.Info, "Le joueur " + Players[CurrentPlayerIndex].Name + " a fini son tour");
                Thread.Sleep(TIME_BETWEEN_TWO_PLAYERS);
            }
            return this;
        }

        /// <summary>
        /// Draw cards to players
        /// </summary>
        public void DrawCardToPlayers()
        {
            if (Players.Count != 0)
            {
                Players[CurrentPlayerIndex].IsPlayerTurn = true;
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

        /// <summary>
        /// Next player
        /// </summary>
        public void Next()
        {
            Players[CurrentPlayerIndex].IsPlayerTurn = false;
            // Change Player Index
            if (CurrentPlayerIndex == Players.Count - 1)
            {
                CurrentPlayerIndex = 0;
                if (GamePhase == EGamePhase.Initialization)
                    GamePhase = EGamePhase.Playing;
            }
            else
            {
                CurrentPlayerIndex++;
            }
            Players[CurrentPlayerIndex].IsPlayerTurn = true;
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Au tour de "+ Players[CurrentPlayerIndex].Name);
        }
        #endregion

        #endregion
    }
}
