using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Core;
using Core.RelayCommand;
using Data.PlayersData;
using Skyjo.WindowManager;

namespace Skyjo.ViewModel
{
    class ViewModelPlayerWindow : ViewModelBase
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        public ICommand ReturnCard { get; }
        public ICommand ExchangeCards { get; }
        public ICommand ThrowAwayDrawnCard { get; }
        public ICommand TurnOverFirstTwoCards { get; }
        #endregion

        #region Field et Properties

        #region Control
        public string VisibilityCommandGlobal;
        public string VisibilityCommandInitialization;
        public string VisibilityCommandPlaying;
        public bool IsInitialisationPhase => GameModel.GamePhase == GameModel.EGamePhase.Initialization;

        public bool IsPlayingPhase => GameModel.GamePhase == GameModel.EGamePhase.Playing;

        public bool DrawnCardThrown => _drawnCard == null && OneCardIsSelected;

        public bool DiscardedCard;

        public bool IsDrawnCard => _drawnCard != null && DiscardedCard;

        public bool OneCardIsSelected => CheckIfOneCardIsSelected();

        private ViewModelCard? _drawnCard;
        public ViewModelCard? DrawnCard
        {
            get { return _drawnCard; }
            set
            {
                if (_drawnCard != value)
                {
                    _drawnCard = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ViewModelCard? _selectedCard;
        #endregion

        #region CurrentPlayer
        private readonly Guid _playerId;
        private Player? _player;

        /// <summary>
        /// Check if is player turn
        /// </summary>
        public bool IsPlayerTurn
        {
            get
            {
                if (_player != null)
                    return _player.IsPlayerTurn;
                else
                    return false;
            }
        }

        private ViewModelPlayerGrid _currentPlayerGrid;
        public ViewModelPlayerGrid CurrentPlayerGrid
        {
            get
            {
                return _currentPlayerGrid;
            }
            set
            {
                _currentPlayerGrid = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The player name
        /// </summary>
        public String CurrentPlayerName
        {
            get
            {
                if (_player != null)
                    return _player.Name;
                else
                    return "Aucun joueur trouvé";
            }
        }


        /// <summary>
        /// The player card score
        /// </summary>
        public int CurrentPlayerCardScore
        {
            get
            {
                if (_player != null)
                    return _player.PlayerGrid.GetCurrentScore();
                else
                    return 0;
            }
        }
        #endregion

        #region Player2
        private Player? _player2;

        /// <summary>
        /// The player grid
        /// </summary>
        public ViewModelPlayerGrid? Player2Grid
        {
            get
            {
                if (_player2!=null)
                    return new ViewModelPlayerGrid(_player2.PlayerGrid);
                return null;
            }
        }

        /// <summary>
        /// The player name
        /// </summary>
        public String Player2Name
        {
            get
            {
                if (_player2 != null)
                    return _player2.Name;
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// The player card score
        /// </summary>
        public int? Player2CardScore
        {
            get
            {
                if (_player2 != null)
                    return _player2.PlayerGrid.GetCurrentScore();
                else
                    return null;
            }
        }
        #endregion

        #region Player3
        private Player? _player3;

        /// <summary>
        /// The player grid
        /// </summary>
        public ViewModelPlayerGrid? Player3Grid
        {
            get
            {
                if (_player3 != null)
                    return new ViewModelPlayerGrid(_player3.PlayerGrid);
                return null;
            }
        }

        /// <summary>
        /// The player name
        /// </summary>
        public String Player3Name
        {
            get
            {
                if (_player3 != null)
                    return _player3.Name;
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// The player card score
        /// </summary>
        public int? Player3CardScore
        {
            get
            {
                if (_player3 != null)
                    return _player3.PlayerGrid.GetCurrentScore();
                else
                    return null;
            }
        }
        #endregion

        #region Player4
        private Player? _player4;

        /// <summary>
        /// The player grid
        /// </summary>
        public ViewModelPlayerGrid? Player4Grid
        {
            get
            {
                if (_player4 != null)
                    return new ViewModelPlayerGrid(_player4.PlayerGrid);
                return null;
            }
        }

        /// <summary>
        /// The player name
        /// </summary>
        public String Player4Name
        {
            get
            {
                if (_player4 != null)
                    return _player4.Name;
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// The player card score
        /// </summary>
        public int? Player4CardScore
        {
            get
            {
                if (_player4 != null)
                    return _player4.PlayerGrid.GetCurrentScore();
                else
                    return null;
            }
        }
        #endregion

        #region Global
        private readonly NavigationService _navigationService;

        /// <summary>
        /// The local Game Model
        /// </summary>
        public GameModel GameModel
        {
            get;
            private set;
        }

        private StringBuilder _logBuilder = new();
        private string _logText;
        /// <summary>
        /// Log Text Management
        /// </summary>
        public string LogText
        {
            get => _logText;
            set
            {
                _logText = value;
                RaisePropertyChanged();
            }
        }

        private string _currentPhase;
        public String CurrentPhase
        {
            get { return _currentPhase; }
            set
            {
                _currentPhase = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="navService"></param>
        /// <param name="playerId"></param>
        public ViewModelPlayerWindow(NavigationService navService, Guid playerId)
        {
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Lancement de la partie");
            _navigationService = navService;
            _playerId = playerId;

            _logText = String.Empty;

            // Commands
            ReturnCard = new RelayCommand(Command_ReturnCard);
            ExchangeCards = new RelayCommand(Command_ExchangeCards);
            ThrowAwayDrawnCard = new RelayCommand(Command_ThrowAwayDrawnCard);
            TurnOverFirstTwoCards = new RelayCommand(Command_TurnOverFirstTwoCards);

            _navigationService.GameModelUpdated += UpdateViewModel;
            
            Logger.Instance.LogUpdated += OnLogUpdated;

            // Local Game Model
            GameModel = navService.ShareGameModel;

            UpdateValues();
            
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Au tour de : " + CurrentPlayerName);
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        /// <summary>
        /// Update game model (event)
        /// </summary>
        /// <param name="newGameModel"></param>
        private void UpdateViewModel(GameModel newGameModel)
        {
            GameModel = newGameModel;

            if (GameModel.GamePhase == GameModel.EGamePhase.Playing && GameModel.Players[GameModel.CurrentPlayerIndex].PlayerId == _playerId)
            {
                DrawnCard = new ViewModelCard(GameModel.CurrentDeck.DrawCard(), -1, -1);
                DrawnCard.ChangeVisibility(true);
            }
                
            UpdateValues();
        }

        /// <summary>
        /// Update values from GameModel
        /// </summary>
        private void UpdateValues()
        {
            DiscardedCard = false;
            // List of players
            List<Player> otherPlayers = new List<Player>();
            foreach (Player p in GameModel.Players)
            {
                if (p.PlayerId == _playerId)
                    _player = p;
                else
                    otherPlayers.Add(p);
            }
            GetOthersPlayers(otherPlayers);

            // Player grid
            if (_player != null)
                _currentPlayerGrid = new ViewModelPlayerGrid(_player.PlayerGrid);
            else
                _currentPlayerGrid = new ViewModelPlayerGrid(new PlayerGrid(0, 0));

            _currentPhase = GameModel.GamePhase.ToString();

            DisplayUpdate();
        }
        /// <summary>
        /// Check if one card is selected
        /// </summary>
        /// <returns></returns>
        private bool CheckIfOneCardIsSelected()
        {
            _selectedCard = null;
            for (int i=0; i< CurrentPlayerGrid.RowCount; i++)
            {
                for (int y=0; y<CurrentPlayerGrid.ColumnCount; y++)
                {
                    if (CurrentPlayerGrid.GetCard(i, y) != null)
                    {
                        if (CurrentPlayerGrid.GetCard(i, y).IsSelected)
                        {
                            if (_selectedCard != null)
                            {
                                _selectedCard = null;
                                return false;
                            }
                            else
                            {
                                _selectedCard = CurrentPlayerGrid.GetCard(i, y);
                            }
                            
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Get the others players
        /// </summary>
        private void GetOthersPlayers(List<Player> otherPlayers)
        {
            for (int i =0; i < otherPlayers.Count; i++)
            {
                switch (i) {
                    case 0:
                        _player2 = otherPlayers[i];
                        break;
                    case 1:
                        _player3 = otherPlayers[i];
                        break;
                    case 2:
                        _player4 = otherPlayers[i];
                        break;
                }
            }
        }

        /// <summary>
        /// Unselect all cards
        /// </summary>
        private void UnselectAllCard()
        {
            for (int i = 0; i < CurrentPlayerGrid.RowCount; i++)
            {
                for (int y = 0; y < CurrentPlayerGrid.ColumnCount; y++)
                {
                    if (CurrentPlayerGrid.GetCard(i, y) != null)
                    {
                        CurrentPlayerGrid.GetCard(i, y).IsSelected = false;
                    }
                }
            }
        }
        /// <summary>
        /// Display update
        /// </summary>
        private void DisplayUpdate()
        {
            if (IsPlayerTurn)
                VisibilityCommandGlobal = "Visible";
            else
                VisibilityCommandGlobal = "Hidden";

            if (IsInitialisationPhase)
                VisibilityCommandInitialization = "Visible";
            else
                VisibilityCommandInitialization = "Hidden";

            if (IsPlayingPhase)
                VisibilityCommandPlaying = "Visible";
            else
                VisibilityCommandPlaying = "Hidden";



            RaisePropertyChanged(nameof(CurrentPlayerName));
            RaisePropertyChanged(nameof(CurrentPlayerCardScore));
            RaisePropertyChanged(nameof(CurrentPlayerGrid));
            RaisePropertyChanged(nameof(IsPlayerTurn));

            RaisePropertyChanged(nameof(Player2Name));
            RaisePropertyChanged(nameof(Player2CardScore));
            RaisePropertyChanged(nameof(Player2Grid));

            RaisePropertyChanged(nameof(Player3Name));
            RaisePropertyChanged(nameof(Player3CardScore));
            RaisePropertyChanged(nameof(Player3Grid));

            RaisePropertyChanged(nameof(Player4Name));
            RaisePropertyChanged(nameof(Player4CardScore));
            RaisePropertyChanged(nameof(Player4Grid));

            RaisePropertyChanged(nameof(CurrentPhase));
            RaisePropertyChanged(nameof(IsInitialisationPhase));
            RaisePropertyChanged(nameof(IsPlayingPhase));
            RaisePropertyChanged(nameof(DrawnCard));
            RaisePropertyChanged(nameof(OneCardIsSelected));
            RaisePropertyChanged(nameof(DrawnCardThrown));
            RaisePropertyChanged(nameof(VisibilityCommandGlobal));
            RaisePropertyChanged(nameof(VisibilityCommandInitialization));
            RaisePropertyChanged(nameof(VisibilityCommandPlaying));
        }
        private void OnLogUpdated(string newLog)
        {
            _logBuilder.AppendLine(newLog);
            LogText = _logBuilder.ToString();
        }

        private void Command_ReturnCard()
        {
            if (DrawnCard == null)
            {
                CheckIfOneCardIsSelected();
                if (_selectedCard != null)
                {
                    _currentPlayerGrid.ChangeVisibility(true, _selectedCard.Position.Item1, _selectedCard.Position.Item2);
                    Logger.Instance.Log(Logger.ELevelMessage.Info, "Le joueur "+CurrentPlayerName+" a retourné une de ses cartes");
                    SendToNextPlayer();
                }
                else
                {
                    Logger.Instance.Log(Logger.ELevelMessage.Warning, "Il en faut choisir qu'une seule carte à retourner dans votre jeu");
                    return;
                }
            }
            else
            {
                Logger.Instance.Log(Logger.ELevelMessage.Warning, "Vous devez jeter la carte pioché si vous ne souhaitez pas la garder");
                return;
            }
        }

        private void Command_ExchangeCards()
        {
            if (CheckIfOneCardIsSelected() )
            {
                ViewModelCard oldCard = _currentPlayerGrid.GetCard(_selectedCard.Position.Item1, _selectedCard.Position.Item2);
                _currentPlayerGrid.SetCard(DrawnCard, _selectedCard.Position.Item1, _selectedCard.Position.Item2);
                _selectedCard = null;
                DrawnCard = null;
                GameModel.CurrentDeck.LastPlayedCard = oldCard.CardObject;
                Logger.Instance.Log(Logger.ELevelMessage.Info, "Le joueur a pris la carte pioché");
            }
            else
            {
                Logger.Instance.Log(Logger.ELevelMessage.Warning, "Il en faut choisir qu'une seule carte à échanger dans votre jeu");
                return;
            }
            SendToNextPlayer();
        }

        private void Command_ThrowAwayDrawnCard()
        {
            DiscardedCard = true;
            GameModel.CurrentDeck.LastPlayedCard = DrawnCard.CardObject;
            DrawnCard = null;
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Le joueur a jeté la carte pioché");
        }

        private void Command_TurnOverFirstTwoCards()
        {
            ViewModelCard? firstSelectedCard = null;
            ViewModelCard? secondSelectedCard = null;
            for (int i = 0; i < CurrentPlayerGrid.RowCount; i++)
            {
                for (int y = 0; y < CurrentPlayerGrid.ColumnCount; y++)
                {
                    if (CurrentPlayerGrid.GetCard(i, y) != null)
                    {
                        if (CurrentPlayerGrid.GetCard(i, y).IsSelected)
                        {
                            if (firstSelectedCard != null && secondSelectedCard !=null)
                            {
                                Logger.Instance.Log(Logger.ELevelMessage.Warning, "Vous ne pouvez pas retourner plus de deux cartes au début de la partie");
                                return;
                            }
                            else if (firstSelectedCard == null)
                            {
                                firstSelectedCard = CurrentPlayerGrid.GetCard(i, y);
                            }
                            else if (secondSelectedCard == null)
                            {
                                secondSelectedCard = CurrentPlayerGrid.GetCard(i, y);
                            }
                        }
                    }
                }
            }
            if (firstSelectedCard == null || secondSelectedCard == null)
            {
                Logger.Instance.Log(Logger.ELevelMessage.Warning, "Il faut sélectionner deux cartes");
                return;
            }

            Logger.Instance.Log(Logger.ELevelMessage.Info, "Découverte des deux premières cartes pour "+CurrentPlayerName);
            firstSelectedCard.ChangeVisibility(true);
            secondSelectedCard.ChangeVisibility(true);

            SendToNextPlayer();
        }

        private void SendToNextPlayer()
        {
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Passage au joueur suivant");

            if (_player != null)
                _player.IsPlayerTurn = false;

            UnselectAllCard();
            DisplayUpdate();
            _navigationService.NextPlayer(GameModel);
        }
        #endregion

        #region Public Methods
        #endregion

        #endregion
    }
}
