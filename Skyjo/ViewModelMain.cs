using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Core;
using Core.RelayCommand;

namespace Skyjo
{
    public class ViewModelMain : ViewModelBase
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        public ICommand StartGameCommand { get; private set; }
        public ICommand ViewStatsCommand { get; private set; }
        public ICommand QuitCommand { get; private set; }
        #endregion

        #region Field et Properties
        private readonly Skyjo.WindowManager.NavigationService _navigationService;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewModelMain()
        {
            _navigationService = new Skyjo.WindowManager.NavigationService();

            StartGameCommand = new RelayCommand(StartGame);
            ViewStatsCommand = new RelayCommand(ViewStats);
            QuitCommand = new RelayCommand(Quit);
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        private void StartGame()
        {
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Lancement d'une partie");
            _navigationService.NavigateTo("ChoosePlayers");
        }

        private void ViewStats()
        {
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Visualisation des statistiques");
        }

        private void Quit()
        {
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Fermeture de l'application");
            Environment.Exit(0);
        }
        #endregion

        #region Public Methods
        #endregion

        #endregion
    }
}
