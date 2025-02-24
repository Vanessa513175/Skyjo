using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Data.PlayersData;

namespace Data.AutoGame
{
    public class ComputerPlayer
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        #endregion

        #region Field et Properties
        #endregion

        #region Constructor
        #endregion

        #region Methods

        #region Private and Protected Methods
        #endregion

        #region Public Methods
        public PlayerGrid ReturnTwoCards(PlayerGrid grid)
        {
            Random random = new Random();
            int row = random.Next(0, grid.Width-1);
            int column = random.Next(0, grid.Height-1);

            if (grid.GetCard(row, column) != null)
                grid.GetCard(row, column).IsVisible = true;
            else
                Logger.Instance.Log(Logger.ELevelMessage.Error, "Impossible de retourner : "+row + " " + column);

            random = new Random();
            row = random.Next(0, grid.Width - 1);
            column = random.Next(0, grid.Height - 1);
            if (grid.GetCard(row, column) != null)
                grid.GetCard(row, column).IsVisible = true;
            else
                Logger.Instance.Log(Logger.ELevelMessage.Error, "Impossible de retourner : " + row + " " + column);

            return grid;
        }
        #endregion

        #endregion
    }
}
