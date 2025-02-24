using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.SkyjoData
{
    public class Card
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        #endregion

        #region Field et Properties
        private string _color;
        /// <summary>
        /// Get the color of the card
        /// </summary>
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private int _value;
        /// <summary>
        /// Get the value of the card
        /// </summary>
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private bool _isVisible;
        /// <summary>
        /// Get if the card is visible
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="value"></param>
        public Card (int value)
        {
            _value = value;

            _isVisible = false;

            if (value >= -2 && value <= -1)
                _color = "DarkBlue";
            else if (value >= 1 && value <= 4)
                _color = "LightGreen";
            else if (value >= 5 && value <= 8)
                _color = "Yellow";
            else if (value >= 9 && value <= 12)
                _color = "OrangeRed";
            else if (value == 0)
                _color = "LightBlue";
            else
                _color = "DarkRed";
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        #endregion

        #region Public Methods
        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{_color} {_value}";
        }
        #endregion

        #endregion
    }
}
