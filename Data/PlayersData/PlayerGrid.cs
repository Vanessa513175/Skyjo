using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
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
        /// <summary>
        /// Delete a specific column
        /// </summary>
        /// <param name="i"></param>
        private void DeleteColumn(int i)
        {
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Vous avez le même chiffre sur une colonne ! Suppression de la colonne");

            for (int j = 0; j < Height; j++)
            {
                for (int k = i; k < Width - 1; k++)
                {
                    Cards[j, k] = Cards[j, k + 1];
                }
                Cards[j, Width - 1] = new Card(int.MaxValue) { IsVisible = false, IsInGame = false };
            }

            CheckIfNeedToDeleteLineOrColumn();
        }


        /// <summary>
        /// Delete a specific Line
        /// </summary>
        /// <param name="j"></param>
        private void DeleteLine(int j)
        {
            Logger.Instance.Log(Logger.ELevelMessage.Info, "Vous avez le même chiffre sur une ligne ! Suppression de la ligne");

            for (int i = 0; i < Width; i++)
            {
                for (int k = j; k < Height - 1; k++)
                {
                    Cards[k, i] = Cards[k + 1, i];
                }
                Cards[Height - 1, i] = new Card(int.MaxValue) { IsVisible = false, Color = "Pink" };
            }

            CheckIfNeedToDeleteLineOrColumn();
        }

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

        /// <summary>
        /// Check If Need To Delete Line Or Column
        /// </summary>
        public void CheckIfNeedToDeleteLineOrColumn()
        {
            // Vérification des colonnes
            for (int i = 0; i < Width; i++) // Boucle sur les colonnes
            {
                bool needToDeleteColumn = true;

                for (int j = 0; j < Height - 1; j++) // Boucle sur les lignes
                {
                    Card current = GetCard(j, i);
                    Card next = GetCard(j + 1, i);

                    if (current == null || next == null || !current.IsVisible || !next.IsVisible ||current.Value == int.MaxValue || next.Value == int.MaxValue || current.Value != next.Value || current.Value<=0 || next.Value<=0)
                    {
                        needToDeleteColumn = false;
                        break;
                    }
                }

                if (needToDeleteColumn)
                {
                    DeleteColumn(i);
                    return;
                }
            }

            // Vérification des lignes
            for (int j = 0; j < Height; j++) // Boucle sur les lignes
            {
                bool needToDeleteLine = true;

                for (int i = 0; i < Width - 1; i++) // Boucle sur les colonnes
                {
                    Card current = GetCard(j, i);
                    Card next = GetCard(j, i + 1);

                    if (current == null || next == null || !current.IsVisible || !next.IsVisible || current.Value == int.MaxValue || next.Value == int.MaxValue || current.Value != next.Value || current.Value <= 0 || next.Value <= 0)
                    {
                        needToDeleteLine = false;
                        break;
                    }
                }

                if (needToDeleteLine)
                {
                    DeleteLine(j);
                    return;
                }
            }
        }

        #endregion

        #endregion
    }

}
