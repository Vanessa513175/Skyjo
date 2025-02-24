using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Core;
using Data.PlayersData;
using Data.SkyjoData;

namespace Skyjo.ViewModel
{
    public class ViewModelPlayerGrid : ViewModelBase
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        #endregion

        #region Field et Properties
        private PlayerGrid _playerGrid;

        /// <summary>
        /// The row count
        /// </summary>
        public int RowCount => _playerGrid.Cards.GetLength(0);

        /// <summary>
        /// The column count
        /// </summary>
        public int ColumnCount => _playerGrid.Cards.GetLength(1);

        /// <summary>
        /// List of view model cards
        /// </summary>
        public ObservableCollection<ViewModelCard> Cards { get; }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="grid"></param>
        public ViewModelPlayerGrid(PlayerGrid grid)
        {
            _playerGrid = grid;
            Cards = [];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    var c = _playerGrid.GetCard(i, j);
                    if (c != null)
                        Cards.Add(new ViewModelCard(c, i, j));
                }
            }
            DisplayUpdate();
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        /// <summary>
        /// Display update
        /// </summary>
        private void DisplayUpdate()
        {
            RaisePropertyChanged(nameof(RowCount));
            RaisePropertyChanged(nameof(ColumnCount));
            RaisePropertyChanged(nameof(Cards));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get a specific card
        /// </summary>
        /// <param name="i"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public ViewModelCard? GetCard(int i, int y)
        {
            foreach (ViewModelCard vmc in Cards)
            {
                if (vmc.Position.Item1 == i && vmc.Position.Item2 == y)
                {
                    return vmc;
                }
            }
            return null;
        }

        /// <summary>
        /// Set Card
        /// </summary>
        /// <param name="Card"></param>
        /// <param name="z"></param>
        /// <param name="a"></param>
        public void SetCard(ViewModelCard Card, int z, int a)
        {
            _playerGrid.SetCard(z, a, Card.CardObject);
            Cards.Clear();
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    var c = _playerGrid.GetCard(i, j);
                    if (c != null)
                        Cards.Add(new ViewModelCard(c, i, j));
                }
            }
            DisplayUpdate();
        }

        public void ChangeVisibility(bool visible, int i, int j)
        {
            _playerGrid.GetCard(i, j).IsVisible = visible;
            DisplayUpdate();
        }
        #endregion

        #endregion
    }
}
