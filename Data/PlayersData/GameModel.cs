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

        private const int TIME_BETWEEN_TWO_PLAYERS = 3000;
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
                Tuple<PlayerGrid, Deck> tuple = computer.PlayATurn(Players[CurrentPlayerIndex].PlayerGrid, CurrentDeck);
                Players[CurrentPlayerIndex].PlayerGrid = tuple.Item1;
                CurrentDeck = tuple.Item2;
                Players[CurrentPlayerIndex].PlayerGrid.CheckIfNeedToDeleteLineOrColumn();


                Logger.Instance.Log(Logger.ELevelMessage.Warning, "Le joueur " + Players[CurrentPlayerIndex].Name + " a fini son tour");
                Thread.Sleep(TIME_BETWEEN_TWO_PLAYERS);
            }
            return this;
        }

        /// <summary>
        /// Restart game
        /// </summary>
        public void RestartGame()
        {
            _currentDeck = new Deck();
            GamePhase = EGamePhase.Initialization;
            DrawCardToPlayers();
            CurrentPlayerIndex = 0;
            Players[CurrentPlayerIndex].IsPlayerTurn = true;
            for (int i = 1; i < Players.Count; i++)
            {
                Players[i].IsPlayerTurn = false;
            }
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

        /// <summary>
        /// Get player with best score
        /// </summary>
        /// <returns></returns>
        public Player GetPlayerWithBestScore()
        {
            Player bestPlayer = Players[0];
            foreach (Player player in Players)
            {
                if (bestPlayer.CardScore > player.CardScore)
                    bestPlayer = player;
            }
            return bestPlayer;
        }

        /// <summary>
        /// Return all cards
        /// </summary>
        public void ReturnAllCards()
        {
            foreach (Player player in Players)
            {
                foreach (Card card in player.PlayerGrid.Cards)
                {
                    card.IsVisible = true;
                }
            }
        }

        /// <summary>
        /// Check if player finish
        /// </summary>
        /// <returns></returns>
        public bool CheckIfPlayerFinish()
        {
            bool finish = true;
            foreach (Player player in Players)
            {
                finish = true;
                foreach (Card card in player.PlayerGrid.Cards)
                {
                    if (card.IsInGame)
                        finish = finish && (card.IsVisible);
                }
                if (finish)
                    return true;
            }
            return false;
        }
        #endregion

        #endregion
    }
}
