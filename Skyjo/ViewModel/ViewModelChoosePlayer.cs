﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Core;
using Core.RelayCommand;
using Data.PlayersData;
using Data.Statistiques;
using Skyjo.WindowManager;
using NavigationService = Skyjo.WindowManager.NavigationService;

namespace Skyjo.ViewModel
{
    public class ViewModelChoosePlayer : ViewModelBase
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        #endregion

        #region Field et Properties
        private readonly NavigationService _navigationService;

        private ObservableCollection<Player> _knowPlayers;
        /// <summary>
        /// List of know players
        /// </summary>
        public ObservableCollection<Player> KnownPlayers
        {
            get { return _knowPlayers; }
            set
            {
                _knowPlayers = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Player> _playersToAdd;
        /// <summary>
        /// List of players to add
        /// </summary>
        public ObservableCollection<Player> PlayersToAdd
        {
            get { return _playersToAdd; }
            set
            {
                _playersToAdd = value;
                RaisePropertyChanged();
            }
        }

        private string _newPlayerName;
        /// <summary>
        /// String contains the new player name
        /// </summary>
        public string NewPlayerName
        {
            get { return _newPlayerName; }
            set
            {
                _newPlayerName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Check if can next command
        /// </summary>
        public bool CanNext
        {
            get { return PlayersToAdd.Count > 1; }
        }

        public ICommand AddPlayerCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand AddKnownPlayerCommand { get; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navService"></param>
        public ViewModelChoosePlayer(NavigationService navService)
        {
            _navigationService = navService;

            _newPlayerName = String.Empty;
            KnownPlayers = [];
            _playersToAdd = [];

            List<PlayerStat> listOldPlayers = navService.StatManager.LoadPlayerStats();
            foreach(PlayerStat playerStat in listOldPlayers)
            {
                KnownPlayers.Add(new Player(playerStat.Name, playerStat.Id, GameModel.DEFAULT_HEIGHT_MATRIX, GameModel.DEFAULT_WIDTH_MATRIX));
            }

            AddPlayerCommand = new RelayCommand(AddPlayer);
            NextCommand = new RelayCommand(Next);
            AddKnownPlayerCommand = new RelayCommandWithParameter<Player>(AddKnownPlayerToSelected);
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckIfPlayerExist(string name)
        {
            foreach(Player p in PlayersToAdd)
            {
                if (p.Name == name)
                    return true;
            }
            foreach (Player p in KnownPlayers)
            {
                if (p.Name == name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Add a player
        /// </summary>
        private void AddPlayer()
        {
            if (CheckIfPlayerExist(NewPlayerName))
            {
                Logger.Instance.Log(Logger.ELevelMessage.Error, "Le joueur "+ NewPlayerName + " existe deja");
                return;
            } 

            if (!string.IsNullOrWhiteSpace(NewPlayerName))
            {
                var newPlayer = new Player(NewPlayerName, GameModel.DEFAULT_HEIGHT_MATRIX, GameModel.DEFAULT_WIDTH_MATRIX);
                PlayersToAdd.Add(newPlayer);
                NewPlayerName = string.Empty;
                RaisePropertyChanged(nameof(CanNext));
            }
        }

        /// <summary>
        /// Save new players in file
        /// </summary>
        private void SaveNewPlayers()
        {
            bool isPlayerWrite;
            foreach (Player p in PlayersToAdd)
            {
                isPlayerWrite = false;
                foreach (Player p2 in KnownPlayers)
                {
                    if (p == p2)
                    {
                        isPlayerWrite = true;
                    }
                }
                if (!isPlayerWrite)
                {
                    CreatePlayerInFile(p);
                }
            }
        }

        /// <summary>
        /// Create Player In File
        /// </summary>
        /// <param name="p"></param>
        private void CreatePlayerInFile(Player p)
        {
            PlayerStat playerStat = new PlayerStat(p.Name);
            _navigationService.StatManager.CreatePlayer(playerStat);
        }

        /// <summary>
        /// Next command
        /// </summary>
        private void Next()
        {
            if (CanNext)
            {
                SaveNewPlayers();
                Logger.Instance.Log(Logger.ELevelMessage.Info, "Création d'une partie avec " + PlayersToAdd.Count + " joueurs");
                _navigationService.ShareGameModel.Players = [.. PlayersToAdd];

                Logger.Instance.Log(Logger.ELevelMessage.Info, "Distibution des cartes");
                _navigationService.ShareGameModel.DrawCardToPlayers();

                _navigationService.NavigateTo("PlayerWindow", PlayersToAdd.FirstOrDefault().PlayerId);
            }
            else
            {
                Logger.Instance.Log(Logger.ELevelMessage.Warning, "Il faut au moins 2 joueurs pour créer une partie");
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add Known Player To Selected
        /// </summary>
        /// <param name="player"></param>
        public void AddKnownPlayerToSelected(Player player)
        {
            if (player != null && !PlayersToAdd.Contains(player))
            {
                PlayersToAdd.Add(player);
            }

        }
        #endregion

        #endregion
    }
}
