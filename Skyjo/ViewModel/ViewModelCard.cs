using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Data.SkyjoData;

namespace Skyjo.ViewModel
{
    public class ViewModelCard : ViewModelBase
    {
        #region Enum
        #endregion

        #region Constants
        private const string DEFAULT_CARD_COLOR = "Black";
        #endregion

        #region Events
        #endregion

        #region Field et Properties
        private Card _cardObject;

        /// <summary>
        /// Color of the card
        /// </summary>
        public string Color
        {
            get 
            { 
                if (_cardObject.IsVisible)
                    return _cardObject.Color;
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
                if (_cardObject.IsVisible)
                    return _cardObject.Value.ToString();
                else
                    return String.Empty;
            }
        }


        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="card"></param>
        public ViewModelCard (Card card)
        {
            _cardObject = card;
            RaisePropertyChanged(nameof(Color));
            RaisePropertyChanged(nameof(Value));
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        #endregion

        #region Public Methods
        #endregion

        #endregion
    }
}
