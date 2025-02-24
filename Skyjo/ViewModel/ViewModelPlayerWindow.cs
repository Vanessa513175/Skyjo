using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
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
        #endregion

        #region Field et Properties

        #region CurrentPlayer
        private readonly Guid _playerId;
        private readonly Player? _player;

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
            _navigationService = navService;
            _playerId = playerId;

            _logText = String.Empty;

            //POUR TEST
            Random random = new Random();

            // List of players
            List<Player> otherPlayers = new List<Player>();
            foreach (Player p in navService.ShareGameModel.Players)
            {
                int randomRow = random.Next(0, 3);
                int randomColumn = random.Next(0, 3);

                p.PlayerGrid.SetVisibility(0, randomRow, true);
                p.PlayerGrid.SetVisibility(2, randomColumn, true);
                if (p.PlayerId == playerId)
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

            // Local Game Model
            GameModel = navService.ShareGameModel;

            Logger.Instance.LogUpdated += OnLogUpdated;

            DisplayUpdate();
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
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
        /// Display update
        /// </summary>
        private void DisplayUpdate()
        {
            RaisePropertyChanged(nameof(CurrentPlayerName));
            RaisePropertyChanged(nameof(CurrentPlayerCardScore));
            RaisePropertyChanged(nameof(CurrentPlayerGrid));

            RaisePropertyChanged(nameof(Player2Name));
            RaisePropertyChanged(nameof(Player2CardScore));
            RaisePropertyChanged(nameof(Player2Grid));

            RaisePropertyChanged(nameof(Player3Name));
            RaisePropertyChanged(nameof(Player3CardScore));
            RaisePropertyChanged(nameof(Player3Grid));

            RaisePropertyChanged(nameof(Player4Name));
            RaisePropertyChanged(nameof(Player4CardScore));
            RaisePropertyChanged(nameof(Player4Grid));
        }
        private void OnLogUpdated(string newLog)
        {
            _logBuilder.AppendLine(newLog);
            LogText = _logBuilder.ToString();
        }
        #endregion

        #region Public Methods
        #endregion

        #endregion
    }
}
