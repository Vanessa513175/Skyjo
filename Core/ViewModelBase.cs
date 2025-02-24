using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Enum
        #endregion

        #region Constants
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Field et Properties
        #endregion

        #region Constructor
        #endregion

        #region Methods

        #region Private and Protected Methods
        #endregion

        #region Public Methods
        #endregion

        #endregion

    }
}
