using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Core;
using Data.PlayersData;
using Data.Statistiques;
using Skyjo.Window;

namespace Skyjo.WindowManager
{
    public class NavigationService
    {
        private const string FILE_PATH_FOR_STAT = "D:\\SkyjoData\\Statistiques\\stats.json";

        public event Action<GameModel>? GameModelUpdated;

        private System.Windows.Window? _currentWindow;

        public GameModel ShareGameModel;

        public FileManager StatManager;

        public NavigationService(System.Windows.Window currentWindow)
        {
            _currentWindow = currentWindow;
            ShareGameModel = new GameModel();
            StatManager = new FileManager(FILE_PATH_FOR_STAT);
        }

        public NavigationService()
        {
            _currentWindow = null;
            ShareGameModel = new GameModel();
            StatManager = new FileManager(FILE_PATH_FOR_STAT);
        }

        public void NextPlayer(GameModel gameModel)
        {
            ShareGameModel = gameModel;
            if (ShareGameModel.CheckIfPlayerFinish())
            {
                ShareGameModel.GamePhase = GameModel.EGamePhase.End;
                ShareGameModel.ReturnAllCards();
                Player winner = ShareGameModel.GetPlayerWithBestScore();

                foreach (Player p in ShareGameModel.Players)
                {
                    if (p.Name == winner.Name)
                    {
                        StatManager.UpdatePlayerStat(p, true);
                    }
                    else
                    {
                        StatManager.UpdatePlayerStat(p, false);
                    }
                }
                
                Logger.Instance.Log(Logger.ELevelMessage.Warning, "---- FIN DE LA PARTIE ----");
                Logger.Instance.Log(Logger.ELevelMessage.Warning, "Le joueur " + winner.Name + " a gagné !!");
                GameModelUpdated?.Invoke(ShareGameModel);
            }
            else
            {
                ShareGameModel.Next();

                GameModelUpdated?.Invoke(ShareGameModel);

                if (ShareGameModel.CurrentPlayerIndex != 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ShareGameModel = ShareGameModel.PlayAutoTurn();
                        NextPlayer(ShareGameModel);
                    }), System.Windows.Threading.DispatcherPriority.Background);
                }
            }

            
            
        }

        /// <summary>
        /// Restart game
        /// </summary>
        public void RestartGame()
        {
            ShareGameModel.RestartGame();
        }


        public void NavigateTo(string viewName, Guid playerId = default)
        {
            System.Windows.Window? nextWindow = null;

            switch (viewName)
            {
                case "Menu":
                    break;
                case "ChoosePlayers":
                    nextWindow = new ViewChoosePlayers(this);
                    break;
                case "StatsWindow":
                    nextWindow = new StatsWindow(this);
                    break;
                case "PlayerWindow":
                    nextWindow = new ViewPlayerWindow(this, playerId);
                    break;
                default:
                    throw new ArgumentException("Vue non prise en charge");
            }

            if (nextWindow != null || viewName == "Menu")
            {
                if (nextWindow != null)
                    nextWindow.Show();
                _currentWindow?.Close();
                _currentWindow = nextWindow;
                nextWindow = null;
            }
        }
    }
}
