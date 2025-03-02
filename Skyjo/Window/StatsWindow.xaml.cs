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
using System.Windows.Shapes;
using Skyjo.ViewModel;

namespace Skyjo.Window
{
    /// <summary>
    /// Logique d'interaction pour StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : System.Windows.Window
    {
        public StatsWindow(Skyjo.WindowManager.NavigationService nav)
        {
            InitializeComponent();
            DataContext = new ViewModelStatsWindow(nav);
        }
    }
}
