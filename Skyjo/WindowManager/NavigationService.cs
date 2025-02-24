using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Data.PlayersData;
using Skyjo.Window;

namespace Skyjo.WindowManager
{
    public class NavigationService
    {
        public event Action<GameModel>? GameModelUpdated;

        private System.Windows.Window? _currentWindow;

        public GameModel ShareGameModel;

        public NavigationService(System.Windows.Window currentWindow)
        {
            _currentWindow = currentWindow;
            ShareGameModel = new GameModel();
        }

        public NavigationService()
        {
            _currentWindow = null;
            ShareGameModel = new GameModel();
        }

        public void NextPlayer(GameModel gameModel)
        {
            ShareGameModel = gameModel;
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


        public void NavigateTo(string viewName, Guid playerId = default)
        {
            System.Windows.Window? nextWindow = null;

            switch (viewName)
            {
                case "ChoosePlayers":
                    nextWindow = new ViewChoosePlayers(this);
                    break;
                //case "StatsWindow":
                //    nextWindow = new StatsWindow();
                //    break;
                case "PlayerWindow":
                    nextWindow = new ViewPlayerWindow(this, playerId);
                    break;
                default:
                    throw new ArgumentException("Vue non prise en charge");
            }

            if (nextWindow != null)
            {
                nextWindow.Show();
                _currentWindow?.Close();
                _currentWindow = nextWindow;
                nextWindow = null;
            }
        }
    }
}
