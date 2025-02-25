using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Core;
using Core.RelayCommand;
using Data.SkyjoData;

namespace Skyjo.ViewModel
{
    public class ViewModelCard : ViewModelBase
    {
        #region Enum
        #endregion

        #region Constants
        private const string DEFAULT_CARD_COLOR = "LightGray";
        #endregion

        #region Events
        public ICommand SelectCardCommand { get; }
        #endregion

        #region Field et Properties
        public Card CardObject;

        /// <summary>
        /// Color of the card
        /// </summary>
        public string Color
        {
            get 
            { 
                if (CardObject.IsVisible)
                    return CardObject.Color;
                else
                    return DEFAULT_CARD_COLOR;
            }
        }

        /// <summary>
        /// Value of the card
        /// </summary>
        public string Value
        {
            get 
            {
                if (CardObject.IsVisible)
                    return CardObject.Value.ToString();
                else
                    return String.Empty;
            }
        }

        private bool _isSelected;
        /// <summary>
        /// Check if card is selected
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        public  readonly Tuple<int, int> Position;


        private string _isInGame;
        public string IsInGame
        {
            get {  return _isInGame; }
            set
            {
                _isInGame = value;
                RaisePropertyChanged(nameof(IsInGame));
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="card"></param>
        public ViewModelCard (Card card, int i, int y)
        {
            CardObject = card;
            SelectCardCommand = new RelayCommand(ToggleSelection);
            RaisePropertyChanged(nameof(Color));
            RaisePropertyChanged(nameof(Value));
            Position = new Tuple<int, int> (i, y);
            IsInGame = "Visible";
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        #endregion

        #region Public Methods
        /// <summary>
        /// Toggle selection
        /// </summary>
        public void ToggleSelection()
        {
            IsSelected = !IsSelected;
        }

        /// <summary>
        /// Change visibility of card
        /// </summary>
        /// <param name="visible"></param>
        public void ChangeVisibility (bool visible)
        {
            CardObject.IsVisible = visible;
            RaisePropertyChanged(nameof(Color));
            RaisePropertyChanged(nameof(Value));
        }
        #endregion

        #endregion
    }
}
