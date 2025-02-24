using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.SkyjoData;

namespace Data.PlayersData
{
    public class PlayerGrid
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        #endregion

        #region Fields et Properties

        public readonly int Height;
        public readonly int Width;
        /// <summary>
        /// The matrix cards
        /// </summary>
        public Card[,] Cards { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public PlayerGrid(int height, int width)
        {
            Height = height;
            Width = width;
            Cards = new Card[height, width];
        }

        #endregion

        #region Methods

        #region Private and Protected Methods
        #endregion

        #region Public Methods
        /// <summary>
        /// Set a card
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="card"></param>
        public void SetCard(int row, int col, Card card)
        {
            if (row >= 0 && row < Height && col >= 0 && col < Width)
            {
                Cards[row, col] = card;
            }
        }

        /// <summary>
        /// Set visibility of a card
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="visible"></param>
        public void SetVisibility(int row, int col, bool visible)
        {
            if (row >= 0 && row < Height && col >= 0 && col < Width)
            {
                Cards[row, col].IsVisible = visible;
            }
        }

        /// <summary>
        /// Get a card
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public Card? GetCard(int row, int col)
        {
            if (row >= 0 && row < Height && col >= 0 && col < Width)
            {
                return Cards[row, col];
            }
            return null;
        }

        /// <summary>
        /// Get the string of the grid
        /// </summary>
        public String PrintGrid()
        {
            string result = "";
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    result+=($"{Cards[i, j].ToString}\t");
                }
                result += "\n";
            }
            return result;
        }

        /// <summary>
        /// Clear matrix
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Clear()
        {
            Cards = new Card[Height, Width];
        }

        /// <summary>
        /// Get the current score of the matrice
        /// </summary>
        /// <returns></returns>
        public int GetCurrentScore()
        {
            int score = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Cards[i, j].IsVisible)
                        score += Cards[i, j].Value;
                }
            }
            return score;
        }
        #endregion

        #endregion
    }

}
