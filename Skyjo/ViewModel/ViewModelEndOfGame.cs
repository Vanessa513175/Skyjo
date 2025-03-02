using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Core;
using Core.RelayCommand;
using Skyjo.WindowManager;

namespace Skyjo.ViewModel
{
    public class ViewModelEndOfGame : ViewModelBase
    {
        #region Enum
        #endregion

        #region Constants
        private const Visibility LABEL_VISIBLE = Visibility.Visible;
        private const Visibility LABEL_COLLAPSED = Visibility.Collapsed;
        #endregion

        #region Events
        #endregion

        #region Field et Properties
        private Visibility _endOfGame;
        /// <summary>
        /// Visibility of user control
        /// </summary>
        public Visibility EndOfGame
        {
            get { return _endOfGame; }
            set
            {
                _endOfGame = value;
                RaisePropertyChanged();
            }
        }

        private string _firstText;
        public string FirstText
        {
            get { return _firstText; }
            set
            {
                _firstText = value;
                RaisePropertyChanged();
            }
        }

        private string _secondText;
        public string SecondText
        {
            get { return _secondText; }
            set
            {
                _secondText = value;
                RaisePropertyChanged();
            }
        }

        private bool _isControlEnabled;
        public bool IsControlEnabled 
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ReplayGame { get; private set; }
        public ICommand ReturnToMenuCommand { get; private set; }

        private NavigationService _navService;
        private Guid _currentPlayerId;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewModelEndOfGame(NavigationService nav)
        {
            _navService = nav;

            EndOfGame = LABEL_COLLAPSED;
            IsControlEnabled = false;
            FirstText = String.Empty;
            SecondText = String.Empty;

            ReplayGame = new RelayCommand(Command_ReplayGame);
            ReturnToMenuCommand = new RelayCommand(Command_ReturnToMenuCommand);
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        /// <summary>
        /// Execute the ReplayGame command
        /// </summary>
        private void Command_ReplayGame()
        {
            Clean();
            _navService.RestartGame();
            _navService.NavigateTo("PlayerWindow", _currentPlayerId);
        }

        /// <summary>
        /// Execute the ReturnToMenuCommand command
        /// </summary>
        private void Command_ReturnToMenuCommand()
        {
            Clean();
            _navService.NavigateTo("Menu");
        }

        /// <summary>
        /// Clean the user control
        /// </summary>
        private void Clean()
        {
            EndOfGame = LABEL_COLLAPSED;
            FirstText = String.Empty;
            SecondText = String.Empty;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Notify end of game
        /// </summary>
        /// <param name="nameWinner"></param>
        public void IsEndOfGame(string nameWinner, int score, Guid currentPlayerId)
        {
            EndOfGame = LABEL_VISIBLE;
            IsControlEnabled = true;
            if (score== null || score==int.MinValue)
            {
                Logger.Instance.Log(Logger.ELevelMessage.Error, "Problème de score : "+ score.ToString());
            }

            FirstText = "Fin de la Partie !";
            SecondText = "Le gagnant est "+ nameWinner + " avec un score de " + score.ToString();
            _currentPlayerId = currentPlayerId;
        }
        #endregion

        #endregion
    }
}
