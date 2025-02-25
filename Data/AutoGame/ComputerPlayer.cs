using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Data.PlayersData;
using Data.SkyjoData;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private bool IsMostVisibleCard(PlayerGrid grid)
        {
            int totalCard = 0;
            int visibleCard = 0;

            for (int j = 0; j < grid.Width; j++)
            {
                for (int i = 0; i < grid.Height; i++)
                {
                    totalCard++;
                    if (grid.GetCard(i, j).IsVisible)
                    {
                        visibleCard++;
                    }
                }
            }
            return visibleCard > totalCard;
        }
        private int CheckIfNumberIsInLine(PlayerGrid grid, int number)
        {
            // for each line
            int nbrSameValue;
            for (int j = 0; j < grid.Width; j++)
            {
                nbrSameValue = 0;
                for (int i = 0; i < grid.Height; i++) 
                {
                    if (number == grid.GetCard(i, j).Value)
                    {
                        nbrSameValue++;
                    }
                }
                if (nbrSameValue > grid.Width - 1)
                {
                    return j;
                }
            }
            return -1;
        }

        private int CheckIfNumberIsInColumn(PlayerGrid grid, int number)
        {
            // for each column
            int nbrSameValue;
            for (int i = 0; i < grid.Height; i++)
            {
                nbrSameValue = 0;
                for (int j = 0; j < grid.Width; j++)
                {
                    if (number == grid.GetCard(i, j).Value)
                    {
                        nbrSameValue++;
                    }
                }
                if (nbrSameValue> grid.Width-1)
                {
                    return i;
                }
            }
            return -1;
        }

        private Tuple<PlayerGrid, Card>? CheckIfNumberCanReplaceAVisibleCard(PlayerGrid grid, Card card)
        {
            Tuple<int, int> bestTuple = new Tuple<int, int>(-1, -1);
            int bestDiff = int.MinValue;
            int diff = 0;
            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    if (grid.GetCard(i, j).IsVisible)
                    {
                        diff = grid.GetCard(i, j).Value - card.Value;
                        if (diff > bestDiff)
                        {
                            bestDiff = diff;
                            bestTuple = new Tuple<int, int>(i, j);
                        }
                    }
                }
            }
            if (bestDiff > 3)
            {
                Card oldCard = grid.GetCard(bestTuple.Item1, bestTuple.Item2);
                card.IsVisible = true;
                grid.SetCard(bestTuple.Item1, bestTuple.Item2, card);
                return new Tuple<PlayerGrid, Card>(grid, oldCard);
            }
            return null;
        }

        private Tuple<PlayerGrid, Card>? CheckIfCardCanReaplceAHiddenCard(PlayerGrid grid, Card card)
        {
            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    if (!grid.GetCard(i,j).IsVisible)
                    {
                        Card oldCard = grid.GetCard(i, j);
                        card.IsVisible = true;
                        grid.SetCard(i, j, card);
                        return new Tuple<PlayerGrid, Card>(grid, oldCard);
                    }
                }
            }
            return null;
        }

        private PlayerGrid ReturnACard(PlayerGrid grid)
        {
            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    if (!grid.GetCard(i, j).IsVisible)
                    {
                        grid.GetCard(i, j).IsVisible = true;
                        return grid;
                     }
                }
            }
            return grid;
        }

        private Tuple<PlayerGrid, Card>? CheckLineColumnSameNumber(PlayerGrid grid, Card card)
        {
            int line = CheckIfNumberIsInLine(grid, card.Value);
            if (line != -1)
            {
                for (int i = 0; i < grid.Height; i++)
                {
                    if (card.Value != grid.GetCard(i, line).Value)
                    {
                        Card oldCard = grid.GetCard(i, line);
                        card.IsVisible = true;
                        grid.SetCard(i, line, card);
                        return new Tuple<PlayerGrid, Card>(grid, oldCard);
                    }
                }
            }
            else
            {
                int column = CheckIfNumberIsInColumn(grid, card.Value);
                if (column != -1)
                {
                    for (int j = 0; j < grid.Width; j++)
                    {
                        if (card.Value != grid.GetCard(column, j).Value)
                        {
                            Card oldCard = grid.GetCard(column, j);
                            card.IsVisible = true;
                            grid.SetCard(column, j, card);
                            return new Tuple<PlayerGrid, Card>(grid, oldCard);
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
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

        public Tuple<PlayerGrid,Card> PlayATurn(PlayerGrid grid, Card card) 
        {
            Tuple<PlayerGrid, Card> tuple;
            tuple = CheckLineColumnSameNumber(grid, card);
            if (tuple != null)
            {
                return tuple;
            }

            // check number of visible card
            if (grid.GetCurrentScore() < 8 || !IsMostVisibleCard(grid) || grid.GetCurrentScore() > 20)
            {

                tuple = CheckIfNumberCanReplaceAVisibleCard(grid, card);
                if (tuple != null)
                {
                    return tuple;
                }
            }
            if (card.Value < 6)
            {
                tuple = CheckIfCardCanReaplceAHiddenCard(grid, card);
                if (tuple != null)
                {
                    return tuple;
                }
            }
            grid = ReturnACard(grid);
            return new Tuple<PlayerGrid, Card>(grid, card);
        }
        #endregion

        #endregion
    }
}
