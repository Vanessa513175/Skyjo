using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Data.PlayersData;
using Skyjo.ViewModel;

namespace Skyjo.Window
{
    /// <summary>
    /// Logique d'interaction pour ViewPlayerWindow.xaml
    /// </summary>
    public partial class ViewPlayerWindow : System.Windows.Window
    {
        public ViewPlayerWindow(Skyjo.WindowManager.NavigationService nav, Guid playerId)
        {
            InitializeComponent();
            DataContext = new ViewModelPlayerWindow(nav, playerId);
        }
    }
}
