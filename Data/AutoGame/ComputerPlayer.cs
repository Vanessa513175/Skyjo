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
            int totalCard = 0;
            int visibleCard = 0;

            // for each line
            int nbrSameValue;
            for (int j = 0; j < grid.Width; j++)
            {
                nbrSameValue = 0;
                for (int i = 0; i < grid.Height; i++) 
                {
                    totalCard++;
                    if (grid.GetCard(i, j).IsVisible)
                    {
                        visibleCard++;
                    }
                    if (number == grid.GetCard(i, j).Value)
                    {
                        nbrSameValue++;
                    }
                }
                if (nbrSameValue >= grid.Width - 1)
                {
                    return j;
                }
            }
            return -1;
        }

        private int CheckIfNumberIsInColumn(PlayerGrid grid, int number)
        {
            int totalCard = 0;
            int visibleCard = 0;
            // for each column
            int nbrSameValue;
            for (int i = 0; i < grid.Height; i++)
            {
                nbrSameValue = 0;
                for (int j = 0; j < grid.Width; j++)
                {
                    totalCard++;
                    if (grid.GetCard(i, j).IsVisible)
                    {
                        visibleCard++;
                    }
                    if (number == grid.GetCard(i, j).Value)
                    {
                        nbrSameValue++;
                    }
                }
                if (nbrSameValue == 1)
                {
                    int nbrCardNoVisible = totalCard - visibleCard;
                    if (nbrCardNoVisible > 5 && number >= 7)
                    {
                        return i;
                    }
                }
                if (nbrSameValue>= grid.Height - 1)
                {
                    return i;
                }
            }
            return -1;
        }

        private Tuple<PlayerGrid, Card>? CheckIfNumberCanReplaceAVisibleCard(PlayerGrid grid, Card card)
        {
            if (card.Value > 6)
            {
                return null;
            }
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
            if (bestDiff > 5)
            {
                if (grid.GetCard(bestTuple.Item1, bestTuple.Item2).Value <= card.Value)
                {
                    return null;
                }
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
                    if (card.Value == grid.GetCard(i, line).Value)
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
                        if (card.Value == grid.GetCard(column, j).Value)
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

        private Tuple<PlayerGrid, Deck>? MiddleCardIsGood(PlayerGrid grid, Deck deck)
        {
            var tuple = CheckLineColumnSameNumber(grid, deck.LastPlayedCard);
            if (tuple != null)
            {
                deck.LastPlayedCard = tuple.Item2;
                return new Tuple<PlayerGrid, Deck>(tuple.Item1, deck);
            }
            else
            {
                if (deck.LastPlayedCard.Value < 5)
                {
                    var tuple2 = CheckIfNumberCanReplaceAVisibleCard(grid, deck.LastPlayedCard);
                    if (tuple2 != null)
                    {
                        deck.LastPlayedCard = tuple2.Item2;
                        return new Tuple<PlayerGrid, Deck>(tuple2.Item1, deck);
                    }
                    else
                    {
                        var tuple3 = CheckIfCardCanReaplceAHiddenCard(grid, deck.LastPlayedCard);
                        if (tuple3 != null)
                        {
                            deck.LastPlayedCard = tuple3.Item2;
                            return new Tuple<PlayerGrid, Deck>(tuple3.Item1, deck);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Public Methods
        public PlayerGrid ReturnTwoCards(PlayerGrid grid)
        {
            Random random = new Random();

            int row1 = random.Next(0, grid.Width - 1);
            int column1 = random.Next(0, grid.Height - 1);

            if (grid.GetCard(row1, column1) != null)
                grid.GetCard(row1, column1).IsVisible = true;
            else
                Logger.Instance.Log(Logger.ELevelMessage.Error, "Impossible de retourner : " + row1 + " " + column1);

            int row2, column2;
            do
            {
                row2 = random.Next(0, grid.Width - 1);
                column2 = random.Next(0, grid.Height - 1);
            }
            while (row1 == row2 && column1 == column2);

            if (grid.GetCard(row2, column2) != null)
                grid.GetCard(row2, column2).IsVisible = true;
            else
                Logger.Instance.Log(Logger.ELevelMessage.Error, "Impossible de retourner : " + row2 + " " + column2);

            return grid;
        }


        public Tuple<PlayerGrid,Deck> PlayATurn(PlayerGrid grid, Deck deck) 
        {
            Tuple<PlayerGrid, Deck> tuple;

            //Check if middle card is good
            var tupleWithMiddle = MiddleCardIsGood(grid, deck);
            if (tupleWithMiddle != null)
            {
                Logger.Instance.Log(Logger.ELevelMessage.Warning, $"Joueur a échangé carte du milieu car good");
                return tupleWithMiddle;
            }
            Card card = deck.DrawCard();
            Tuple<PlayerGrid, Card> tupleBis;
            tupleBis = CheckLineColumnSameNumber(grid, card);
            if (tupleBis != null)
            {
                deck.LastPlayedCard = tupleBis.Item2;
                tuple = new Tuple<PlayerGrid, Deck>(tupleBis.Item1, deck);
                return tuple;
            }

            // check number of visible card
            if (grid.GetCurrentScore() < 8 || !IsMostVisibleCard(grid) || grid.GetCurrentScore() > 20)
            {

                tupleBis = CheckIfNumberCanReplaceAVisibleCard(grid, card);
                if (tupleBis != null)
                {
                    deck.LastPlayedCard = tupleBis.Item2;
                    tuple = new Tuple<PlayerGrid, Deck>(tupleBis.Item1, deck);
                    return tuple;
                }
            }
            if (card.Value < 6)
            {
                tupleBis = CheckIfCardCanReaplceAHiddenCard(grid, card);
                if (tupleBis != null)
                {
                    deck.LastPlayedCard = tupleBis.Item2;
                    tuple = new Tuple<PlayerGrid, Deck>(tupleBis.Item1, deck);
                    return tuple;
                }
            }
            grid = ReturnACard(grid);
            deck.LastPlayedCard = card;
            tuple = new Tuple<PlayerGrid, Deck>(grid, deck);
            return tuple;
        }
        #endregion

        #endregion
    }
}
